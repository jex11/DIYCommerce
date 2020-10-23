app.controller('GalleryCtrl', function ($scope, $rootScope, $http) {
    $rootScope.loading = "hidden";

    //initiate page
    //$http.get('CustomProduct/Gallery/MostRecent')
    //    .success(function (data, status, headers, config) {
    //        console.log(data);
    //        $scope.MostRecent = data;
    //    });
    //$http.get('CustomProduct/Gallery/MostPopular')
    //    .success(function (data, status, headers, config) {
    //        console.log(data);
    //        $scope.MostPopular = data;
    //    });

    //$http.get('CustomProduct/Gallery/PopularAuthor')
    //   .success(function (data, status, headers, config) {
    //       console.log(data);
    //       $scope.PopularAuthor = data;
    //   });
});