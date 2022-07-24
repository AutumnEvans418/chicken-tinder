
window.Fireworks = {
    launch: () => {
        launch();
    },
    confetti: () => {
        // Globals
        var random = Math.random
            , cos = Math.cos
            , sin = Math.sin
            , PI = Math.PI
            , PI2 = PI * 2
            , timer = undefined
            , frame = undefined
            , confetti = [];

        var particles = 10
            , spread = 40
            , sizeMin = 3
            , sizeMax = 12 - sizeMin
            , eccentricity = 10
            , deviation = 100
            , dxThetaMin = -.1
            , dxThetaMax = -dxThetaMin - dxThetaMin
            , dyMin = .13
            , dyMax = .18
            , dThetaMin = .4
            , dThetaMax = .7 - dThetaMin;

        var colorThemes = [
            function () {
                return color(200 * random() | 0, 200 * random() | 0, 200 * random() | 0);
            }, function () {
                var black = 200 * random() | 0; return color(200, black, black);
            }, function () {
                var black = 200 * random() | 0; return color(black, 200, black);
            }, function () {
                var black = 200 * random() | 0; return color(black, black, 200);
            }, function () {
                return color(200, 100, 200 * random() | 0);
            }, function () {
                return color(200 * random() | 0, 200, 200);
            }, function () {
                var black = 256 * random() | 0; return color(black, black, black);
            }, function () {
                return colorThemes[random() < .5 ? 1 : 2]();
            }, function () {
                return colorThemes[random() < .5 ? 3 : 5]();
            }, function () {
                return colorThemes[random() < .5 ? 2 : 4]();
            }
        ];
        function color(r, g, b) {
            return 'rgb(' + r + ',' + g + ',' + b + ')';
        }

        // Cosine interpolation
        function interpolation(a, b, t) {
            return (1 - cos(PI * t)) / 2 * (b - a) + a;
        }

        // Create a 1D Maximal Poisson Disc over [0, 1]
        var radius = 1 / eccentricity, radius2 = radius + radius;
        function createPoisson() {
            // domain is the set of points which are still available to pick from
            // D = union{ [d_i, d_i+1] | i is even }
            var domain = [radius, 1 - radius], measure = 1 - radius2, spline = [0, 1];
            while (measure) {
                var dart = measure * random(), i, l, interval, a, b, c, d;

                // Find where dart lies
                for (i = 0, l = domain.length, measure = 0; i < l; i += 2) {
                    a = domain[i], b = domain[i + 1], interval = b - a;
                    if (dart < measure + interval) {
                        spline.push(dart += a - measure);
                        break;
                    }
                    measure += interval;
                }
                c = dart - radius, d = dart + radius;

                // Update the domain
                for (i = domain.length - 1; i > 0; i -= 2) {
                    l = i - 1, a = domain[l], b = domain[i];
                    // c---d          c---d  Do nothing
                    //   c-----d  c-----d    Move interior
                    //   c--------------d    Delete interval
                    //         c--d          Split interval
                    //       a------b
                    if (a >= c && a < d)
                        if (b > d) domain[l] = d; // Move interior (Left case)
                        else domain.splice(l, 2); // Delete interval
                    else if (a < c && b > c)
                        if (b <= d) domain[i] = c; // Move interior (Right case)
                        else domain.splice(i, 0, c, d); // Split interval
                }

                // Re-measure the domain
                for (i = 0, l = domain.length, measure = 0; i < l; i += 2)
                    measure += domain[i + 1] - domain[i];
            }

            return spline.sort();
        }

        // Create the overarching container
        var container = document.createElement('div');
        container.style.position = 'fixed';
        container.style.top = '0';
        container.style.left = '0';
        container.style.width = '100%';
        container.style.height = '0';
        container.style.overflow = 'visible';
        container.style.zIndex = '9999';

        // Confetto constructor
        function Confetto(theme) {
            this.frame = 0;
            this.outer = document.createElement('div');
            this.inner = document.createElement('div');
            this.outer.appendChild(this.inner);

            var outerStyle = this.outer.style, innerStyle = this.inner.style;
            outerStyle.position = 'absolute';
            outerStyle.width = (sizeMin + sizeMax * random()) + 'px';
            outerStyle.height = (sizeMin + sizeMax * random()) + 'px';
            innerStyle.width = '100%';
            innerStyle.height = '100%';
            innerStyle.backgroundColor = theme();

            outerStyle.perspective = '50px';
            outerStyle.transform = 'rotate(' + (360 * random()) + 'deg)';
            this.axis = 'rotate3D(' +
                cos(360 * random()) + ',' +
                cos(360 * random()) + ',0,';
            this.theta = 360 * random();
            this.dTheta = dThetaMin + dThetaMax * random();
            innerStyle.transform = this.axis + this.theta + 'deg)';

            this.x = window.innerWidth * random();
            this.y = -deviation;
            this.dx = sin(dxThetaMin + dxThetaMax * random());
            this.dy = dyMin + dyMax * random();
            outerStyle.left = this.x + 'px';
            outerStyle.top = this.y + 'px';

            // Create the periodic spline
            this.splineX = createPoisson();
            this.splineY = [];
            for (var i = 1, l = this.splineX.length - 1; i < l; ++i)
                this.splineY[i] = deviation * random();
            this.splineY[0] = this.splineY[l] = deviation * random();

            this.update = function (height, delta) {
                this.frame += delta;
                this.x += this.dx * delta;
                this.y += this.dy * delta;
                this.theta += this.dTheta * delta;

                // Compute spline and convert to polar
                var phi = this.frame % 7777 / 7777, i = 0, j = 1;
                while (phi >= this.splineX[j]) i = j++;
                var rho = interpolation(
                    this.splineY[i],
                    this.splineY[j],
                    (phi - this.splineX[i]) / (this.splineX[j] - this.splineX[i])
                );
                phi *= PI2;

                outerStyle.left = this.x + rho * cos(phi) + 'px';
                outerStyle.top = this.y + rho * sin(phi) + 'px';
                innerStyle.transform = this.axis + this.theta + 'deg)';
                return this.y > height + deviation;
            };
        }

        function poof() {
            if (!frame) {
                // Append the container
                document.body.appendChild(container);

                // Add confetti
                var theme = colorThemes[0]
                    , count = 0;
                (function addConfetto() {
                    var confetto = new Confetto(theme);
                    confetti.push(confetto);
                    container.appendChild(confetto.outer);
                    timer = setTimeout(addConfetto, spread * random());
                })(0);

                // Start the loop
                var prev = undefined;
                requestAnimationFrame(function loop(timestamp) {
                    var delta = prev ? timestamp - prev : 0;
                    prev = timestamp;
                    var height = window.innerHeight;

                    for (var i = confetti.length - 1; i >= 0; --i) {
                        if (confetti[i].update(height, delta)) {
                            container.removeChild(confetti[i].outer);
                            confetti.splice(i, 1);
                        }
                    }

                    if (timer || confetti.length)
                        return frame = requestAnimationFrame(loop);

                    // Cleanup
                    document.body.removeChild(container);
                    frame = undefined;
                });
            }
        }

        poof();
    }
}

