app.controller('HomeCtrl', function ($scope, $rootScope, $state) {
    $rootScope.loading = "visible";
    YUI().use('aui-menu', 'aui-alert', function (Y) {

        var primaryMenu = new Y.Menu({
            boundingBox: '#primary-menu',
            contentBox: '#primary-menu',
            trigger: '#trigger',
            items: [
                 {
                     content: '<a ui-sref="Template">New</a>',
                     shortcut: {
                         ctrlKey: true,
                         altKey: true,
                         keys: ['N', 'n'],
                         text: 'Ctrl+ Alt + N'
                     }
                 },
                 {
                     divider: true
                 },
                {
                    content: '<a href="#">Save</a>',
                    shortcut: {
                        ctrlKey: true,
                        keys: ['S', 's'],
                        text: 'Ctrl + S'
                    }
                },
                
                {
                    content: '<a>Save As</a>',
                    shortcut: {
                        ctrlKey: true,
                        altKey: true,
                        keys: ['S', 's'],
                        text: 'Ctrl + Alt + S'
                    }
                }
            ],

        }).render();

        primaryMenu.after('itemSelected', function (event) {
            //Y.one('#message').text(event.item.get('content'));
            var temp = document.createElement('div');
            temp.innerHTML = event.item.get('content');
            $state.go(temp.firstChild.getAttribute('ui-sref'));
            new Y.Alert(
               {
                   animated: true,
                   bodyContent: event.item.get('content'),
                   boundingBox: '#message',
                   closeable: true,
                   cssClass: 'alert-warning',
                   destroyOnHide: false,
                   duration: 1,
                   render: true
               })

        });




    });
    $scope.OnLoad = function () {
        $rootScope.loading = "visible";
    }
});