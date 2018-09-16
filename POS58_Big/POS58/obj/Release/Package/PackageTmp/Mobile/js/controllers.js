angular.module('app.controllers', [])
.run(function ($rootScope) {
    $rootScope.items = [];
    $rootScope.$on("addItem", function (event, item) {
        var nohave = true;

        //console.log("addItem item.sid:" + item.sid);
        //判斷購物車有沒有，沒有才放進去
        angular.forEach($rootScope.items, function (_item, key) {
            if (_item.sid == item.sid) {
                nohave = false;
            }
        });
        if (nohave) $rootScope.items.push(item);
        console.log($rootScope.items);
        countTotal();
    });

    //計算總金額
    countTotal = function () {
        $rootScope.total = 0;
        angular.forEach($rootScope.items, function (_item, key) {
            $rootScope.total = $rootScope.total + (_item.dprice * _item.num);
        });
    }

    clear = function () {
        $rootScope.items = [];
        $rootScope.total = 0;
        console.log('clear!!');
    }
})
.controller('menuCtrl', ['$scope', '$stateParams', // The following is the constructor function for this page's controller. See https://docs.angularjs.org/guide/controller
// You can include any angular dependencies as parameters for this function
// TIP: Access Route Parameters for your page via $stateParams.parameterName
function ($scope, $stateParams) {

}])
   
.controller('homeCtrl', ['$scope', '$stateParams', // The following is the constructor function for this page's controller. See https://docs.angularjs.org/guide/controller
// You can include any angular dependencies as parameters for this function
// TIP: Access Route Parameters for your page via $stateParams.parameterName
function ($scope, $stateParams) {


}])

//購物車
.controller('shopcartCtrl', ['$scope', '$state', '$ionicPopup', '$ionicLoading', '$http', '$stateParams', '$rootScope', // The following is the constructor function for this page's controller. See https://docs.angularjs.org/guide/controller
// You can include any angular dependencies as parameters for this function
// TIP: Access Route Parameters for your page via $stateParams.parameterName
function ($scope, $state, $ionicPopup, $ionicLoading, $http, $stateParams, $rootScope) {
    $rootScope.currentPage = "shopcart";
    console.log("enter cart page");

    //一開始進入
    $scope.$on("$ionicView.enter", function (event, data) {
        countTotal();
    });

    //[button]增加數量
    $scope.increase = function (item) {
        if (item.num > 0) {
            item.num++;
            item.money = item.dprice * item.num;
            countTotal();
        }
    }

    //[button]減少數量
    $scope.decrease = function (item) {
        if (item.num > 1) {
            item.num--;
            item.money = item.dprice * item.num;
            countTotal();
        }
    }

    //計算總金額
    countTotal = function () {
        $scope.total = 0;
        angular.forEach($scope.items, function (_item, key) {
            $scope.total = $scope.total + (_item.dprice * _item.num);
        });
    }

    //送出訂單
    $scope.submitOrderForm = function () {
        var _desc = document.getElementById("odesc").value;
        console.log('document.desc:' + _desc);
        //$rootScope.usid = '15';
        if (!angular.isUndefined($rootScope.usid)) {
            //var _params;
            //data: { order: angular.toJson($scope.selected), usid: $rootScope.usid }
            console.log($rootScope.usid);
            $http.post('Service/OrderData.asmx/InsertOrder', { "usid": $rootScope.usid, "order": angular.toJson($scope.items), "desc": _desc }).then(function (res) {
                $scope.response = res.data;
                console.log('xxxxxxxxxxxxxx');
                var alertPopup = $ionicPopup.alert({
                    title: 'Message',
                    template: 'Thank you for your order.'
                });

                alertPopup.then(function (res) {
                    console.log('Thank you for your order.');
                    clear();
                    document.getElementById("odesc").value = "";
                    $state.go('orderlist');
                });
            });
        } else {
            $state.go('login');
        }

        //JSON.stringify($scope.selected)
    }

    //移除Item
    $scope.removeItem = function (item) {
        var index = $rootScope.items.indexOf(item);
        $rootScope.items.splice(index, 1);
        countTotal();
    }

}])

