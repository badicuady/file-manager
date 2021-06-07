type ItemsQueriesType = {
    listItems: string;
    uploadFile: string;
    renameDirectory: string;
    renameFile: string;
    deleteDirectory: string;
    deleteFile: string;
    copyDirectory: string;
    copyFile: string;
    createDirectory: string;
}

const Item: string = 
    `{
        id,
        name,
        icon,
        size,
        metadata {
            name,
            creationTime,
            lastAccessTime,
            extension,
            fullName,
            attributes
        }
    }`;

const ItemsQueries: ItemsQueriesType = {
    listItems: `query Items ($path: String) {
        items(path: $path) ${Item}
    }`,

    uploadFile: `mutation uploadFile($activeDirectory: String, $uploadFileName: String!, $file: String!) {
        uploadFile(activeDirectory:$activeDirectory, uploadFileName:$uploadFileName, file:$file) ${Item}
    }`,

    renameDirectory: `mutation renameDirectory($activeDirectory: String, $renameDirectoryName: String!) {
        renameDirectory(activeDirectory:$activeDirectory, renameDirectoryName:$renameDirectoryName)
    }`,

    renameFile: `mutation renameFile($activeDirectory: String, $oldFileName:String!, $renameFileName: String!) {
        renameFile(activeDirectory:$activeDirectory, oldFileName:$oldFileName, renameFileName:$renameFileName)
    }`,

    deleteDirectory: `mutation deleteDirectory($activeDirectory: String, $deletedDirectoryName: String!) {
        deleteDirectory(activeDirectory:$activeDirectory, deletedDirectoryName:$deletedDirectoryName)
    }`,

    deleteFile: `mutation deleteFile($activeDirectory: String, $deleteFileName: String!) {
        deleteFile(activeDirectory:$activeDirectory, deleteFileName:$deleteFileName)
    }`,

    copyDirectory: `mutation deleteDirectory($activeDirectory: String, $copyDirectoryName: String!) {
        copyDirectory(activeDirectory:$activeDirectory, copyDirectoryName:$copyDirectoryName) ${Item}
    }`,

    copyFile: `mutation deleteFile($activeDirectory: String, $oldFileName:String!, $copyFileName: String!) {
        copyFile(activeDirectory:$activeDirectory, oldFileName:$oldFileName, copyFileName:$copyFileName) ${Item}
    }`,

    createDirectory: `mutation createDirectory($activeDirectory: String, $createdDirectoryName: String!) {
        createDirectory(activeDirectory: $activeDirectory, createdDirectoryName: $createdDirectoryName) ${Item}
    }`
};

export default ItemsQueries;