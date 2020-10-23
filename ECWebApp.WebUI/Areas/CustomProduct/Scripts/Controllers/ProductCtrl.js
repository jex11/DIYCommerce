/// <reference path="../../Views/Custom/SaveDialog.html" />
app.controller('ProductCtrl', function ($scope, $rootScope, $state, $stateParams, $http, $animate, $filter, $mdDialog, GarmentService, MathService) {
    
    $scope.Accessories = null;
    $scope.SelectedAccessory = null;
    $scope.Price = {
        Template: [],
        Accessories: [],
        TotalPrice: null
    };
    $scope.SelectedTexture = "//:0";
    $scope.specification = {
        'Measurements':{
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
        'ProductName': null,
        'TemplateID': $stateParams.TemplateID,
        'ProductRetailPrice': null,
        'Color': null,
        'ProductImageBase64': null,
        'ProductImageType': 'svg+xml',
        'TextureID': null,
        'Accessories': null
    };


    var d = [{ x: 240, y: 233 }];

    $http.get('/CustomProduct/Custom/GetTemplate/' + $stateParams.TemplateID).
      success(function (data, status, headers, config) {
          // this callback will be called asynchronously
          // when the response is available
          
          var temp = JSON.stringify(data.TemplateSource);
          $scope.LineData = JSON.parse(data.TemplateSource);
          $scope.ConstantData = JSON.parse(data.TemplateSource);
          $scope.Price.Template.push({
              "Name": data.TemplateName,
              "Price": data.TemplatePrice,
              "Quantity": 1
          });
          
//          $scope.LineData = [{
//              "id": "body",
//              "data": [
//              { "x": 279.5, "y": 85 },//neck midpoint 
//              { "x": 239.5, "y": 55 },//neck left
//              { "x": 239.5, "y": 35 },//neck left shoulder start
//              { "x": 145, "y": 50 },//neck left end or shoulder end
//              { "x": 175, "y": 100 },// left armpit
//              { "x": 165, "y": 160 },
//              { "x": 175, "y": 250 },//breast left upper
//              { "x": 195, "y": 310 },//breast left mid
//              { "x": 175, "y": 450 },//breast left lower
//              { "x": 185, "y": 480 },
//              { "x": 185, "y": 525 },
//              { "x": 190, "y": 525 },
//              { "x": 279.5, "y": 545 },
//              { "x": 350, "y": 525 },
//              { "x": 355, "y": 525 },
//              { "x": 355, "y": 480 },
//              { "x": 365, "y": 450 },//breast right lower
//              { "x": 345, "y": 310 },//breast right mid
//              { "x": 365, "y": 250 },//breast right upper
//              { "x": 375, "y": 160 },
//              { "x": 365, "y": 100 },//right armpit
//              { "x": 395, "y": 50 },//neck right end Shoulder end
//              { "x": 310.5, "y": 35 },//neck right Shoulder start
//              { "x": 310.5, "y": 55 },//neck right
//              { "x": 279.5, "y": 85 }]//neck mid point
//          },


//{
//    "id": "sleeve_left", "data": [
//    { "x": 163, "y": 58 },
//    { "x": 85, "y": 310 },
//    { "x": 145, "y": 330 },
//    { "x": 170, "y": 123 }]
//},

//{
//    "id": "sleeve_right", "data": [
//    { "x": 377, "y": 58 },
//    { "x": 457, "y": 310 },
//    { "x": 400, "y": 330 },
//    { "x": 370, "y": 123 }]
//},

//{
//    "id": "kain", "data": [
//   { "x": 779.5, "y": 91 }, // Middle point top
//   { "x": 695, "y": 75 },  // Left Top Edge
//   { "x": 690, "y": 75 },  //
//   { "x": 690, "y": 105 },  //
//   { "x": 690, "y": 125 },  //
//   { "x": 681, "y": 145 },  //
//   { "x": 670, "y": 595 },  // Left Bottom Edge
//   { "x": 670, "y": 605 },  //
//   { "x": 675, "y": 605 },  //
//   { "x": 725, "y": 595 },  //
//   { "x": 795, "y": 625 },  //

//   { "x": 860, "y": 605 },  // Right Bottom Edge
//   { "x": 865, "y": 605 },  //
//   { "x": 865, "y": 595 },  //
//   { "x": 866, "y": 145 },  //
//   { "x": 845, "y": 125 },  // Right Top Edge
//   { "x": 845, "y": 75 },  //
//   { "x": 840, "y": 75 },  //
//   { "x": 779.5, "y": 91 } // Middle point top 
//    ]
//}];

//          $scope.ConstantData = [{
//              "id": "body",
//              "data": [
//              { "x": 279.5, "y": 85 },//neck midpoint 
//              { "x": 239.5, "y": 55 },//neck left
//              { "x": 239.5, "y": 35 },//neck left
//              { "x": 145, "y": 50 },//neck left end
//              { "x": 175, "y": 100 },// left armpit
//              { "x": 165, "y": 160 },
//              { "x": 175, "y": 250 },//breast left upper
//              { "x": 195, "y": 310 },//breast left mid
//              { "x": 175, "y": 450 },//breast left lower
//              { "x": 170, "y": 480 },
//              { "x": 185, "y": 525 },
//              { "x": 190, "y": 525 },
//              { "x": 279.5, "y": 545 },
//              { "x": 350, "y": 525 },
//              { "x": 355, "y": 525 },
//              { "x": 360, "y": 480 },
//              { "x": 365, "y": 450 },//breast right lower
//              { "x": 345, "y": 310 },//breast right mid
//              { "x": 365, "y": 250 },//breast right upper
//              { "x": 375, "y": 160 },
//              { "x": 365, "y": 100 },//right armpit
//              { "x": 395, "y": 50 },//neck right end
//              { "x": 310.5, "y": 35 },//neck right
//              { "x": 310.5, "y": 55 },//neck right
//              { "x": 279.5, "y": 85 }]//neck mid point
//          },


//{
//    "id": "sleeve_left", "data": [
//    { "x": 163, "y": 58 },
//    { "x": 85, "y": 310 },
//    { "x": 145, "y": 330 },
//    { "x": 170, "y": 123 }]
//},

//{
//    "id": "sleeve_right", "data": [
//    { "x": 377, "y": 58 },
//    { "x": 457, "y": 310 },
//    { "x": 400, "y": 330 },
//    { "x": 370, "y": 123 }]
//},

//{
//    "id": "kain", "data": [
//   { "x": 779.5, "y": 91 }, // Middle point top
//   { "x": 695, "y": 75 },  // Left Top Edge
//   { "x": 690, "y": 75 },  //
//   { "x": 690, "y": 105 },  //
//   { "x": 690, "y": 125 },  //
//   { "x": 681, "y": 145 },  //
//   { "x": 670, "y": 595 },  // Left Bottom Edge
//   { "x": 670, "y": 605 },  //
//   { "x": 675, "y": 605 },  //
//   { "x": 725, "y": 595 },  //wave up
//   { "x": 795, "y": 625 },  //wave down

//   { "x": 860, "y": 605 },  // Right Bottom Edge
//   { "x": 865, "y": 605 },  //
//   { "x": 865, "y": 595 },  //
//   { "x": 866, "y": 145 },  //
//   { "x": 845, "y": 125 },  // Right Top Edge
//   { "x": 845, "y": 75 },  //
//   { "x": 840, "y": 75 },  //
//   { "x": 779.5, "y": 91 } // Middle point top 
//    ]
//}];
          $scope.UpdateImage();
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

          
          
      }).
      error(function (data, status, headers, config) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          console.log(data);
      });

    //Get Textures
    $http.get('/CustomProduct/Custom/GetTextures').
        success(function (data, status, headers, config) {
            $scope.Textures = JSON.parse(data);
        });

    //Select Texture 
    $scope.TextureSelected = function (id) {
        var found = $filter('filter')($scope.Textures, { TextureId: id }, true);
        if (found.length) {
            $scope.SelectedTexture = found[0];
            $scope.specification.TextureID = $scope.SelectedTexture.TextureId;
            $scope.UpdateImage();
        } else {
            $scope.specification.Textures = null;
        }

    }


    //Get Accessoriess
    $http.get('/CustomProduct/Custom/GetAccessories').
        success(function (data, status, headers, config) {
            
            $scope.Accessories = JSON.parse(data);
        });

    //Select Accessories 
    $scope.AccessorySelected = function (id) {
        
        var found = $filter('filter')($scope.Accessories, { AccessoriesTemplateId: id }, true);

        
        if (found.length || found) {
            if ($scope.specification.Accessories == null) {
               $scope.specification.Accessories = new Array();
            }
            var existingRecord = false;
            for(var i = 0; i<$scope.Price.Accessories.length ; i++){
                var accessory = $scope.Price.Accessories[i];
                console.log(accessory);
                console.log(found[0]);
                if (accessory.Name == found[0].AccessoriesTemplateName) {
                    var unitPrice = accessory.Price / accessory.Quantity;
                    
                    accessory.Quantity++;
                    accessory.Price = unitPrice * accessory.Quantity;
                    accessory.Price = accessory.Price.toFixed(2);
                    existingRecord = true;
                    break;
                }
            }
            if (existingRecord == false) {
                console.log(found[0]);
                $scope.Price.Accessories.push({
                    "Code": found[0].AccessoriesTemplateCode,
                    "Name": found[0].AccessoriesTemplateName,
                    "Price": found[0].AccessoriesTemplatePrice,
                    "Quantity": 1
                });
            }
            


            var temp = {
                'AccessoriesID': MathService.Guid(),
                'AccessoriesTemplateID': found[0].AccessoriesTemplateId,
                'AccessoriesX': d[0].x,
                'AccessoriesY':d[0].y
            }
            $scope.specification.Accessories.push(temp);
            
            $scope.UpdateImage();
        } else {
            $scope.specification.Accessories = null;
        }

    }


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

    $scope.submit = function () {
        $rootScope.loading = "visible";
        var input = $scope.specification;
        //var temp = input.ProductRetailPrice;
        input.ProductImageBase64 = btoa(document.getElementById('product').innerHTML);
        input.Measurements = JSON.stringify(input.Measurements);
        //input.ProductRetailPrice = temp;
        $http.post('/CustomProduct/Custom/CreateProduct', input)
        .success(function (data, status, headers, config) {
          // this callback will be called asynchronously
            // when the response is available
            $rootScope.loading = "hidden";
            
            $mdDialog.show({
                templateUrl: '/CustomProduct/Custom/SaveDialog/' + data.ProductID,
                controller: 'DialogCtrl',
                locals: {
                    item: {
                        'ProductID': data.ProductID,
                        'CustomProductID': data.CustomProductID
                    }
                }
            }).then(function(){
                $state.go('Vault');
            });

        })
        .error(function (data, status, headers, config) {
              // called asynchronously if an error occurs
            // or server returns response with an error status.
            console.log(data);
          });
    }
    
    var lineFunction = d3.svg.line()
                            .x(function (d) { return d.x; })
                            .y(function (d) { return d.y; })
                            .interpolate("basis");
    var linearFunction = d3.svg.line()
                             .x(function (d) { return d.x; })
                             .y(function (d) { return d.y; })
                             .interpolate("linear");

    function onDragDrop(dragStartHandler, dragHandler, dropHandler) {
        var drag = d3.behavior.drag();
        drag.on("drag", dragHandler)
            .on("dragstart", dragStartHandler)
        .on("dragend", dropHandler);
        return drag;
    }

    d3.selection.prototype.moveToFront = function () {
        return this.each(function () {
            this.parentNode.appendChild(this);
        });
    };

    function dragstart(d) {
        var sel = d3.select(this);
        sel.moveToFront();
        if ($scope.SelectedAccessory != null) {
            if ($scope.SelectedAccessory.AccessoriesID != d.id) {
                $scope.SelectedAccessory = $filter('filter')($scope.specification.Accessories, { AccessoriesID: this.id }, true)[0];
            }
        } else {
            $scope.SelectedAccessory = $filter('filter')($scope.specification.Accessories, { AccessoriesID: this.id }, true)[0];
        }
    }

    function dragmove(d) {
        $scope.SelectedAccessory.AccessoriesX += d3.event.dx;
        $scope.SelectedAccessory.AccessoriesY += d3.event.dy;
        //d3.select(this).attr("transform", "translate(" + $scope.SelectedAccessory.AccessoriesX + "," + $scope.SelectedAccessory.AccessoriesY + ")");
        d3.select(this).attr("x", $scope.SelectedAccessory.AccessoriesX);
        d3.select(this).attr("y", $scope.SelectedAccessory.AccessoriesY);
    }

    function dragend(d) {
        $scope.SelectedAccessory = null;
    }

    


    //The SVG Container
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


    $scope.UpdateImage = function () {
        svgContainer.selectAll("path").remove();
        svgContainer.selectAll("image").remove();
        pattern.selectAll("image").remove();
        if ($scope.SelectedTexture != "//:0") {

            //208px x 450px
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
        //console.log($scope.specification.Accessories[0]);
        if ($scope.specification.Accessories) {
            for (var i = 0 ; i < $scope.specification.Accessories.length; i++) {
                
                
                var accessory = $scope.specification.Accessories[i];
                var found = $filter('filter')($scope.Accessories, { AccessoriesTemplateId: accessory.AccessoriesTemplateID }, true);
                console.log(accessory);
                d[0].x = accessory.AccessoriesX;
                d[0].y = accessory.AccessoriesY;
                var addedaccessory = svgContainer.data(d).append("image")
                                        .attr("id", accessory.AccessoriesID)
                                        .attr("xlink:href", found[0].AccessoriesThumbnailBase64)
                                        .attr("x", accessory.AccessoriesX)
                                        .attr("y", accessory.AccessoriesY)
                                        .attr("height", "100px")
                                        .attr("width", "100px")
                                        .call(onDragDrop(dragstart,dragmove, dragend));

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