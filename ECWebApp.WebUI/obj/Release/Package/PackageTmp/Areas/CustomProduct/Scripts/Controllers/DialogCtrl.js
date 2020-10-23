app.controller('DialogCtrl', function ($scope, $rootScope, item, $mdDialog, $mdToast, $http) {
    console.log(item);
    $rootScope.loading = "hidden";
    //$rootScope.$apply();
    $scope.toastPosition = {
        bottom: true,
        top: false,
        left: false,
        right: true
    };

    $scope.getToastPosition = function () {
        return Object.keys($scope.toastPosition)
          .filter(function (pos) { return $scope.toastPosition[pos]; })
          .join(' ');
    };


    $scope.AddToCart = function () {
        var url = '/CustomProduct/Custom/AddToCart';
        console.log(url);
        $http.post(url, item)
        .success(function (data, status, headers, config) {
            // this callback will be called asynchronously
            // when the response is available
            console.log(data);
            var toast = $mdToast.simple()
                  .content('Added to cart. ')
                  .action('OK')
                  .highlightAction(false)
                  .position($scope.getToastPosition());
            $mdToast.show(toast).then(function () {
                
            });

        })
        .error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
            console.log(data);
        })
        .then(function () {
            $mdDialog.hide();
        });
    }

    $scope.Publish = function () {
        $mdDialog.hide();
    }
});