//Order List=================   
.controller('OrderListCtrl', ['$scope', '$http', '$ionicLoading', '$stateParams', '$rootScope', '$state', // The following is the constructor function for this page's controller. See https://docs.angularjs.org/guide/controller
// You can include any angular dependencies as parameters for this function
// TIP: Access Route Parameters for your page via $stateParams.parameterName
function ($scope, $http, $ionicLoading, $stateParams, $rootScope, $state) {
    $rootScope.currentPage = "orderlist";
    $scope.action = function(){
        console.log("here");
    };

    

    $scope.orders = [
        { no: 'Jani', items: [{ price: 2300, name: 'cookie' }, { price: 300, name: 'coca' }] },
        { no: 'Jani', items: [{ price: 2300, name: 'cookie' }, { price: 300, name: 'coca' }] },
        { no: 'Jani', items: [{ price: 2300, name: 'cookie' }, { price: 300, name: 'coca' }] }
    ];

    $scope.OrderController = function () {
        $scope.order = "<div id='page3-markdown4' class='show-list-numbers-and-dots'><h2>訂單編號： **markdown**</h2><button ng-click='action()'>Go to second tab</button><h2>物品清單：</h2><p style='margin-top:0px;color:#000000;'>薯條三兄弟(50) x 1 = 50</p></div>";
    };

    //讀取中
    $scope.show = function () {
        $ionicLoading.show({
            template: 'Loading...',
            duration: 3000
        }).then(function () {
            console.log("The loading indicator is now displayed");
        });
    };
    $scope.hide = function () {
        $ionicLoading.hide().then(function () {
            console.log("The loading indicator is now hidden");
        });
    };

    $scope.show();
    $scope.$on("$ionicView.enter", function (event, data) {
        // handle event
        //$rootScope.usid = "1"; //test use
        if (!angular.isUndefined($rootScope.usid)) {
            console.log("State Params: ", data.stateParams);
            $http.post('Service/OrderData.asmx/getAllOrder', { usid: $rootScope.usid })
             .then(function (response) {
                 console.log(response.data.d);
                 $scope.orders = eval(response.data.d);
                 $scope.hide();
             });
        } else {
            $state.go('login');
        }
        
    });

}])
   
.controller('aboutCtrl', ['$scope', '$stateParams', // The following is the constructor function for this page's controller. See https://docs.angularjs.org/guide/controller
// You can include any angular dependencies as parameters for this function
// TIP: Access Route Parameters for your page via $stateParams.parameterName
function ($scope, $stateParams) {


}])
      
//登入
.controller('loginCtrl', ['$scope', '$stateParams', '$http', '$ionicPopup', '$state', '$rootScope', // The following is the constructor function for this page's controller. See https://docs.angularjs.org/guide/controller
// You can include any angular dependencies as parameters for this function
// TIP: Access Route Parameters for your page via $stateParams.parameterName
function ($scope, $stateParams, $http, $ionicPopup, $state, $rootScope) {
    $scope.Submit = function () {
        console.log("xxx");
        var _name = document.getElementById("username").value;
        var _pass = document.getElementById("pass").value;

        //var _params;
        $http.post('Service/UserData.asmx/UserCheck', { ename: _name, pass: _pass}).then(function (res) {
            $scope.response = res.data;
            var _res = eval(res.data.d);
            console.log(_res[0].name);
            if (_res[0].result == 'OK') {
                console.log('xxxxxxxxxxxxxx');
                $rootScope.ename = _res[0].name;
                $rootScope.usid = _res[0].usid;
                var alertPopup = $ionicPopup.alert({
                    title: 'Message',
                    template: 'Success!!'
                });

                alertPopup.then(function (res) {
                    console.log($rootScope.currentPage+'Success!!');
                    if ($rootScope.currentPage == "shopcart") {
                        $state.go('tabsController.shopcart');
                    }

                    if ($rootScope.currentPage == "orderlist") {
                        $state.go('orderlist');
                    }

                    if ($rootScope.currentPage == undefined) {
                        $state.go('tabsController.home');
                    }
                    
                });
            } else {
                var alertPopup = $ionicPopup.alert({
                    title: 'Message',
                    template: 'Fail!!'
                });

            }
        });
        //JSON.stringify($scope.selected)

    }

}])

//註冊   
.controller('signupCtrl', ['$scope', '$state', '$ionicPopup', '$stateParams', '$http', '$ionicLoading',  // The following is the constructor function for this page's controller. See https://docs.angularjs.org/guide/controller
// You can include any angular dependencies as parameters for this function
// TIP: Access Route Parameters for your page via $stateParams.parameterName
function ($scope, $state, $ionicPopup, $stateParams, $http, $ionicLoading) {
    //讀取中
    $scope.show = function () {
        $ionicLoading.show({
            template: 'Loading...',
            duration: 10000
        }).then(function () {
            console.log("The loading indicator is now displayed");
        });
    };
    $scope.hide = function () {
        $ionicLoading.hide().then(function () {
            console.log("The loading indicator is now hidden");
        });
    };

    $scope.Submit = function () {
        $scope.show();
        console.log(document.getElementById("ename").value);
        var _ename = document.getElementById("ename").value;
        var _phone = document.getElementById("phone").value;
        var _pass = document.getElementById("password").value;
        var _email = document.getElementById("email").value;
        //var _params;
        $http.post('Service/UserData.asmx/InsertUser', { ename: _ename, phone: _phone, pass: _pass, email: _email }).then(function (res) {
            $scope.response = res.data;
            console.log('xxxxxxxxxxxxxx');
            var alertPopup = $ionicPopup.alert({
                title: 'Message',
                template: 'Success!!'
            });

            alertPopup.then(function (res) {
                console.log('Success!!');
                $scope.hide();
                $state.go('login');
            });

        });
        //JSON.stringify($scope.selected)

    }

}])

