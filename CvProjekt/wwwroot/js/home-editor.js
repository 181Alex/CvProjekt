

var seMore = document.getElementById("SeeMoreBtn");
var seLess = document.getElementById("SeeLessBtn");
var AllP = document.getElementById("AllProjectsBox");
var LatestP = document.getElementById("LatestProjectsBox");

var AllProfiles = document.getElementById("AllProfilesBox");
var LatestProfiles = document.getElementById("LatestProfilesBox");

function seMoreNow() {
    LatestP.style.display = 'none';
    AllP.style.display = 'block';
}

function seLessNow() {
    LatestP.style.display = 'flex';
    AllP.style.display = 'none';
}

function seMoreProfilesNow() {
    LatestProfiles.style.display = 'none';
    AllProfiles.style.display = 'block';
}

function seLessProfilesNow() {
    LatestProfiles.style.display = 'flex';
    AllProfiles.style.display = 'none';
}