function launch() {


    // when animating on canvas, it is best to use requestAnimationFrame instead of setTimeout or setInterval
    // not supported in all browsers though and sometimes needs a prefix, so we need a shim
    window.requestAnimFrame = (function () {
        return window.requestAnimationFrame ||
            window.webkitRequestAnimationFrame ||
            window.mozRequestAnimationFrame ||
            function (callback) {
                window.setTimeout(callback, 1000 / 60);
            };
    })();

    // now we will setup our basic variables for the demo
    var canvas = document.getElementById('canvas'),
        ctx = canvas.getContext('2d'),
        // full screen dimensions
        cw = window.innerWidth,
        ch = window.innerHeight,
        // firework collection
        fireworks = [],
        // particle collection
        particles = [],
        // starting hue
        hue = 120,
        // when launching fireworks with a click, too many get launched at once without a limiter, one launch per 5 loop ticks
        limiterTotal = 5,
        limiterTick = 0,
        // this will time the auto launches of fireworks, one launch per 80 loop ticks
        timerTotal = 80,
        timerTick = 0,
        mousedown = false,
        // mouse x coordinate,
        mx,
        // mouse y coordinate
        my;

    // set canvas dimensions
    canvas.width = cw;
    canvas.height = ch;

    // now we are going to setup our function placeholders for the entire demo

    // get a random number within a range
    function random(min, max) {
        return Math.random() * (max - min) + min;
    }

    // calculate the distance between two points
    function calculateDistance(p1x, p1y, p2x, p2y) {
        var xDistance = p1x - p2x,
            yDistance = p1y - p2y;
        return Math.sqrt(Math.pow(xDistance, 2) + Math.pow(yDistance, 2));
    }

    // create firework
    function Firework(sx, sy, tx, ty) {
        // actual coordinates
        this.x = sx;
        this.y = sy;
        // starting coordinates
        this.sx = sx;
        this.sy = sy;
        // target coordinates
        this.tx = tx;
        this.ty = ty;
        // distance from starting point to target
        this.distanceToTarget = calculateDistance(sx, sy, tx, ty);
        this.distanceTraveled = 0;
        // track the past coordinates of each firework to create a trail effect, increase the coordinate count to create more prominent trails
        this.coordinates = [];
        this.coordinateCount = 3;
        // populate initial coordinate collection with the current coordinates
        while (this.coordinateCount--) {
            this.coordinates.push([this.x, this.y]);
        }
        this.angle = Math.atan2(ty - sy, tx - sx);
        this.speed = 2;
        this.acceleration = 1.05;
        this.brightness = random(50, 70);
        // circle target indicator radius
        this.targetRadius = 1;
    }

    // update firework
    Firework.prototype.update = function (index) {
        // remove last item in coordinates array
        this.coordinates.pop();
        // add current coordinates to the start of the array
        this.coordinates.unshift([this.x, this.y]);

        // cycle the circle target indicator radius
        if (this.targetRadius < 8) {
            this.targetRadius += 0.3;
        } else {
            this.targetRadius = 1;
        }

        // speed up the firework
        this.speed *= this.acceleration;

        // get the current velocities based on angle and speed
        var vx = Math.cos(this.angle) * this.speed,
            vy = Math.sin(this.angle) * this.speed;
        // how far will the firework have traveled with velocities applied?
        this.distanceTraveled = calculateDistance(this.sx, this.sy, this.x + vx, this.y + vy);

        // if the distance traveled, including velocities, is greater than the initial distance to the target, then the target has been reached
        if (this.distanceTraveled >= this.distanceToTarget) {
            createParticles(this.tx, this.ty);
            // remove the firework, use the index passed into the update function to determine which to remove
            fireworks.splice(index, 1);
        } else {
            // target not reached, keep traveling
            this.x += vx;
            this.y += vy;
        }
    }

    // draw firework
    Firework.prototype.draw = function () {
        ctx.beginPath();
        // move to the last tracked coordinate in the set, then draw a line to the current x and y
        ctx.moveTo(this.coordinates[this.coordinates.length - 1][0], this.coordinates[this.coordinates.length - 1][1]);
        ctx.lineTo(this.x, this.y);
        ctx.strokeStyle = 'hsl(' + hue + ', 100%, ' + this.brightness + '%)';
        ctx.stroke();

        ctx.beginPath();
        // draw the target for this firework with a pulsing circle
        ctx.arc(this.tx, this.ty, this.targetRadius, 0, Math.PI * 2);
        ctx.stroke();
    }

    // create particle
    function Particle(x, y) {
        this.x = x;
        this.y = y;
        // track the past coordinates of each particle to create a trail effect, increase the coordinate count to create more prominent trails
        this.coordinates = [];
        this.coordinateCount = 5;
        while (this.coordinateCount--) {
            this.coordinates.push([this.x, this.y]);
        }
        // set a random angle in all possible directions, in radians
        this.angle = random(0, Math.PI * 2);
        this.speed = random(1, 10);
        // friction will slow the particle down
        this.friction = 0.95;
        // gravity will be applied and pull the particle down
        this.gravity = 1;
        // set the hue to a random number +-50 of the overall hue variable
        this.hue = random(hue - 50, hue + 50);
        this.brightness = random(50, 80);
        this.alpha = 1;
        // set how fast the particle fades out
        this.decay = random(0.015, 0.03);
    }

    // update particle
    Particle.prototype.update = function (index) {
        // remove last item in coordinates array
        this.coordinates.pop();
        // add current coordinates to the start of the array
        this.coordinates.unshift([this.x, this.y]);
        // slow down the particle
        this.speed *= this.friction;
        // apply velocity
        this.x += Math.cos(this.angle) * this.speed;
        this.y += Math.sin(this.angle) * this.speed + this.gravity;
        // fade out the particle
        this.alpha -= this.decay;

        // remove the particle once the alpha is low enough, based on the passed in index
        if (this.alpha <= this.decay) {
            particles.splice(index, 1);
        }
    }

    // draw particle
    Particle.prototype.draw = function () {
        ctx.beginPath();
        // move to the last tracked coordinates in the set, then draw a line to the current x and y
        ctx.moveTo(this.coordinates[this.coordinates.length - 1][0], this.coordinates[this.coordinates.length - 1][1]);
        ctx.lineTo(this.x, this.y);
        ctx.strokeStyle = 'hsla(' + this.hue + ', 100%, ' + this.brightness + '%, ' + this.alpha + ')';
        ctx.stroke();
    }

    // create particle group/explosion
    function createParticles(x, y) {
        // increase the particle count for a bigger explosion, beware of the canvas performance hit with the increased particles though
        var particleCount = 30;
        while (particleCount--) {
            particles.push(new Particle(x, y));
        }
    }

    // main demo loop
    function loop() {
        // this function will run endlessly with requestAnimationFrame
        requestAnimFrame(loop);

        // increase the hue to get different colored fireworks over time
        //hue += 0.5;

        // create random color
        hue = random(0, 360);

        // normally, clearRect() would be used to clear the canvas
        // we want to create a trailing effect though
        // setting the composite operation to destination-out will allow us to clear the canvas at a specific opacity, rather than wiping it entirely
        ctx.globalCompositeOperation = 'destination-out';
        // decrease the alpha property to create more prominent trails
        ctx.fillStyle = 'rgba(0, 0, 0, 0.5)';
        ctx.fillRect(0, 0, cw, ch);
        // change the composite operation back to our main mode
        // lighter creates bright highlight points as the fireworks and particles overlap each other
        ctx.globalCompositeOperation = 'lighter';

        // loop over each firework, draw it, update it
        var i = fireworks.length;
        while (i--) {
            fireworks[i].draw();
            fireworks[i].update(i);
        }

        // loop over each particle, draw it, update it
        var i = particles.length;
        while (i--) {
            particles[i].draw();
            particles[i].update(i);
        }

        // launch fireworks automatically to random coordinates, when the mouse isn't down
        if (timerTick >= timerTotal) {
            if (!mousedown) {
                // start the firework at the bottom middle of the screen, then set the random target coordinates, the random y coordinates will be set within the range of the top half of the screen
                fireworks.push(new Firework(cw / 2, ch, random(0, cw), random(0, ch / 2)));
                timerTick = 0;
            }
        } else {
            timerTick++;
        }

        // limit the rate at which fireworks get launched when mouse is down
        if (limiterTick >= limiterTotal) {
            if (mousedown) {
                // start the firework at the bottom middle of the screen, then set the current mouse coordinates as the target
                fireworks.push(new Firework(cw / 2, ch, mx, my));
                limiterTick = 0;
            }
        } else {
            limiterTick++;
        }
    }

    // mouse event bindings
    // update the mouse coordinates on mousemove
    canvas.addEventListener('mousemove', function (e) {
        mx = e.pageX - canvas.offsetLeft;
        my = e.pageY - canvas.offsetTop;
    });

    // toggle mousedown state and prevent canvas from being selected
    canvas.addEventListener('mousedown', function (e) {
        e.preventDefault();
        mousedown = true;
    });

    canvas.addEventListener('mouseup', function (e) {
        e.preventDefault();
        mousedown = false;
    });

    // once the window loads, we are ready for some fireworks!
    window.onload = loop;

}