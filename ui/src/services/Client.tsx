import { ApolloClient, ApolloQueryResult, InMemoryCache } from '@apollo/client';
import { gql } from '@apollo/client';
import settings from '../config/Settings';
import { ListItemsResponse } from '../models/response/ListItemsResponse';
import ListItemsQueries from './queries/Items'

class GraphQLClient {
    readonly client = new ApolloClient({
        uri: settings.apiUrl,
        cache: new InMemoryCache()
    });

    listItems(path: string = '/'): Promise<ApolloQueryResult<ListItemsResponse>> {
        return this.client.query({
            query: gql(ListItemsQueries.listItems),
            variables: { path: path }
        });
    }
}

export default GraphQLClient;