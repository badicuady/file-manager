import { Nullable, WritableProps } from 'tsdef';
import {
    ChonkyActions,
    ChonkyActionUnion,
    ChonkyIconName,
    FullFileBrowser,
    FileAction,
    FileArray,
    FileData,
    MapFileActionsToData,
    defineFileAction,
    FileViewMode
} from 'chonky';
import React from 'react';
import { IconTypeResponse, ItemResponse } from '../models/response/ListItemsResponse';
import GraphQLClient from '../services/Client';

interface IProps { }
interface IState { files: FileArray, folderChain: FileArray }
interface ICustomAction { id: string }
interface ICustomActions { [index: string]: ICustomAction }

class FileBrowser extends React.Component<IProps, IState> {
    readonly baseDirectory: string = 'root';
    readonly customActionIds: ICustomActions = {
        RenameItem: { id: 'rename_item' },
        CreateDirectory: { id: 'create_directory' },
        CopyItem: { id: 'copy_item' }
    };

    readonly fileActions: WritableProps<FileAction>[] = [
        ChonkyActions.UploadFiles,
        ChonkyActions.DownloadFiles,
        ChonkyActions.DeleteFiles,
    ];

    readonly client: GraphQLClient;
    readonly wrapper: React.RefObject<any>;
    readonly fileUpload: React.RefObject<any>;
    currentDirectory: string = '/';

    constructor(args: any[]) {
        super(args);

        this.wrapper = React.createRef();
        this.fileUpload = React.createRef();
        this.client = new GraphQLClient();
        this.state = {
            files: [],
            folderChain: [{ id: this.baseDirectory, name: this.baseDirectory, isDir: true }]
        };

        this.defineCustomActions();
    }

    async componentDidMount() {
        await this.loadCurrentDirectory();
    }

    render(): JSX.Element {
        return (
            <div style={{ height: 500 }} ref={this.wrapper}>
                <input type="file" 
                    ref={this.fileUpload} 
                    id="fileUpload" 
                    name="fileUpload" 
                    style={{visibility: 'hidden'}} 
                    onChange={async () => this.handleUploadFile()} />
                <FullFileBrowser
                    files={this.state.files}
                    folderChain={this.state.folderChain}
                    fileActions={this.fileActions}
                    onFileAction={async (evt: MapFileActionsToData<ChonkyActionUnion>): Promise<void> => await this.handleFileAction(evt)}
                    doubleClickDelay={300}
                    defaultFileViewActionId={FileViewMode.List}
                    disableDragAndDrop={true}
                />
            </div>
        );
    }

    //#region Custom Actions
    defineCustomActions() {
        const renameAction: WritableProps<FileAction> = defineFileAction({
            id: this.customActionIds.RenameItem.id,
            requiresSelection: true,
            button: {
                name: 'Rename item',
                toolbar: true,
                contextMenu: true,
                icon: ChonkyIconName.key
            },
        } as const);
        const createAction: WritableProps<FileAction> = defineFileAction({
            id: this.customActionIds.CreateDirectory.id,
            requiresSelection: false,
            button: {
                name: 'Create directory',
                toolbar: true,
                contextMenu: true,
                icon: ChonkyIconName.folder
            },
        } as const);
        const copyAction: WritableProps<FileAction> = defineFileAction({
            id: this.customActionIds.CopyItem.id,
            requiresSelection: true,
            button: {
                name: 'Copy item',
                toolbar: true,
                contextMenu: true,
                icon: ChonkyIconName.archive,
                group: 'Actions'
            },
        } as const);
        this.fileActions.splice(1, 0, createAction, renameAction, copyAction);
    }
    //#endregion

