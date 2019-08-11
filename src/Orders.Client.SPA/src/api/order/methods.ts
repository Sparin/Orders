import { SearchOptions, Order, SearchResponse } from './models';
import makeHttpRequest, { defaultPage, defaultLimit } from '../makeHttpRequest';
import queryString from 'query-string';

export function searchOrders(
    page: number = defaultPage,
    limit: number = defaultLimit,
    options: SearchOptions
) {
    const params = { ...options, page, limit };
    const url = `/api/order?` + queryString.stringify(params);

    return makeHttpRequest<SearchResponse<Order>>(url, 'GET');
}

export function getOrder(id: number): Promise<Order> {
    const url = `/api/order/${id}`;

    return makeHttpRequest<Order>(url, 'GET');
}

export function createOrder(order: Order): Promise<Order> {
    const body = order
    const url = `/api/order`;

    return makeHttpRequest<Order>(url, 'POST', body);
}

export function updateOrder(id: number, order: Order): Promise<Order> {
    const body = order
    const url = `/api/order/${id}`;

    return makeHttpRequest<Order>(url, 'PUT', body);
}

export function deleteOrder(id: number) {
    const url = `/api/order/${id}`;

    return makeHttpRequest<any>(url, 'DELETE');
}