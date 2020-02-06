/// <reference path="../typings/knockout/knockout.d.ts" /> #
/// <reference path="http/HomePageService.ts"/> #
var bien;
(function (bien) {
    var Index = /** @class */ (function () {
        function Index() {
            var _this = this;
            this.isModelValid = true;
            this.departments = ko.observableArray([]);
            this.ventilationCapacity = ko.observableArray([]);
            this.columnsTotal = ko.pureComputed(function () {
                var self = _this;
                var columns_total = [];
                _this.ventilationCapacity().forEach(function (row) {
                    var idx = 0;
                    row.departmentCapacity().forEach(function (column) {
                        var item = columns_total[idx] || new TotalInfo();
                        item.VentilationCapacity = column.VentilationCapacity();
                        item.total(item.total() + parseInt(column.capacity()));
                        columns_total[idx] = item;
                        idx = idx + 1;
                        if (!item.isValid() && self.isModelValid)
                            self.isModelValid = false;
                    });
                });
                console.log(columns_total);
                return columns_total;
            }, this);
        }
        Index.prototype.AddRow = function () {
            var self = this;
            var row = new VentilationCapacity();
            this.departments().forEach(function (item) {
                row.departmentCapacity.push(new CapacityInfo(item));
            });
            this.ventilationCapacity.push(row);
        };
        Index.prototype.removeRow = function (row) {
            console.info(row);
            this.ventilationCapacity.remove(row);
        };
        Index.prototype.saveRecords = function () {
            var self = this;
            var models = [];
            if (self.isModelValid) {
                this.ventilationCapacity().forEach(function (row) {
                    row.departmentCapacity().forEach(function (column) {
                        var model = { UnitName: row.unit(), EntityKey: row.key, Departmentkey: column.departmentKey(), Capacity: column.capacity() };
                        model.EntityKey = Math.random().toString(20).substring(2);
                        models.push(model);
                    });
                });
                var httpService = new bien.HomePageService();
                httpService.saveData(models).always(function (result) {
                    alert(result);
                });
            }
            else {
                alert('Please correct the error before saving!');
            }
        };
        return Index;
    }());
    bien.Index = Index;
    var TotalInfo = /** @class */ (function () {
        function TotalInfo() {
            var _this = this;
            this.totalExpr = ko.pureComputed({
                owner: this,
                read: function () {
                    var self = _this;
                    return self.total() + "/" + self.VentilationCapacity;
                }
            }, this);
            this.hasExceedLimit = ko.pureComputed({
                owner: this,
                read: function () {
                    return _this.total() > _this.VentilationCapacity ? "badge badge-danger" : "badge badge-info";
                }
            }, this);
            this.isValid = ko.pureComputed({
                owner: this,
                read: function () {
                    return _this.total() < _this.VentilationCapacity ? true : false;
                }
            }, this);
            this.VentilationCapacity = 0;
            this.total = ko.observable(0);
            this.VentilationCapacity = 0;
        }
        return TotalInfo;
    }());
    bien.TotalInfo = TotalInfo;
    var Department = /** @class */ (function () {
        function Department() {
            this.name = ko.observable("");
            this.capacity = ko.observable(0);
            this.key = ko.observable("");
        }
        return Department;
    }());
    bien.Department = Department;
    var VentilationCapacity = /** @class */ (function () {
        function VentilationCapacity() {
            this.unit = ko.observable("");
            this.key = Math.random().toString(20).substring(2);
            /*public key: KnockoutComputed<string> = ko.pureComputed(() => {
                return Math.random().toString(36).substring(16);
            }, this);*/
            this.departmentCapacity = ko.observableArray([]);
            this.key = Math.random().toString(20).substring(2);
        }
        return VentilationCapacity;
    }());
    bien.VentilationCapacity = VentilationCapacity;
    var CapacityInfo = /** @class */ (function () {
        function CapacityInfo(dept) {
            this.capacity = ko.observable(0);
            this.departmentKey = ko.observable("");
            this.VentilationCapacity = ko.observable(0);
            if (dept) {
                this.capacity(0);
                this.departmentKey(dept.key());
                this.VentilationCapacity(dept.capacity());
            }
        }
        return CapacityInfo;
    }());
    bien.CapacityInfo = CapacityInfo;
    $(document).ready(function () {
        var ele = document.getElementById("dvValitilationCapacity");
        var model = new bien.Index();
        var httpService = new bien.HomePageService();
        var result = httpService.pullDepartments().always(function (departments) {
            departments.forEach(function (item) {
                model.departments.push(item);
            });
            model.AddRow(); // add first row
            // model.departments(departments);
            ko.applyBindings(model, ele);
        });
    });
})(bien || (bien = {}));
//# sourceMappingURL=Index.js.map