//物品清單
.controller('itemlistCtrl', ['$scope', '$state', '$ionicPopup', '$ionicLoading', '$http', '$stateParams', '$rootScope', // The following is the constructor function for this page's controller. See https://docs.angularjs.org/guide/controller
// You can include any angular dependencies as parameters for this function
// TIP: Access Route Parameters for your page via $stateParams.parameterName
function ($scope, $state, $ionicPopup, $ionicLoading, $http, $stateParams, $rootScope) {
    console.log('itemlistCtrl');
    console.log('again');
    clear();
    //讀取中
    $scope.show = function () {
        $ionicLoading.show({
            template: 'Loading...',
            duration: 3000
        }).then(function () {
            console.log("The loading indicator is now displayed");
        });
    };
    $scope.hide = function () {
        $ionicLoading.hide().then(function () {
            console.log("The loading indicator is now hidden");
        });
    };
    
    $scope.show();
    $http.get('Service/ItemData.asmx/getCookieItemData')
         .then(function (response) {
             $scope.items = response.data;
             $scope.hide();
         });
    
    $scope.selected = [];
    $scope.total = 0;
    $scope.clickItem = function (item) {
        if (item.checked) {
            $scope.selected.push(item);
            if (isNaN(item.total)) item.total = item.dprice;
        } else {
            var _index = $scope.selected.indexOf(item);
            $scope.selected.splice(_index, 1);
            item.total = "";
        }
    };

    $scope.cul = function (item) {
        if(item.checked) item.total = item.num * item.dprice;
    };

    //[button]放入購物車
    $scope.putItem = function (item) {
        console.log("addItem");
        $rootScope.$broadcast("addItem", item);

        var alertPopup = $ionicPopup.alert({
            title: 'Message',
            template: '加入成功!!'
        });
    };

    //[button]增加數量
    $scope.increase = function (item) {
        console.log("++");
        if (item.num>0) item.num++;
    }

    //[button]減少數量
    $scope.decrease = function (item) {
        console.log("--");
        if (item.num > 1) item.num--;
    }

    //設定數量[停用]
    $scope.setNum = function (item,num) {
        item.num = num;
    };

    

}])

//圖釘物品清單
.controller('pushpinCtrl', ['$scope', '$state', '$ionicPopup', '$ionicLoading', '$http', '$stateParams', '$rootScope', // The following is the constructor function for this page's controller. See https://docs.angularjs.org/guide/controller
// You can include any angular dependencies as parameters for this function
// TIP: Access Route Parameters for your page via $stateParams.parameterName
function ($scope, $state, $ionicPopup, $ionicLoading, $http, $stateParams, $rootScope) {
    console.log('pushpinCtrl');
    clear();
    //讀取中
    $scope.show = function () {
        $ionicLoading.show({
            template: 'Loading...',
            duration: 3000
        }).then(function () {
            console.log("The loading indicator is now displayed");
        });
    };
    $scope.hide = function () {
        $ionicLoading.hide().then(function () {
            console.log("The loading indicator is now hidden");
        });
    };

    $scope.show();
    $http.get('Service/ItemData.asmx/getPushPinItemData')
         .then(function (response) {
             $scope.items = response.data;
             $scope.hide();
         });

    $scope.selected = [];
    $scope.total = 0;
    $scope.clickItem = function (item) {
        if (item.checked) {
            $scope.selected.push(item);
            if (isNaN(item.total)) item.total = item.dprice;
        } else {
            var _index = $scope.selected.indexOf(item);
            $scope.selected.splice(_index, 1);
            item.total = "";
        }
    };

    $scope.cul = function (item) {
        if (item.checked) item.total = item.num * item.dprice;
    };

    //[button]放入購物車
    $scope.putItem = function (item) {
        console.log("addItem");
        $rootScope.$broadcast("addItem", item);
    };

    //[button]增加數量
    $scope.increase = function (item) {
        console.log("++");
        if (item.num > 0) item.num++;
    }

    //[button]減少數量
    $scope.decrease = function (item) {
        console.log("--");
        if (item.num > 1) item.num--;
    }

    //設定數量[停用]
    $scope.setNum = function (item, num) {
        item.num = num;
    };

    //[button]放入購物車
    $scope.putItem = function (item) {
        console.log("addItem");
        $rootScope.$broadcast("addItem", item);

        var alertPopup = $ionicPopup.alert({
            title: 'Message',
            template: '加入成功!!'
        });
    };

}])
 