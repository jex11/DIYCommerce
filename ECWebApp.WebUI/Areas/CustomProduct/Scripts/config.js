var app = angular.module('DIYCommerce', ['ui.router','ngMdIcons', 'chart.js', 'ngTable', 'ngMaterial', 'angAccordion', 'ngDraggable', 'ngMessages', 'colorpicker.module']);

app.config(function ($stateProvider, $urlRouterProvider) {

    //
    // Now set up the states
    $stateProvider
      .state('Profile', {
          url: "/profile",
          templateUrl: "CustomProduct/Profile/Index",
          controller: 'ProfileCtrl'
      })
    
    $stateProvider
     .state('Gallery', {
         url: "/gallery",
         templateUrl: "CustomProduct/Gallery/Index",
         controller: 'GalleryCtrl'
     })

    $stateProvider
     .state('Category', {
         url: "/category",
         templateUrl: "CustomProduct/Category/Index",
         controller: 'CategoryCtrl'
     })

    $stateProvider
     .state('Vault', {
         url: "/vault",
         templateUrl: "CustomProduct/Vault/Index",
         controller: 'VaultCtrl'
     })

    $stateProvider
    .state('Favourite', {
        url: "/favourite",
        templateUrl: "CustomProduct/Favourite/Index",
        controller: 'FavouriteCtrl'
    })

    $stateProvider
    .state('Search', {
        url: "/search",
        templateUrl: "CustomProduct/Search/Index",
        controller: 'SearchCtrl'
    })


    $stateProvider
     .state('Template', {
         url: "/create/ChooseTemplate",
         templateUrl: "CustomProduct/Custom/Template",
         controller: 'TemplateCtrl'
     })

    $stateProvider
     .state('Product', {
         url: "/create/Product/:TemplateID",
         templateUrl: "CustomProduct/Custom/Product",
         controller: 'ProductCtrl'
     })

    $stateProvider
     .state('ExistingProduct', {
         url: "/open/Product/:ProductID",
         templateUrl: "CustomProduct/Custom/ExistingProduct",
         controller: 'ExistingProductCtrl'
     })

    ////
    //// For any unmatched url, redirect to /state1
    $urlRouterProvider.otherwise("/gallery");
    ////


});