window.interloop = {
    copyText: function (text) {
        navigator.clipboard.writeText(text).then(function () {
        })
            .catch(function (error) {
                alert(error);
            });
    },
    shareUrl: function (url) {
        navigator.share({ url: url });
    },
};
