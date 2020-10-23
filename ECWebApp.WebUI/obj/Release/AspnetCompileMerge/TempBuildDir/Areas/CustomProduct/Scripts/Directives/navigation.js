app.directive("ngRemove", function ($rootScope, $http) {
    return {
        link: function (scope, element, attrs) {
            
            element.bind("click", function () {
                $http.get('/CustomProduct/Vault/DeleteOwnedProducts/' + attrs.id);
                element.parent().remove();
            });
        }
    }

});

app.directive("ngProduct", function ($rootScope, $state) {
    return {
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                console.log(attrs.pid);
                $state.go("ExistingProduct", {ProductID: attrs.pid});
            });
        }
    }

});

app.directive("ngAuthor", function ($rootScope) {
    return {
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                element.parent().remove();
            });
        }
    }

});