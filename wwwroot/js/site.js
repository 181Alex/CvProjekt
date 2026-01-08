// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// CvProjekt/wwwroot/js/site.js


function seMoreProfilesNow() {
    var latestProfiles = document.getElementById("LatestProfilesBox");
    var allProfiles = document.getElementById("AllProfilesBox");

    if (latestProfiles && allProfiles) {
        latestProfiles.style.display = 'none';
        allProfiles.style.display = 'block';
    }
}


function seLessProfilesNow() {
    var latestProfiles = document.getElementById("LatestProfilesBox");
    var allProfiles = document.getElementById("AllProfilesBox");

    if (latestProfiles && allProfiles) {
        latestProfiles.style.display = 'flex';
        allProfiles.style.display = 'none';
    }
}
