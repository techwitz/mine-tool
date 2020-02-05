/// <reference path="../typings/knockout/knockout.d.ts" /> #
/// <reference path="http/HomePageService.ts"/> #

module bien {
    export class Index {
        public departments: KnockoutObservableArray<Department> = ko.observableArray([]);
        public ventilationCapacity: KnockoutObservableArray<VentilationCapacity> = ko.observableArray([]);

        public AddRow() {
            var row = new VentilationCapacity();
            this.ventilationCapacity.push(row);
        }
    }

    export class Department {
        public name: KnockoutObservable<string> = ko.observable("");
        public capacity: KnockoutObservable<number> = ko.observable(0);
        public key: KnockoutObservable<string> = ko.observable("");
        /*public key: KnockoutComputed<string> = ko.pureComputed(() => {
            return Math.random().toString(36).substring(16);
        }, this);*/
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
        public capacity: KnockoutObservable<number> = ko.observable(0);
        public departmentKey: KnockoutObservable<string> = ko.observable("");
        public VentilationCapacity: KnockoutObservable<number> = ko.observable(0);
    }

    $(document).ready(() => {
        var ele = document.getElementById("dvValitilationCapacity");
        var model = new bien.Index();

        var httpService = new HomePageService();
        httpService.pullDepartments().then(departments => {
            model.departments(departments);
            ko.applyBindings(model, ele);
        });
    });
}