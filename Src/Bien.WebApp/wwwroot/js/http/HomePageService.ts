/// <reference path="HttpFetchService.ts" /> #
/// <reference path="../../typings/jquery/jquery.d.ts" /> #
/// <reference path="../Index.ts" /> #

module bien {
    export class HomePageService extends HttpFetchService<Index> {
        constructor() {
            super("Home Page");
        }

        public saveData(data) {
            var self = this;

            var promise = $.Deferred<any>();
            self.Post<any>("/api/Department", data).then(result => {
                promise.resolve(result);
                return result;
            });

            return promise;
        }

        public pullDepartments() {
            var self = this;

            var promise = $.Deferred<Department[]>();
            self.Get<[]>("/api/Department", null).then(result => {
                var items = [];
                if (result) {
                    result.forEach(d => {
                        var row = <any>d;
                        var item = new Department();
                        item.capacity(row.capacity);
                        item.name(row.name);
                        item.key(row.entityKey);
                        items.push(item);
                    });
                }

                promise.resolve(items);
                return items;
            });

            return <any>promise;
        }
    }
}