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
        return localStorage["ChickenTinder:" + key];
    },
    setLocalStorage: (key, value) => {
        localStorage["ChickenTinder:" + key] = value;
    },
    LaunchMaps: (lat, long) => {

        var userAgent = navigator.userAgent || navigator.vendor || window.opera;

        if (/windows phone/i.test(userAgent)) {

        }
        else if (/android/i.test(userAgent)) {
            window.open("http://maps.google.com/maps?saddr=" + lat + "," + long, "_blank");
        }
        else if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream) {
            window.open("http://maps.apple.com/?ll=" + lat + "," + long, "_blank");
        }
        else {
            window.open("https://www.google.com/maps/search/?api=1&query=" + lat + "," + long, "_blank");
        }

    },
    LaunchMapsAdress: (address) => {

        var userAgent = navigator.userAgent || navigator.vendor || window.opera;

        if (/windows phone/i.test(userAgent)) {

        }
        else if (/android/i.test(userAgent)) {
            window.location.href = "http://maps.google.com/maps?saddr=" + address;
        }
        else if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream) {
            window.location.href = "http://maps.apple.com/?address=" + address;
        }
        else {
            window.location.href = "https://www.google.com/maps/search/?api=1&query=" + address;
        }
    }
};
