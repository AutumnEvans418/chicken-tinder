var dotRef = null;
var tinderContainer = null;
var allCards = null;

window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text).then(function () {
        })
            .catch(function (error) {
                alert(error);
            });
    }
};

function createButtonListener(love) {
    return function (event) {
        var cards = document.querySelectorAll('.tinder--card:not(.removed)');
        var moveOutWidth = document.body.clientWidth * 1.5;

        if (!cards.length) return false;

        var card = cards[0];

        card.classList.add('removed');

        if (love == Hammer.DIRECTION_RIGHT) {
            card.style.transform = 'translate(' + moveOutWidth + 'px, -100px) rotate(-30deg)';
        } else if (love == Hammer.DIRECTION_LEFT) {
            card.style.transform = 'translate(-' + moveOutWidth + 'px, -100px) rotate(30deg)';
        } else if (love == Hammer.DIRECTION_UP) {
            card.style.transform = 'translate(-100px, -' + moveOutWidth + 'px) rotate(10deg)';
        } else if (love == Hammer.DIRECTION_DOWN) {
            card.style.transform = 'translate(-100px, ' + moveOutWidth + 'px) rotate(-10deg)';
        }
        swiped(love);
        window.Swipe.initCards();

        event.preventDefault();
    };
}

var nopeListener = createButtonListener(Hammer.DIRECTION_DOWN);
var loveListener = createButtonListener(Hammer.DIRECTION_UP);
var likeListener = createButtonListener(Hammer.DIRECTION_RIGHT);
var dislikeListener = createButtonListener(Hammer.DIRECTION_LEFT);

function swiped(direction) {
    dotRef.invokeMethodAsync("Swipe", direction);
}

window.Swipe = {
    initCards: () => {
        var newCards = document.querySelectorAll('.tinder--card:not(.removed)');

        newCards.forEach(function (card, index) {
            card.style.zIndex = allCards.length - index;
            card.style.transform = 'scale(' + (20 - index) / 20 + ') translateY(-' + 30 * index + 'px)';
            card.style.opacity = (10 - index) / 10;
        });

        tinderContainer.classList.add('loaded');
    },
    start: (ref) => {
        dotRef = ref;
        tinderContainer = document.querySelector('.tinder');
        allCards = document.querySelectorAll('.tinder--card');
        var nope = document.getElementById('nope');
        var love = document.getElementById('love');
        var like = document.getElementById('like');
        var dislike = document.getElementById('dislike');
        function initCards() {
            window.Swipe.initCards();
        }

        initCards();

        allCards.forEach(function (el) {
            var hammertime = new Hammer(el);
            hammertime.add(new Hammer.Pan({
                position: Hammer.position_ALL,
                threshold: 0
            }));
            hammertime.on('pan', function (event) {
                el.classList.add('moving');
            });

            hammertime.on('pan', function (event) {
                if (event.deltaX === 0) return;
                if (event.center.x === 0 && event.center.y === 0) return;

                var direction = event.direction;
                tinderContainer.classList.toggle('tinder_like', direction == Hammer.DIRECTION_RIGHT);
                tinderContainer.classList.toggle('tinder_nope', direction == Hammer.DIRECTION_DOWN);
                tinderContainer.classList.toggle('tinder_love', direction == Hammer.DIRECTION_UP);
                tinderContainer.classList.toggle('tinder_dislike', direction == Hammer.DIRECTION_LEFT);

                var xMulti = event.deltaX * 0.03;
                var yMulti = event.deltaY / 80;
                var rotate = xMulti * yMulti;

                event.target.style.transform = 'translate(' + event.deltaX + 'px, ' + event.deltaY + 'px) rotate(' + rotate + 'deg)';
            });

            hammertime.on('panend', function (event) {
                el.classList.remove('moving');
                tinderContainer.classList.remove('tinder_love');
                tinderContainer.classList.remove('tinder_nope');
                tinderContainer.classList.remove('tinder_like');
                tinderContainer.classList.remove('tinder_dislike');

                var moveOutWidth = document.body.clientWidth;
                let distanceValue = 40;
                let velocity = 0.1;
                
                var keep = (Math.abs(event.deltaX) < distanceValue
                    || Math.abs(event.velocityX) < velocity)
                    &&
                    (Math.abs(event.deltaY) < distanceValue
                    || Math.abs(event.velocityY) < velocity);

                event.target.classList.toggle('removed', !keep);

                if (keep) {
                    event.target.style.transform = '';
                } else {
                    var endX = Math.max(Math.abs(event.velocityX) * moveOutWidth, moveOutWidth);
                    var toX = event.deltaX > 0 ? endX : -endX;
                    var endY = Math.abs(event.velocityY) * moveOutWidth;
                    var toY = event.deltaY > 0 ? endY : -endY;
                    var xMulti = event.deltaX * 0.03;
                    var yMulti = event.deltaY / 80;
                    var rotate = xMulti * yMulti;

                    event.target.style.transform = 'translate(' + toX + 'px, ' + (toY + event.deltaY) + 'px) rotate(' + rotate + 'deg)';
                    swiped(event.direction);
                    initCards();
                }
            });
        });

        



        like.removeEventListener('click', likeListener);
        dislike.removeEventListener('click', dislikeListener);
        nope.removeEventListener('click', nopeListener);
        love.removeEventListener('click', loveListener);

        like.addEventListener('click', likeListener);
        dislike.addEventListener('click', dislikeListener);
        nope.addEventListener('click', nopeListener);
        love.addEventListener('click', loveListener);
    },
    
}