    //#region Handlers
    private async handleFileAction(evt: MapFileActionsToData<ChonkyActionUnion>): Promise<void> {
        if (evt.id === ChonkyActions.UploadFiles.id) {
            this.fileUpload.current.click();
        }

        if (evt.id === ChonkyActions.OpenFiles.id) {
            const selectedItem: FileData = evt.payload.targetFile ?? evt.payload.files[0];
            if (!selectedItem.isDir) { return; }
            await this.handleOpenDirectory(selectedItem);
        }

        if (evt.id === ChonkyActions.DeleteFiles.id) {
            const selectedItems: FileData[] = [...evt.state.selectedFilesForAction];
            await this.handleDeleteItems(selectedItems);
        }

        if (evt.id === ChonkyActions.DownloadFiles.id) {
            alert('Functionality not available yet. This function will be available in future releases!');
        }

        if (evt.id === this.customActionIds.RenameItem.id) {
            const selectedItem: FileData = evt.state.selectedFilesForAction[0];
            const newName: string | null = prompt(`What is the new name for your item (${selectedItem.name})?`, selectedItem?.name);
            await this.handleRenameItem(selectedItem, newName);
        }

        if (evt.id === this.customActionIds.CopyItem.id) {
            const selectedItem: FileData = evt.state.selectedFilesForAction[0];
            await this.handleCopyItem(selectedItem);
        }

        if (evt.id === this.customActionIds.CreateDirectory.id) {
            const newName: string | null = prompt(`What is the name for the directory?`, 'new_folder');
            await this.handleCreateDirectory(newName || 'new_folder');
        }
    }

    private async handleUploadFile(): Promise<void> {
        const files = this.fileUpload.current.files;
        if (files.length > 0) {
            const currentDirectory = this.getCurrenDirectory();
            const result = await this.uploadFile(currentDirectory, files[0].name, files[0]);
           
            if (result) {
                this.setState({ files: [...this.state.files, result] });
            }
        }
    }

    private async handleOpenDirectory(selectedItem: FileData): Promise<void> {
        const index = this.state.folderChain.findIndex(e => e?.id === selectedItem.id);
        if (index < 0) {
            this.currentDirectory = `${this.currentDirectory}${this.currentDirectory === '/' ? '' : '/'}${selectedItem.name}`;
            this.addFolderToFolderChain(selectedItem);
        } else {
            this.removeFolderFromFolderChain(index);
            this.currentDirectory = this.state.folderChain
                .reduce((acc: string, current: Nullable<FileData>) => `${acc}/${current?.name}`, '')
                .replace(`/${this.baseDirectory}`, '/');
        }

        await this.loadCurrentDirectory(this.currentDirectory);
    }

    private async handleRenameItem(selectedItem: FileData, newName: string | null): Promise<void> {
        if (!newName) { return; }
        if (selectedItem?.name.localeCompare(newName, undefined, { sensitivity: 'base' }) === 0) { return; }

        let result: boolean = false;
        if (selectedItem.isDir) { result = await this.handleRenameDirectory(selectedItem, newName); }
        if (!selectedItem.isDir) { result = await this.handleRenameFile(selectedItem, newName); }

        if (result) {
            const ndx: number = this.state.files.findIndex(e => e?.id === selectedItem.id);
            const newSelectedItem = { ...selectedItem, name: newName || "" };
            const files = [...this.state.files];
            files.splice(ndx, 1, newSelectedItem);
            this.setState({ files });
        }
    }

    private async handleRenameDirectory(selectedItem: FileData, newName: string | null): Promise<boolean> {
        const currentDirectory = this.getCurrenDirectory();
        return await this.renameDirectory(`${currentDirectory}/${selectedItem.name}`, newName || '');
    }

    private async handleRenameFile(selectedItem: FileData, newName: string | null): Promise<boolean> {
        const currentDirectory = this.getCurrenDirectory();
        return await this.renameFile(currentDirectory, selectedItem.name, newName || '');
    }

    private async handleDeleteItems(selectedItems: FileData[]): Promise<boolean> {
        const currentDirectory = this.getCurrenDirectory();

        let resultAll = true;
        for (let ndx = 0; ndx < selectedItems.length; ndx++) {
            const selectedItem = selectedItems[ndx];
            let result = false;
            if (selectedItem.isDir) { result = await this.deleteDirectory(currentDirectory, selectedItem.name); }
            if (!selectedItem.isDir) { result = await this.deleteFile(currentDirectory, selectedItem.name); }
            resultAll = resultAll && result;

            if (result) {
                const i: number = this.state.files.findIndex(e => e?.id === selectedItem.id);
                const files = [...this.state.files];
                files.splice(i, 1);
                this.setState({ files });
            }
        }

        return resultAll;
    }

    private async handleCopyItem(selectedItem: FileData): Promise<void> {
        let result: Nullable<FileData> = null;
        if (selectedItem.isDir) { result = await this.handleCopyDirectory(selectedItem); }
        if (!selectedItem.isDir) { result = await this.handleCopyFile(selectedItem); }

        if (result) {
            this.setState({ files: [...this.state.files, result] });
        }
    }

