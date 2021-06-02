import { Nullable } from 'tsdef';
import { ChonkyActions, FullFileBrowser, FileArray, FileData, MapFileActionsToData, ChonkyActionUnion } from 'chonky';
import React from 'react';
import { IconTypeResponse, ItemResponse } from '../models/response/ListItemsResponse';
import GraphQLClient from '../services/Client';

interface IProps { }
interface IState { files: FileArray, folderChain: FileArray }

class FileBrowser extends React.Component<IProps, IState> {
    readonly baseDirectory: string = 'root';

    readonly fileActions = [
        ChonkyActions.UploadFiles,
        ChonkyActions.DownloadFiles,
        ChonkyActions.DeleteFiles,
    ];

    readonly client: GraphQLClient;

    currentDirectory: string = '/';

    constructor(args: any[]) {
        super(args);
        this.client = new GraphQLClient();
        this.state = {
            files: [],
            folderChain: [{ id: this.baseDirectory, name: this.baseDirectory, isDir: true }]
        };
    }

    async componentDidMount() {
        await this.loadCurrentDirectory();
    }

    render(): JSX.Element {
        return (
            <div style={{ height: 500 }}>
                <FullFileBrowser
                    files={this.state.files}
                    folderChain={this.state.folderChain}
                    fileActions={this.fileActions}
                    onFileAction={async (evt: MapFileActionsToData<ChonkyActionUnion>): Promise<void> => await this.handleFileAction(evt)}
                />
            </div>
        );
    }

    //#region Handlers

    private async handleFileAction(evt: MapFileActionsToData<ChonkyActionUnion>): Promise<void> {
        console.log(evt);
        if (evt.id === ChonkyActions.OpenFiles.id) {
            const selectedItem: FileData = evt.payload.targetFile ?? evt.payload.files[0];
            if (!selectedItem.isDir) { return; }

            const index = this.state.folderChain.findIndex(e => e?.id === selectedItem.id);
            if (index < 0) {
                this.currentDirectory = `${this.currentDirectory}${this.currentDirectory === '/' ? '' : '/'}${selectedItem.name}`;
                this.addFolderToFolderChain(selectedItem);
            } else {
                this.removeFolderToFolderChain(index);
                this.currentDirectory = this.state.folderChain
                    .reduce((acc: string, current: Nullable<FileData>) => `${acc}/${current?.name}`, '')
                    .replace(`/${this.baseDirectory}`, '/');
            }

            await this.loadCurrentDirectory(this.currentDirectory);
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

    private removeFolderToFolderChain(index: number): void {
        const iterations: number = this.state.folderChain.length - index - 1;
        let ndx: number = 0;
        let folderChain: FileArray = [...this.state.folderChain];
        while (ndx < iterations) {
            folderChain.pop();
            ndx += 1;
        }
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

    private mapItemResponse(e: ItemResponse): Nullable<FileData> {
        return {
            ...e,
            isDir: e.icon === IconTypeResponse.ManagerDirectory,
            name: e.name.substring(e.name.lastIndexOf('\\') + 1)
        }
    }

    //#endregion
};

export default FileBrowser;