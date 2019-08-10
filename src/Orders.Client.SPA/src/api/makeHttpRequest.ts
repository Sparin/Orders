
export const defaultPage = 0;
export const defaultLimit = 50;

export default function makeHttpRequest<T>(
    url: string,
    method: string,
    body: any = undefined,
    apiRoot: string = 'https://localhost:5001'): Promise<T> {
    url = apiRoot + url;
    const headers = new Headers();
    headers.append("Accept", "application/json");
    headers.append("Content-Type", "application/json");

    const options = {
        method: method,
        headers: headers,
        body: JSON.stringify(body)
    }

    return fetch(url, options)
        .then(response => {
            if (!response.ok) {
                switch (response.status) {
                    case 400: return response.json();
                    default: Promise.reject(response.statusText)
                }
            }
            return response.json();
        })

}