    private async handleCopyDirectory(selectedItem: FileData): Promise<Nullable<FileData>> {
        const currentDirectory = this.getCurrenDirectory();
        return await this.copyDirectory(`${currentDirectory}/${selectedItem.name}`, `${currentDirectory}/Copy of ${selectedItem.name}`);
    }

    private async handleCopyFile(selectedItem: FileData): Promise<Nullable<FileData>> {
        const currentDirectory = this.getCurrenDirectory();
        return await this.copyFile(currentDirectory, selectedItem.name, `Copy of ${selectedItem.name}`);
    }

    private async handleCreateDirectory(name: string): Promise<void> {
        const currentDirectory = this.getCurrenDirectory();
        let createdDirectory = await this.createDirectory(currentDirectory, name);
        if (createdDirectory) {
            this.setState({ files: [...this.state.files, createdDirectory] });
        }
    }

    //#endregion

    //#region Private methods

    private addFolderToFolderChain(file: FileData): void {
        this.setState({
            folderChain: [
                ...this.state.folderChain,
                file
            ]
        });
    }

    private removeFolderFromFolderChain(index: number): void {
        let folderChain: FileArray = [...this.state.folderChain.slice(0, index + 1)];
        this.setState({
            folderChain: [...folderChain]
        });
    }

    private async loadCurrentDirectory(path: string = '/'): Promise<void> {
        const items = await this.client.listItems(path);
        this.setState({
            files: items.data.items.map(this.mapItemResponse)
        })
    }

    private async renameDirectory(activeDirectory: string = '/', renameDirectoryName: string): Promise<boolean> {
        const success = await this.client.renameDirectory(activeDirectory, renameDirectoryName);
        return success.data?.renameDirectory || false;
    }

    private async renameFile(activeDirectory: string = '/', oldFileName: string, renameFileName: string): Promise<boolean> {
        const success = await this.client.renameFile(activeDirectory, oldFileName, renameFileName);
        return success.data?.renameFile || false;
    }

    private async deleteDirectory(activeDirectory: string = '/', deletedDirectoryName: string): Promise<boolean> {
        const success = await this.client.deleteDirectory(activeDirectory, deletedDirectoryName);
        return success.data?.deleteDirectory || false;
    }

    private async deleteFile(activeDirectory: string = '/', deleteFileName: string): Promise<boolean> {
        const success = await this.client.deleteFile(activeDirectory, deleteFileName);
        return success.data?.deleteFile || false;
    }

    private async copyDirectory(activeDirectory: string = '/', copyDirectoryName: string): Promise<Nullable<FileData>> {
        const item = await this.client.copyDirectory(activeDirectory, copyDirectoryName);
        return this.mapItemResponse(item.data?.copyDirectory);
    }

    private async copyFile(activeDirectory: string = '/', oldFileName: string, copyFileName: string): Promise<Nullable<FileData>> {
        const item = await this.client.copyFile(activeDirectory, oldFileName, copyFileName);
        return this.mapItemResponse(item.data?.copyFile);
    }

    private async createDirectory(activeDirectory: string = '/', createdDirectoryName: string): Promise<Nullable<FileData>> {
        const createdDirectory = await this.client.createDirectory(activeDirectory, createdDirectoryName);
        return this.mapItemResponse(createdDirectory.data?.createDirectory);
    }

    private async uploadFile(activeDirectory: string = '/', uploadFileName: string, fileContent: File): Promise<Nullable<FileData>> {
        const arrayBuffer = await new Response(fileContent).arrayBuffer();
        const uint8Array = new Uint8Array(arrayBuffer);
        const fileAsBase64 = btoa(String.fromCharCode(...Array.from(uint8Array)));
        const item = await this.client.uploadFile(activeDirectory, uploadFileName ?? fileContent.name, fileAsBase64);
        return this.mapItemResponse(item.data?.uploadFile);
    }

    private mapItemResponse(e: ItemResponse | null | undefined): Nullable<FileData> {
        return e
            ? {
                ...e,
                isDir: e.icon === IconTypeResponse.ManagerDirectory,
                name: e.name.substring(Math.max(e.name.lastIndexOf('\\'), e.name.lastIndexOf('/')) + 1)
            }
            : null;
    }

    private getCurrenDirectory(): string {
        return this.state.folderChain.slice(1).reduce((all, e) => `${all}/${e?.name}`, '/');
    }
    //#endregion
};

export default FileBrowser;