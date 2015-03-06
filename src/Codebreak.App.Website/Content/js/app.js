function bootAngular() {
    var app = angular.module('earthscape', ['chieffancypants.loadingBar', 'ngAnimate']);

    app.config(function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = true;
    })

    app.controller('earthscape.controller.loading', ["$scope", "$http", "$timeout", "cfpLoadingBar", function ($scope, $http, $timeout, cfpLoadingBar) {
        $scope.start = function () {
            cfpLoadingBar.start();
        };

        $scope.complete = function () {
            cfpLoadingBar.complete();
        };

        $scope.start();
        $timeout(function () {
            $scope.complete();
        }, 200);
    }]);
}

function bootSignalR() {
    var statisticProxy = $.connection.statisticHub;
    statisticProxy.client.notifyVisitorCount = function (count) {
        $("#visitor-count").text(count);
    };
    statisticProxy.client.notifyPlayersConnected = function (count, players) {
        $("#players-connected").html(count + " <br> " + players);
    };
    $.connection.hub.start().done(function () {

    });
}

function initLang() {
    $.i18n.init({
        resGetPath: "/Content/lang/__lng__/__ns__.json",
        debug: true
    }).done(function (t) {
        translate();
    });
}

function setLang(lang) {
    $.i18n.setLng(lang).done(function (t) {
        translate();
    });
}

function translate() {
    $(document).i18n();
}

function appStart() {
    initLang();
    bootAngular();
    bootSignalR();
}