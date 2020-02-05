﻿/// <reference path="../typings/knockout/knockout.d.ts" /> #
/// <reference path="http/HomePageService.ts"/> #

interface ObjectConstructor {
    assign(target?: any, source?: any);
}

module bien {
    export class Index {
        constructor() {
        }

        public departments: KnockoutObservableArray<Department> = ko.observableArray([]);
        public ventilationCapacity: KnockoutObservableArray<VentilationCapacity> = ko.observableArray([]);
        public columnsTotal: KnockoutComputed<any[]> = ko.pureComputed(() => {
            var columns_total: TotalInfo[] = []
            this.ventilationCapacity().forEach(row => {
                var idx = 0;
                row.departmentCapacity().forEach(column => {
                    var item = columns_total[idx] || new TotalInfo();
                    item.VentilationCapacity = column.VentilationCapacity();
                    item.total(item.total() + parseInt(<any>column.capacity()));
                    columns_total[idx] = item;
                    idx = idx + 1;
                });
            });
            console.log(columns_total);
            return columns_total;
        }, this);

        public AddRow() {
            var self = this;
            var row = new VentilationCapacity();
            this.departments().forEach(item => {
                row.departmentCapacity.push(new CapacityInfo(item));
            });
            this.ventilationCapacity.push(row);
        }

        public removeRow(row) {
            console.info(row);
            this.ventilationCapacity.remove(row);
        }
    }

    export class TotalInfo {
        constructor() {
            this.VentilationCapacity = 0;
        }

        public totalExpr: KnockoutComputed<String> = ko.pureComputed({
            owner: this,
            read: () => {
                var self = this;
                return self.total() + "/" + self.VentilationCapacity;
            }
        }, this);

        public hasExceedLimit: KnockoutComputed<String> = ko.pureComputed({
            owner: this,
            read: () => {
                return this.total() > this.VentilationCapacity ? "badge badge-danger" : "badge badge-info";
            }
        }, this);

        public VentilationCapacity: number = 0;
        public total: KnockoutObservable<number> = ko.observable(0);
    }

    export class Department {
        public name: KnockoutObservable<string> = ko.observable("");
        public capacity: KnockoutObservable<number> = ko.observable(0);
        public key: KnockoutObservable<string> = ko.observable("");
    }

    export class VentilationCapacity {
        constructor() {
            this.key = Math.random().toString(36).substring(16);
        }

        public unit: KnockoutObservable<string> = ko.observable("");
        public key: string = Math.random().toString(36).substring(16);
        /*public key: KnockoutComputed<string> = ko.pureComputed(() => {
            return Math.random().toString(36).substring(16);
        }, this);*/
        public departmentCapacity: KnockoutObservableArray<CapacityInfo> = ko.observableArray([]);
    }

    export class CapacityInfo {
        constructor(dept?: Department) {
            if (dept) {
                this.capacity(0);
                this.departmentKey(dept.key());
                this.VentilationCapacity(dept.capacity());
            }
        }
        public capacity: KnockoutObservable<number> = ko.observable(0);
        public departmentKey: KnockoutObservable<string> = ko.observable("");
        public VentilationCapacity: KnockoutObservable<number> = ko.observable(0);
    }

    $(document).ready(() => {
        var ele = document.getElementById("dvValitilationCapacity");
        var model = new bien.Index();

        var httpService = new HomePageService();
        var result = httpService.pullDepartments().always(departments => {
            departments.forEach(item => {
                model.departments.push(item);
            });
            model.AddRow(); // add first row
            // model.departments(departments);
            ko.applyBindings(model, ele);
        });
    });
}