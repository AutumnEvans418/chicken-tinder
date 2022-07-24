window.interloop = {
    copyText: (text) => {
        navigator.clipboard.writeText(text).then(function () {
        })
            .catch(function (error) {
                alert(error);
            });
    },
    shareUrl: (url) => {
        navigator.share({ url: url });
    },
    getLocalStorage: (key) => {
        return localStorage[key];
    },
    setLocalStorage: (key, value) => {
        localStorage[key] = value;
    },
    LaunchMaps: (lat, long) => {

        var userAgent = navigator.userAgent || navigator.vendor || window.opera;

        if (/windows phone/i.test(userAgent)) {
         
        }
        else if (/android/i.test(userAgent)) {
            window.location.href = "http://maps.google.com/maps?saddr=" + lat + "," + long;
        }
        else if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream) {
            window.location.href = "http://maps.apple.com/?ll=" + lat + "," + long;
        }
        else {
            window.location.href = "https://www.google.com/maps/search/?api=1&query=" + lat + "," + long;
        }
    }
};
