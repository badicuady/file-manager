export enum IconTypeResponse {
    ManagerDirectory = "MANAGER_DIRECTORY",
    ManagerFile = "MANAGER_FILE"
}

export enum FileAttributeTypeResponse {
    ReadOnly = "READ_ONLY",
    Hidden = "HIDDEN",
    System = "SYSTEM",
    Directory = "DIRECTORY",
    Archive = "ARCHIVE",
    Device = "DEVICE",
    Normal = "NORMAL",
    Temporary = "TEMPORARY",
    SparseFile = "SPARSE_FILE",
    ReparsePoint = "REPARSE_POINT",
    Compressed = "COMPRESSED",
    Offline = "OFFLINE",
    NotContentIndexed = "NOT_CONTENT_INDEXED",
    Encrypted = "ENCRYPTED",
    IntegrityStream = "INTEGRITY_STREAM",
    NoScrubData = "NO_SCRUB_DATA"    
}

export interface ItemMetadataResponse {
    attributes: FileAttributeTypeResponse[],
    creationTime: Date,
    extension: string,
    fullName: string,
    lastAccessTime: Date,
    name: string
}

export interface ItemResponse {
    icon: IconTypeResponse,
    id: string,
    name: string,
    size: number,
    metadata: ItemMetadataResponse
}

export interface ListItemsResponse {
    items: ItemResponse[]
}

export interface RenameDirectoryResponse {
    renameDirectory: boolean
}

export interface RenameFileResponse {
    renameFile: boolean
}

export interface DeleteDirectoryResponse {
    deleteDirectory: boolean
}

export interface DeleteFileResponse {
    deleteFile: boolean
}

export interface CopyDirectoryResponse {
    copyDirectory: ItemResponse
}

export interface CopyFileResponse {
    copyFile: ItemResponse
}

export interface CreateDirectoryResponse {
    createDirectory: ItemResponse
}