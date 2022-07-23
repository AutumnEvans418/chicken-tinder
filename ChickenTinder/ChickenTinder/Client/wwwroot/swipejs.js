var dotRef = null;
window.Swipe = {
    start: (ref) => {
        dotRef = ref;
        var tinderContainer = document.querySelector('.tinder');
        var allCards = document.querySelectorAll('.tinder--card');
        var nope = document.getElementById('nope');
        var love = document.getElementById('love');
        var like = document.getElementById('like');
        var dislike = document.getElementById('dislike');
        function initCards(card, index) {
            var newCards = document.querySelectorAll('.tinder--card:not(.removed)');

            newCards.forEach(function (card, index) {
                card.style.zIndex = allCards.length - index;
                card.style.transform = 'scale(' + (20 - index) / 20 + ') translateY(-' + 30 * index + 'px)';
                card.style.opacity = (10 - index) / 10;
            });

            tinderContainer.classList.add('loaded');
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
                var keep = Math.abs(event.deltaX) < 80 || Math.abs(event.velocityX) < 0.5;

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
                    this.swiped(event.direction);
                    initCards();
                }
            });
        });

        function createButtonListener(love) {
            return function (event) {
                var cards = document.querySelectorAll('.tinder--card:not(.removed)');
                var moveOutWidth = document.body.clientWidth * 1.5;

                if (!cards.length) return false;

                var card = cards[0];

                card.classList.add('removed');

                if (love == "right") {
                    card.style.transform = 'translate(' + moveOutWidth + 'px, -100px) rotate(-30deg)';
                } else if (love == "left") {
                    card.style.transform = 'translate(-' + moveOutWidth + 'px, -100px) rotate(30deg)';
                } else if (love == "up") {
                    card.style.transform = 'translate(-100px, -' + moveOutWidth + 'px) rotate(10deg)';
                } else if (love == "down") {
                    card.style.transform = 'translate(-100px, ' + moveOutWidth + 'px) rotate(-10deg)';
                }

                initCards();

                event.preventDefault();
            };
        }

        var nopeListener = createButtonListener("down");
        var loveListener = createButtonListener("up");
        var likeListener = createButtonListener('right');
        var dislikeListener = createButtonListener('left');

        like.addEventListener('click', likeListener);
        dislike.addEventListener('click', dislikeListener);
        nope.addEventListener('click', nopeListener);
        love.addEventListener('click', loveListener);
    },
    swiped(direction) {
        dotRef.invokeMethodAsync("Swipe", direction);
    }
}
