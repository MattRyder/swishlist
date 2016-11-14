$(document).ready(function () {

    var navScrollMonitor = scrollMonitor.create($("#navbar-home"), 300);
    navScrollMonitor.lock();

    navScrollMonitor.enterViewport(function () {
        $(this.watchItem).removeClass("slideInDown").removeClass("navbar-fixed-top");
    });
    navScrollMonitor.exitViewport(function () {
        $(this.watchItem).addClass("slideInDown").addClass("navbar-fixed-top");
    });
});