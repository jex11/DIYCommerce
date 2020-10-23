app.controller('ExistingProductCtrl', function ($scope, $rootScope, $state, $stateParams, $http, $animate, $filter, $mdToast, GarmentService, MathService) {
    
    $scope.ProductID = $stateParams.ProductID;
    $scope.Accessories = null;
    $scope.SelectedAccessory = null;
    $scope.Price = {
        Template: [],
        Accessories: [],
        TotalPrice: null
    };
    $scope.SelectedTexture = "//:0";
    $scope.specification = {
        'Measurements': {
            'BodyLength': 38,
            'Shoulders': 16,
            'Armpit': 9,
            'SleeveOpening': 9,
            'Neck': 10,
            'Sleeve': 21,
            'Breast': 32,
            'Waist': 24,
            'Hip': 34
        },
        'ProductID': $stateParams.ProductID,
        'ProductName': null,
        'TemplateID': null,
        'ProductRetailPrice': null,
        'Color': null,
        'ProductImageBase64': null,
        'ProductImageType': 'svg+xml',
        'TextureID': null,

        'Accessories': []
    };
    $scope.toastPosition = {
        bottom: true,
        top: false,
        left: false,
        right: true
    };
    var svgContainer = d3.select("#product").append("svg")
                                        .attr("width", 1000)
                                        .attr("version", 1.1)
                                        .attr("xmlns", "http://www.w3.org/2000/svg")
                                        .attr("height", 768);
    var defs = svgContainer.append("defs");
    var pattern = defs.append("pattern")
                    .attr("id", "body_pattern")
                    .attr("patternUnits", "userSpaceOnUse")
                    .attr('x', 0)
                    .attr('y', 0)
                    .attr('width', 900)
                    .attr('height', 700);

    var lineFunction = d3.svg.line()
                        .x(function (d) { return d.x; })
                        .y(function (d) { return d.y; })
                        .interpolate("basis");
    var linearFunction = d3.svg.line()
                             .x(function (d) { return d.x; })
                             .y(function (d) { return d.y; })
                             .interpolate("linear");


    //initiate page
    //Get Product 
    $http.get('/CustomProduct/Custom/GetCustomProduct/' + $scope.ProductID).
      success(function (data, status, headers, config) {
        $scope.specification.ProductName = data.ProductName;
        $scope.specification.TemplateID = data.TemplateID;
        $scope.specification.ProductRetailPrice = data.ProductRetailPrice;
        $scope.specification.Color = data.Color;
        $scope.specification.TextureID = data.TextureID;
        GetTemplate();
        GetTexture();
        GetAccessories();
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          console.log(data);
      });

    //Get Accessoriess
    function GetAccessories() {
        $http.get('/CustomProduct/Custom/GetAccessory/' + $scope.ProductID).
        success(function (accessories, status, headers, config) {

            $scope.Accessories = JSON.parse(accessories);
            for (var i = 0; i < $scope.Accessories.length ; i++) {
                var accessory = $scope.Accessories[i];

                if ($scope.Price.Accessories.length == 0) {
                    $scope.Price.Accessories.push(
                           {
                               "Code": accessory.AccessoriesTemplateCode,
                               "Name": accessory.AccessoriesTemplateName,
                               "Price": accessory.AccessoriesTemplatePrice,
                               "Quantity": 1
                           });
                } else {
                    var ExistingAccessory = false;
                    for (var j = 0; j < $scope.Price.Accessories.length ; j++) {
                        var acc = $scope.Price.Accessories[j];

                        if (acc.Code == accessory.AccessoriesTemplateCode) {
                            var unitPrice = acc.Price / acc.Quantity;
                            acc.Quantity++;
                            acc.Price = unitPrice * acc.Quantity;
                            acc.Price = acc.Price.toFixed(2);
                            ExistingAccessory = true;
                            break;
                        }
                    }
                    if (!ExistingAccessory) {
                         $scope.Price.Accessories.push(
                           {
                               "Code": accessory.AccessoriesTemplateCode,
                               "Name": accessory.AccessoriesTemplateName,
                               "Price": accessory.AccessoriesTemplatePrice,
                               "Quantity": 1
                            });
                    }
                   
                }



                $scope.specification.Accessories.push(
                    {
                        'AccessoriesID': accessory.AccessoriesID,
                        'AccessoriesTemplateID': accessory.AccessoriesTemplateId,
                        'AccessoriesX': accessory.AccessoriesX,
                        'AccessoriesY': accessory.AccessoriesY
                    });
            }
            $scope.UpdateImage();
        });
    }
    
    //Get Template
    function GetTemplate() {
        $http.get('/CustomProduct/Custom/GetTemplate/' + $scope.specification.TemplateID)
        .success(function (data, status, headers, config) {
          $scope.LineData = JSON.parse(data.TemplateSource);
          $scope.ConstantData = JSON.parse(data.TemplateSource);
          $scope.Price.Template.push({
              "Name": data.TemplateName,
              "Price": data.TemplatePrice,
              "Quantity": 1
          });
          
          $scope.$watch('specification.Measurements.BodyLength', function () {
              $scope.LineData = GarmentService.AlterBodyLengthHeight($scope.LineData, $scope.ConstantData, $scope.specification.Measurements.BodyLength);
              $scope.UpdateImage();
          });

          $scope.$watch('specification.Measurements.Neck', function () {
              $scope.LineData = GarmentService.AlterNeckWidth($scope.LineData, $scope.ConstantData, $scope.specification.Measurements.Neck);
              $scope.UpdateImage();
          });

          $scope.$watch('specification.Measurements.Shoulders', function () {
              $scope.LineData = GarmentService.AlterShouldersWidth($scope.LineData, $scope.ConstantData, $scope.specification.Measurements.Shoulders);
              $scope.UpdateImage();
          });

          $scope.$watch('specification.Measurements.Armpit', function () {
              $scope.LineData = GarmentService.AlterArmpitWidth($scope.LineData, $scope.ConstantData, $scope.specification.Measurements.Armpit);
              $scope.UpdateImage();
          });

          $scope.$watch('specification.Measurements.SleeveOpening', function () {
              $scope.LineData = GarmentService.AlterSleeveOpeningWidth($scope.LineData, $scope.ConstantData, $scope.specification.Measurements.SleeveOpening);
              $scope.UpdateImage();
          });

          $scope.$watch('specification.Measurements.Sleeve', function () {
              $scope.LineData = GarmentService.AlterSleeveLength($scope.LineData, $scope.ConstantData, $scope.specification.Measurements.Sleeve);
              $scope.UpdateImage();
          });

          $scope.$watch('specification.Measurements.Breast', function () {
              $scope.LineData = GarmentService.AlterBreastWidth($scope.LineData, $scope.ConstantData, $scope.specification.Measurements.Breast);
              $scope.UpdateImage();
          });

          $scope.$watch('specification.Measurements.Waist', function () {
              $scope.LineData = GarmentService.AlterWaistWidth($scope.LineData, $scope.ConstantData, $scope.specification.Measurements.Waist);
              $scope.UpdateImage();
          });

          $scope.$watch('specification.Measurements.Hip', function () {
              $scope.LineData = GarmentService.AlterHipWidth($scope.LineData, $scope.ConstantData, $scope.specification.Measurements.Hip);
              $scope.UpdateImage();
          });
      })
        .error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          console.log(data);
      });
    }
    

    //Get Textures
    function GetTexture() {
        $http.get('/CustomProduct/Custom/GetTexture/' + $scope.specification.TextureID).
        success(function (data, status, headers, config) {
            $scope.SelectedTexture = JSON.parse(data);
            $scope.specification.TextureID = data.TextureId;
            $scope.UpdateImage();
            $rootScope.loading = "hidden";
        });
       
    }
    

    //Methods
    $scope.submit = function () {
        $rootScope.loading = "visible";
        var input = $scope.specification;
        input.ProductImageBase64 = btoa(document.getElementById('product').innerHTML);
        input.Measurements = JSON.stringify(input.Measurements);
        $http.post('/CustomProduct/Custom/BuyProduct', input)
        .success(function (data, status, headers, config) {
            // this callback will be called asynchronously
            // when the response is available
            $rootScope.loading = "hidden";
            $mdToast.show($mdToast.simple().content('Added to Cart. Continue shopping ?'));
            $state.go('Gallery');
            

        })
        .error(function (data, status, headers, config) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
            console.log(data);
        });
    }

    $scope.getToastPosition = function () {
        return Object.keys($scope.toastPosition)
          .filter(function (pos) { return $scope.toastPosition[pos]; })
          .join(' ');
    };

    //The SVG Container
    $scope.UpdateImage = function () {
        svgContainer.selectAll("path").remove();
        svgContainer.selectAll("image").remove();
        pattern.selectAll("image").remove();
        if ($scope.SelectedTexture != "//:0") {

            //900 x 700 px
            var texture = pattern.append("image")
                              .attr("id", "body_texture")
                              .attr("xlink:href", $scope.SelectedTexture.TextureThumbnailBase64)
                              .attr('x', 0)
                              .attr('y', 0)
                              .attr('width', 900)
                              .attr('height', 700);

            var body = svgContainer.append("path")
                                    .attr("id", $scope.LineData[0].id)
                                    .attr("d", lineFunction($scope.LineData[0].data))
                                    .attr("stroke", "#2a2a2a")
                                    .attr("stroke-width", 1)
                                    .attr("fill", 'url(#body_pattern)');


            var Sleeve_left = svgContainer.append("path")
                                    .attr("id", $scope.LineData[1].id)
                                    .attr("d", linearFunction($scope.LineData[1].data))
                                    .attr("stroke", "#2a2a2a")
                                    .attr("stroke-width", 1)
                                    .attr("fill", 'url(#body_pattern)');
            var Sleeve_right = svgContainer.append("path")
                                        .attr("id", $scope.LineData[2].id)
                                        .attr("d", linearFunction($scope.LineData[2].data))
                                        .attr("stroke", "#2a2a2a")
                                        .attr("stroke-width", 1)
                                        .attr("fill", 'url(#body_pattern)');
            var kain = svgContainer.append("path")
                                       .attr("id", $scope.LineData[3].id)
                                       .attr("d", lineFunction($scope.LineData[3].data))
                                       .attr("stroke", "#2a2a2a")
                                       .attr("stroke-width", 1)
                                       .attr("fill", $scope.specification.Color);
        } else {
            var body = svgContainer.append("path")
                                    .attr("id", $scope.LineData[0].id)
                                    .attr("d", lineFunction($scope.LineData[0].data))
                                    .attr("stroke", "#2a2a2a")
                                    .attr("stroke-width", 1)
                                    .attr("fill", "blue");

            var Sleeve_left = svgContainer.append("path")
                                    .attr("id", $scope.LineData[1].id)
                                    .attr("d", linearFunction($scope.LineData[1].data))
                                    .attr("stroke", "#2a2a2a")
                                    .attr("stroke-width", 1)
                                    .attr("fill", "blue");
            var Sleeve_right = svgContainer.append("path")
                                        .attr("id", $scope.LineData[2].id)
                                        .attr("d", linearFunction($scope.LineData[2].data))
                                        .attr("stroke", "#2a2a2a")
                                        .attr("stroke-width", 1)
                                        .attr("fill", "blue");
            var kain = svgContainer.append("path")
                                        .attr("id", $scope.LineData[3].id)
                                        .attr("d", lineFunction($scope.LineData[3].data))
                                        .attr("stroke", "#2a2a2a")
                                        .attr("stroke-width", 1)
                                        .attr("fill", $scope.specification.Color);
        }
        $scope.RenderAccessories();
        $rootScope.loading = "hidden";
    };

    $scope.RenderAccessories = function () {
        if ($scope.specification.Accessories) {
            for (var i = 0 ; i < $scope.specification.Accessories.length; i++) {


                var accessory = $scope.specification.Accessories[i];
                var found = $filter('filter')($scope.Accessories, { AccessoriesTemplateId: accessory.AccessoriesTemplateID }, true);
                var addedaccessory = svgContainer.append("image")
                                        .attr("id", accessory.AccessoriesID)
                                        .attr("xlink:href", found[0].AccessoriesThumbnailBase64)
                                        .attr("x", accessory.AccessoriesX)
                                        .attr("y", accessory.AccessoriesY)
                                        .attr("height", "100px")
                                        .attr("width", "100px");

            }
        };




    }

    //Price Calculator

    $scope.PriceSubtotal = function () {
        var subtotal = 0;
        for (var i = 0 ; i < $scope.Price.Template.length; i++) {
            var item = $scope.Price.Template[i];
            subtotal += parseFloat(item.Price);
        }

        for (var i = 0 ; i < $scope.Price.Accessories.length; i++) {
            var item = $scope.Price.Accessories[i];

            subtotal += parseFloat(item.Price);

        }
        $scope.specification.ProductRetailPrice = subtotal;
        return subtotal.toFixed(2);
    }

    $scope.showSaveDialog = function () {
        $mdDialog.show({
            templateUrl: 'Areas/CustomProduct/Views/Custom/SaveDialog.html',
            controller: 'DialogCtrl',
            locals: {
                item: $scope.specification
            }
        });
    };



});