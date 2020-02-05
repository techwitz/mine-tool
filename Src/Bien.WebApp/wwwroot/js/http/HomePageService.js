/// <reference path="HttpFetchService.ts" /> #
/// <reference path="../../typings/jquery/jquery.d.ts" /> #
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var bien;
(function (bien) {
    var HomePageService = /** @class */ (function (_super) {
        __extends(HomePageService, _super);
        function HomePageService() {
            return _super.call(this, "Home Page") || this;
        }
        HomePageService.prototype.pullDepartments = function () {
            var self = this;
            var promise = $.Deferred();
            self.Get("/api/Department", null).then(function (result) {
                var items = [];
                if (result) {
                    result.forEach(function (d) {
                        var row = d;
                        var item = new bien.Department();
                        item.capacity(row.capacity);
                        item.name(row.name);
                        item.key(row.entityKey);
                        items.push(item);
                    });
                }
                promise.resolve(items);
                return items;
            });
            return promise;
        };
        return HomePageService;
    }(bien.HttpFetchService));
    bien.HomePageService = HomePageService;
})(bien || (bien = {}));
//# sourceMappingURL=HomePageService.js.map