angular.module('app.routes', [])

.config(function($stateProvider, $urlRouterProvider) {

  // Ionic uses AngularUI Router which uses the concept of states
  // Learn more here: https://github.com/angular-ui/ui-router
  // Set up the various states which the app can be in.
  // Each state's controller can be found in controllers.js
  $stateProvider
    
  
//首頁
  .state('tabsController.home', {
    url: '/home',
    views: {
      'tab1': {
        templateUrl: 'templates/home.html',
        controller: 'homeCtrl'
      }
    }
  })

//購物車
  .state('tabsController.shopcart', {
    url: '/shopcart',
    views: {
      'tab2': {
        templateUrl: 'templates/shopcart.html',
        controller: 'shopcartCtrl'
      }
    }
  })
//關於我們
  .state('tabsController.page4', {
    url: '/about',
    views: {
      'tab3': {
        templateUrl: 'templates/about.html',
        controller: 'aboutCtrl'
      }
    }
  })

  .state('tabsController', {
      url: '/page1',
    templateUrl: 'templates/tabsController.html',
    abstract:true
  })

  .state('login', {
    url: '/login',
    templateUrl: 'templates/login.html',
    controller: 'loginCtrl'
  })

  .state('signup', {
    url: '/signup',
    templateUrl: 'templates/signup.html',
    controller: 'signupCtrl'
  })

  .state('tabsController.page7', {
    url: '/item',
    views: {
      'tab1': {
        templateUrl: 'templates/Itemlist.html',
        controller: 'itemlistCtrl'
      }
    }
  })
    .state('tabsController.pushpin', {
        url: '/item',
        views: {
            'tab1': {
                templateUrl: 'templates/pushpin.html',
                controller: 'pushpinCtrl'
            }
        }
    })

  .state('orderlist', {
      url: '/orderlist',
      templateUrl: 'templates/orderlist.html',
      controller: 'OrderListCtrl'
  })


  $urlRouterProvider.otherwise('/page1/home')

  

});