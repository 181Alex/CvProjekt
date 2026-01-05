
    var seMore = document.getElementById("SeeMoreBtn")
    var seLess = document.getElementById("SeeLessBtn")
    var AllP = document.getElementById("AllProjectsBox")
    var LatestP = document.getElementById("LatestProjectsBox")

    AllP.style.display = 'none'; // döljs från start

    function seMoreNow() { 
        LatestP.style.display = 'none';
        AllP.style.display = 'block';
    }

    function seLessNow() {
        LatestP.style.display = 'flex';
        AllP.style.display = 'none';
    }