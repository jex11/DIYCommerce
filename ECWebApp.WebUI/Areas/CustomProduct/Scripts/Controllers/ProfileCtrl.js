app.controller('ProfileCtrl', function ($scope, $rootScope, $http, $filter, ngTableParams) {
    $rootScope.loading = "hidden";
    $scope.data = [];

    $http.get('/CustomProduct/Profile/SalesHistory')
    .success(function (data, status, headers, config) {
        // this callback will be called asynchronously
        // when the response is available
        data = JSON.parse(data);
        $scope.data.push(data[0]);
        
        $scope.tableParams = new ngTableParams({
            page: 1,            // show first page
            count: 10,           // count per page
            sorting: {
                name: 'asc'     // initial sorting
            }
        }, {
            total: data.length, // length of data
            getData: function ($defer, params) {
                var orderedData = params.sorting() ?
                                $filter('orderBy')(data, params.orderBy()) :
                                data;

                $defer.resolve(data.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            }
        });
    });

    $http.get('/CustomProduct/Profile/SalesAnalysis')
    .success(function (data, status, headers, config) {
        // this callback will be called asynchronously
        // when the response is available
            $scope.chartData = [];
            data = JSON.parse(data);
            $scope.chartData.push(data.datas);
            $scope.chartLabels = data.labels;
            console.log($scope.chartData);

            console.log($scope.chartLabels);
            $scope.onClick = function (points, evt) {
                console.log(points, evt);
            };
        });
});

