app.service('MathService', function () {
    this.getLengthPythogras = function (a, b) {
        var m = Math.pow(a.x - b.x, 2);
        var n = Math.pow(a.y - b.y, 2);
        return Math.sqrt( m + n );
    }

    this.Guid = function () {
        var d = new Date().getTime();
        var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return guid;
    }
});