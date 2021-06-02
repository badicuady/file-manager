export type ItemsQueriesType = {
    listItems: string;
}

const ListItemsQueries: ItemsQueriesType = {
    listItems: `query Items ($path: String) {
        items(path: $path) {
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
        }
    }`
};

export default ListItemsQueries;