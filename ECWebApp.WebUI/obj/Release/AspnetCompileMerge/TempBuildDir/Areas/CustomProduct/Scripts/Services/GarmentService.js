app.service('GarmentService', function (MathService) {
    var maxLength = null;
    
    this.AlterNeckWidth = function (LineData, ConstantData, value) {
        maxLength = ConstantData[0].data.length - 1;
        var factor = ConstantData[0].data[1].x / 10;
        var diffVal = (ConstantData[0].data[1].x - (value * factor)) / 2;


        LineData[0].data[1].x = ConstantData[0].data[1].x + diffVal;
        LineData[0].data[maxLength - 1].x = ConstantData[0].data[maxLength - 1].x - diffVal;

        return LineData;
    }

    this.AlterBodyLengthHeight = function (LineData, ConstantData, value) {
        maxLength = ConstantData[0].data.length - 1;

        var factor = ConstantData[0].data[maxLength].y / 38;
        var diffVal = -(ConstantData[0].data[maxLength].y - (value * factor));

        for (var i = 4 ; i < maxLength - 4; i++) {
            LineData[0].data[i].y = ConstantData[0].data[i].y + diffVal;
        };

        return LineData;
    }

    this.AlterShouldersWidth = function (LineData, ConstantData, value) {
        maxLength = ConstantData[0].data.length - 1;
        var factor = ConstantData[0].data[3].x / 16;
        var diffVal = (ConstantData[0].data[3].x - (value * factor)) / 2;
        var diffValArmpit = (ConstantData[0].data[3].x - (value * factor)) / 3;

        LineData[0].data[3].x = ConstantData[0].data[3].x + diffVal;
        LineData[0].data[maxLength - 3].x = ConstantData[0].data[maxLength - 3].x - diffVal;
        LineData[1].data[0].x = ConstantData[1].data[0].x + diffValArmpit;
        LineData[2].data[0].x = ConstantData[2].data[0].x - diffValArmpit;
 
        return LineData;
    }

    this.AlterArmpitWidth = function (LineData, ConstantData, value) {
        maxLength = ConstantData[1].data.length - 1;
        
        var factor = ConstantData[1].data[maxLength].y / 9;
        var diffVal = -(ConstantData[1].data[maxLength].y - (value * factor))/4;

       
        LineData[1].data[maxLength].y = ConstantData[1].data[maxLength].y + diffVal;
        LineData[2].data[maxLength].y = ConstantData[2].data[maxLength].y + diffVal;


        return LineData;
    }

    this.AlterSleeveOpeningWidth = function (LineData, ConstantData, value) {
        maxLength = ConstantData[1].data.length - 1;

        var factor = ConstantData[1].data[1].x / 9;
        var diffVal = -(ConstantData[1].data[1].x - (value * factor));

        LineData[1].data[1].x = ConstantData[1].data[1].x - diffVal;
        LineData[2].data[1].x = ConstantData[2].data[1].x + diffVal;


        return LineData;
    }

    this.AlterSleeveLength = function (LineData, ConstantData, value) {
        var factor = [
            MathService.getLengthPythogras(ConstantData[1].data[0], ConstantData[1].data[1]) / 19,
            MathService.getLengthPythogras(ConstantData[1].data[2], ConstantData[1].data[3])
        ];
        var ratioUpper = [
            MathService.getLengthPythogras(ConstantData[1].data[0], ConstantData[1].data[1]),
            MathService.getLengthPythogras(ConstantData[1].data[0], ConstantData[1].data[1]) - factor[0] * value
        ];

        var ratioLower = [
            MathService.getLengthPythogras(ConstantData[1].data[2], ConstantData[1].data[3]),
            MathService.getLengthPythogras(ConstantData[1].data[2], ConstantData[1].data[3]) - factor[1] * value
        ];
        
        //Left Sleeve
        LineData[1].data[1].x = (ratioUpper[0] * ConstantData[1].data[1].x + ratioUpper[1] * LineData[1].data[3].x) / (ratioUpper[0] + ratioUpper[1]);
        LineData[1].data[1].y = (ratioUpper[0] * ConstantData[1].data[1].y + ratioUpper[1] * LineData[1].data[3].y) / (ratioUpper[0] + ratioUpper[1]);
        LineData[1].data[2].x = (ratioUpper[0] * ConstantData[1].data[2].x + ratioUpper[1] * LineData[1].data[3].x) / (ratioUpper[0] + ratioUpper[1]);
        LineData[1].data[2].y = (ratioUpper[0] * ConstantData[1].data[2].y + ratioUpper[1] * LineData[1].data[3].y) / (ratioUpper[0] + ratioUpper[1]);

        //Right Sleeve
        LineData[2].data[1].x = (ratioUpper[0] * ConstantData[2].data[1].x + ratioUpper[1] * LineData[2].data[3].x) / (ratioUpper[0] + ratioUpper[1]);
        LineData[2].data[1].y = (ratioUpper[0] * ConstantData[2].data[1].y + ratioUpper[1] * LineData[2].data[3].y) / (ratioUpper[0] + ratioUpper[1]);
        LineData[2].data[2].x = (ratioUpper[0] * ConstantData[2].data[2].x + ratioUpper[1] * LineData[2].data[3].x) / (ratioUpper[0] + ratioUpper[1]);
        LineData[2].data[2].y = (ratioUpper[0] * ConstantData[2].data[2].y + ratioUpper[1] * LineData[2].data[3].y) / (ratioUpper[0] + ratioUpper[1]);

        return LineData;
    }

    this.AlterBreastWidth = function (LineData, ConstantData, value) {
        maxLength = ConstantData[0].data.length - 1;
        var factor = [
            (ConstantData[0].data[0 + 4].x) / 32,
            (ConstantData[0].data[1 + 4].x) / 32,
            (ConstantData[0].data[2 + 4].x) / 32
        ];

        var diffVal = [
            ((ConstantData[0].data[0 + 4].x) - (value * factor[0])) / 3,
            ((ConstantData[0].data[1 + 4].x) - (value * factor[1])) / 2,
            ((ConstantData[0].data[2 + 4].x) - (value * factor[2])) / 4
        ];

        //Sleeves Left
        LineData[1].data[0].x = ConstantData[1].data[0].x + (diffVal[0] / 3.5);
        LineData[1].data[1].x = ConstantData[1].data[1].x + diffVal[0] ;
        LineData[1].data[2].x = ConstantData[1].data[2].x + diffVal[0] ;
        LineData[1].data[3].x = ConstantData[1].data[3].x + (diffVal[0] / 0.9);

        //Sleeves Right
        LineData[2].data[0].x = ConstantData[2].data[0].x - (diffVal[0]/3.5);
        LineData[2].data[1].x = ConstantData[2].data[1].x - diffVal[0];
        LineData[2].data[2].x = ConstantData[2].data[2].x - diffVal[0];
        LineData[2].data[3].x = ConstantData[2].data[3].x - (diffVal[0] / 0.9);

        //Bust Width
        LineData[0].data[0 + 4].x = ConstantData[0].data[0 + 4].x + diffVal[0];
        LineData[0].data[maxLength - 4].x = ConstantData[0].data[maxLength - 4 - 0].x - diffVal[0];
        LineData[0].data[1 + 4].x = ConstantData[0].data[1 + 4].x + diffVal[1];
        LineData[0].data[maxLength - 4 - 1].x = ConstantData[0].data[maxLength - 4 - 1].x - diffVal[1];
        LineData[0].data[2 + 4].x = ConstantData[0].data[2 + 4].x + diffVal[2];
        LineData[0].data[maxLength - 4 - 2].x = ConstantData[0].data[maxLength - 4 - 2].x - diffVal[2];

        return LineData;
    }

    this.AlterWaistWidth = function (LineData, ConstantData, value) {
        maxLength = ConstantData[0].data.length - 1;
        var factor = ConstantData[0].data[3 + 4].x / 24;
        var diffVal = ((ConstantData[0].data[3 + 4].x) - (value * factor)) / 4;
        LineData[0].data[3 + 4].x = ConstantData[0].data[3 + 4].x + diffVal;
        LineData[0].data[maxLength - 4 - 3].x = ConstantData[0].data[maxLength - 4 - 3].x - diffVal;

        //kain
        maxLengthKain = ConstantData[3].data.length - 1;
        var factor = ConstantData[3].data[1].x / 24;
        var diffVal = ((ConstantData[3].data[1].x) - (value * factor)) / 14;
        LineData[3].data[1].x = ConstantData[3].data[1].x + diffVal;
        LineData[3].data[2].x = ConstantData[3].data[2].x + diffVal;
        LineData[3].data[3].x = ConstantData[3].data[3].x + diffVal;
        LineData[3].data[4].x = ConstantData[3].data[4].x + diffVal;
        LineData[3].data[5].x = ConstantData[3].data[5].x + diffVal;
        LineData[3].data[maxLengthKain - 1].x = ConstantData[3].data[maxLengthKain - 1].x - diffVal;
        LineData[3].data[maxLengthKain - 2].x = ConstantData[3].data[maxLengthKain - 2].x - diffVal;
        LineData[3].data[maxLengthKain - 3].x = ConstantData[3].data[maxLengthKain - 3].x - diffVal;
        LineData[3].data[maxLengthKain - 4].x = ConstantData[3].data[maxLengthKain - 4].x - diffVal;
        return LineData;
    }

    this.AlterHipWidth = function (LineData, ConstantData, value) {
        maxLength = ConstantData[0].data.length - 1;
        var factor = [
            ConstantData[0].data[4 + 4].x / 34,
            ConstantData[0].data[5 + 4].x / 34,
            ConstantData[0].data[6 + 4].x / 34,
            ConstantData[0].data[7 + 4].x / 34
        ];

        var diffVal = [
            ((ConstantData[0].data[4 + 4].x) - (value * factor[0])) / 2,
            ((ConstantData[0].data[5 + 4].x) - (value * factor[1])) / 2,
            ((ConstantData[0].data[6 + 4].x) - (value * factor[2])) / 2,
            ((ConstantData[0].data[7 + 4].x) - (value * factor[3])) / 2
        ];

        LineData[0].data[4 + 4].x =                 ConstantData[0].data[4 + 4].x + diffVal[0];
        LineData[0].data[maxLength - 4 - 4].x =     ConstantData[0].data[maxLength - 4 - 4].x - diffVal[1];
        LineData[0].data[5 + 4].x =                 ConstantData[0].data[5 + 4].x + diffVal[0];
        LineData[0].data[maxLength - 4 - 5].x =     ConstantData[0].data[maxLength - 4 - 5].x - diffVal[1];
        LineData[0].data[6 + 4].x =                 ConstantData[0].data[6 + 4].x + diffVal[0];
        LineData[0].data[maxLength - 4 - 6].x =     ConstantData[0].data[maxLength - 4 - 6].x - diffVal[1];
        LineData[0].data[7 + 4].x =                 ConstantData[0].data[7 + 4].x + diffVal[0];
        LineData[0].data[maxLength - 4 - 7].x = ConstantData[0].data[maxLength - 4 - 7].x - diffVal[1];


        //kain
        maxLengthKain = ConstantData[3].data.length - 1;
        var factor = ConstantData[3].data[1].x / 34;
        var diffVal = ((ConstantData[3].data[1].x) - (value * factor)) / 10;
        //LineData[3].data[5].x = ConstantData[3].data[5].x + diffVal;
        LineData[3].data[6].x = ConstantData[3].data[6].x + diffVal;
        LineData[3].data[7].x = ConstantData[3].data[7].x + diffVal;
        LineData[3].data[8].x = ConstantData[3].data[8].x + diffVal;
        //LineData[3].data[9].x = ConstantData[3].data[9].x + diffVal;
        //LineData[3].data[10].x = ConstantData[3].data[10].x + diffVal;
        //LineData[3].data[11].x = ConstantData[3].data[11].x + diffVal;
        //LineData[3].data[maxLengthKain - 5].x = ConstantData[3].data[maxLengthKain - 5].x - diffVal;
        LineData[3].data[maxLengthKain - 6].x = ConstantData[3].data[maxLengthKain - 6].x - diffVal;
       LineData[3].data[maxLengthKain - 7].x = ConstantData[3].data[maxLengthKain - 7].x - diffVal;
        LineData[3].data[maxLengthKain - 8].x = ConstantData[3].data[maxLengthKain - 8].x - diffVal;
        LineData[3].data[maxLengthKain - 9].x = ConstantData[3].data[maxLengthKain - 9].x - diffVal;

        return LineData;
    }
});