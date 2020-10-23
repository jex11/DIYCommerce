app.controller('TemplateCtrl', function ($scope, $rootScope, $state) {
    $rootScope.loading = "hidden";

    $scope.getTemplate = function (id) {
        $state.go("Product", {TemplateID: id});
    }
});