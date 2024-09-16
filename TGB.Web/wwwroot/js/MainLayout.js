const sideMenuOpenKey = 'SIDE_MENU_OPEN';

function ApplyLastSlideState() {
    var slideState = sessionStorage.getItem(sideMenuOpenKey);

    if (slideState === 'false') {
        SideSlideHidden()
        var sideMenu = document.getElementById('sideMenu');
        sideMenu.classList.remove('show');
    }
}

function SideSlideShown() {
    let toggleExpandIcon = document.getElementById('toggleExpand');

    toggleExpandIcon.classList.add('fa-minimize');
    toggleExpandIcon.classList.remove('fa-expand');
    sessionStorage.setItem(sideMenuOpenKey, 'true');
}

function SideSlideHidden() {
    let toggleExpandIcon = document.getElementById('toggleExpand');

    toggleExpandIcon.classList.add('fa-expand');
    toggleExpandIcon.classList.remove('fa-minimize');
    sessionStorage.setItem(sideMenuOpenKey, 'false');
}

$(document).ready(function () {
    var sideMenu = document.getElementById('sideMenu');

    sideMenu.addEventListener('hide.bs.collapse', () => {
        SideSlideHidden();
    });

    sideMenu.addEventListener('show.bs.collapse', () => {
        SideSlideShown();
    });

    ApplyLastSlideState();
});