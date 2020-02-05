/// <reference path="../typings/knockout/knockout.d.ts" /> #
/// <reference path="http/HomePageService.ts"/> #
var bien;
(function (bien) {
    var Index = /** @class */ (function () {
        function Index() {
            this.departments = ko.observableArray([]);
            this.ventilationCapacity = ko.observableArray([]);
        }
        Index.prototype.AddRow = function () {
            var row = new VentilationCapacity();
            this.ventilationCapacity.push(row);
        };
        return Index;
    }());
    bien.Index = Index;
    var Department = /** @class */ (function () {
        function Department() {
            this.name = ko.observable("");
            this.capacity = ko.observable(0);
            this.key = ko.observable("");
            /*public key: KnockoutComputed<string> = ko.pureComputed(() => {
                return Math.random().toString(36).substring(16);
            }, this);*/
        }
        return Department;
    }());
    bien.Department = Department;
    var VentilationCapacity = /** @class */ (function () {
        function VentilationCapacity() {
            this.unit = ko.observable("");
            this.key = Math.random().toString(36).substring(16);
            /*public key: KnockoutComputed<string> = ko.pureComputed(() => {
                return Math.random().toString(36).substring(16);
            }, this);*/
            this.departmentCapacity = ko.observableArray([]);
            this.key = Math.random().toString(36).substring(16);
        }
        return VentilationCapacity;
    }());
    bien.VentilationCapacity = VentilationCapacity;
    var CapacityInfo = /** @class */ (function () {
        function CapacityInfo() {
            this.capacity = ko.observable(0);
            this.departmentKey = ko.observable("");
            this.VentilationCapacity = ko.observable(0);
        }
        return CapacityInfo;
    }());
    bien.CapacityInfo = CapacityInfo;
    $(document).ready(function () {
        var ele = document.getElementById("dvValitilationCapacity");
        var model = new bien.Index();
        var httpService = new bien.HomePageService();
        httpService.pullDepartments().then(function (departments) {
            model.departments(departments);
            ko.applyBindings(model, ele);
        });
    });
})(bien || (bien = {}));
//# sourceMappingURL=Index.js.map