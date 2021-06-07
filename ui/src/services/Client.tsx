import { ApolloClient, ApolloQueryResult, FetchResult, HttpLink, InMemoryCache, gql /*, from*/ } from '@apollo/client';
import { onError } from "@apollo/client/link/error";
import { createUploadLink } from 'apollo-upload-client';

import settings from '../config/Settings';
import {
    CopyDirectoryResponse,
    CopyFileResponse,
    CreateDirectoryResponse,
    DeleteDirectoryResponse,
    DeleteFileResponse,
    ListItemsResponse,
    RenameDirectoryResponse,
    RenameFileResponse,
    UploadFileResponse
} from '../models/response/ListItemsResponse';
import ItemsQueries from './queries/Items'

class GraphQLClient {
    readonly httpLink = new HttpLink({
        uri: settings.apiUrl
    });

    readonly errorLink = onError(({ graphQLErrors, networkError }) => {
        if (graphQLErrors) {
            let errorMessage: string = '';
            graphQLErrors.forEach(({ message, locations, path }) =>
                errorMessage += `[GraphQL error]: Message: ${message}, Location: ${locations}, Path: ${path}\r\n`
            );
            alert(errorMessage);
        }

        if (networkError) alert(`[Network error]: ${networkError}`);
    });

    readonly cache = new InMemoryCache();
    readonly client = new ApolloClient({
        link: createUploadLink({
           uri: settings.apiUrl
        }),
        cache: this.cache
    });

    listItems(path: string = '/'): Promise<ApolloQueryResult<ListItemsResponse>> {
        return this.client.query({
            query: gql(ItemsQueries.listItems),
            variables: { path }
        });
    }

    uploadFile(activeDirectory: string, uploadFileName: string, file: string): Promise<FetchResult<UploadFileResponse>> {
        this.client.resetStore();

        return this.client.mutate({
            mutation: gql(ItemsQueries.uploadFile),
            variables: { activeDirectory, uploadFileName, file },
        });
    }

    renameDirectory(activeDirectory: string, renameDirectoryName: string): Promise<FetchResult<RenameDirectoryResponse>> {
        this.client.resetStore();
        return this.client.mutate({
            mutation: gql(ItemsQueries.renameDirectory),
            variables: { activeDirectory, renameDirectoryName }
        });
    }

    renameFile(activeDirectory: string, oldFileName: string, renameFileName: string): Promise<FetchResult<RenameFileResponse>> {
        this.client.resetStore();
        return this.client.mutate({
            mutation: gql(ItemsQueries.renameFile),
            variables: { activeDirectory, oldFileName, renameFileName }
        });
    }

    deleteDirectory(activeDirectory: string, deletedDirectoryName: string): Promise<FetchResult<DeleteDirectoryResponse>> {
        this.client.resetStore();
        return this.client.mutate({
            mutation: gql(ItemsQueries.deleteDirectory),
            variables: { activeDirectory, deletedDirectoryName }
        });
    }

    deleteFile(activeDirectory: string, deleteFileName: string): Promise<FetchResult<DeleteFileResponse>> {
        this.client.resetStore();
        return this.client.mutate({
            mutation: gql(ItemsQueries.deleteFile),
            variables: { activeDirectory, deleteFileName }
        });
    }

    copyDirectory(activeDirectory: string, copyDirectoryName: string): Promise<FetchResult<CopyDirectoryResponse>> {
        this.client.resetStore();
        return this.client.mutate({
            mutation: gql(ItemsQueries.copyDirectory),
            variables: { activeDirectory, copyDirectoryName }
        });
    }

    copyFile(activeDirectory: string, oldFileName: string, copyFileName: string): Promise<FetchResult<CopyFileResponse>> {
        this.client.resetStore();
        return this.client.mutate({
            mutation: gql(ItemsQueries.copyFile),
            variables: { activeDirectory, oldFileName, copyFileName }
        });
    }

    createDirectory(activeDirectory: string, createdDirectoryName: string): Promise<FetchResult<CreateDirectoryResponse>> {
        this.client.resetStore();
        return this.client.mutate({
            mutation: gql(ItemsQueries.createDirectory),
            variables: { activeDirectory, createdDirectoryName }
        });
    }
}

export default GraphQLClient;