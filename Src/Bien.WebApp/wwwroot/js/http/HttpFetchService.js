var bien;
(function (bien) {
    // Base class for Http operations GET, PUT, POST, DELETE, PATCH!!
    var HttpFetchService = /** @class */ (function () {
        function HttpFetchService(name) {
            this.name = name;
        }
        HttpFetchService.prototype.Post = function (url, payload) {
            var promise = $.Deferred();
            var request = $.ajax({
                url: url,
                method: "POST",
                data: JSON.stringify(payload),
                contentType: "application/json",
                dataType: "json"
            });
            request.done(function (data) {
                console.info(data);
            }).then(function (response) {
                return response.json();
            })
                .then(function (result) {
                promise.resolve(result);
            });
            //fetch(url,
            //    {
            //        method: 'post',
            //        body: JSON.stringify(payload),
            //        // data: data,
            //        //dataType: "json",
            //    }).then((response) => {
            //        return response.json();
            //    })
            //    .then(function (result: U) {
            //        promise.resolve(result);
            //    });
            return promise;
        };
        HttpFetchService.prototype.Get = function (url, payload) {
            var query = "?";
            if (payload && payload.keys) {
                for (var _i = 0, payload_1 = payload; _i < payload_1.length; _i++) {
                    var _a = payload_1[_i], key = _a[0], value = _a[1];
                    query = query + (key + " = " + value + "&");
                }
            }
            var promise = $.Deferred();
            fetch(url, {
                method: 'get',
            }).then(function (response) {
                return response.json();
            })
                .then(function (result) {
                promise.resolve(result);
            });
            return promise;
        };
        return HttpFetchService;
    }());
    bien.HttpFetchService = HttpFetchService;
})(bien || (bien = {}));
//# sourceMappingURL=HttpFetchService.js.map