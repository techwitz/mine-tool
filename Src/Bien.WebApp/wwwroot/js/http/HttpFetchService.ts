module bien {
    // Base class for Http operations GET, PUT, POST, DELETE, PATCH!!
    export class HttpFetchService<T> {
        name: string;
        constructor(name: string) {
            this.name = name;
        }

        public Get<U>(url: string, payload: any): any {
            var query = "?";
            if (payload && payload.keys) {
                for (let [key, value] of payload) {
                    query = query + `${key} = ${value}&`;
                }
            }

            var promise = $.Deferred<U[]>();
            fetch(url,
                {
                    method: 'get',
                }).then((response) => {
                    return response.json();
                })
                .then(function (result: U[]) {
                    promise.resolve(result);
                });
            return promise;
        }

        /*
         * Response provides multiple promise-based methods to access the body in various formats:
        response.text() – read the response and return as text,
        response.json() – parse the response as JSON,
        response.formData() – return the response as FormData object (explained in the next chapter),
        response.blob() – return the response as Blob (binary data with type),
        response.arrayBuffer() – return the response as ArrayBuffer (low-level representaion of binary data),
        additionally, response.body is a ReadableStream object, it allows to read the body chunk-by-chunk, we’ll see an example later.
         */
    }
}