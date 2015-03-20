/**
 * trap.js by scstauf@gmail.com
 */

if (typeof jQuery != 'undefined') {
    var Mouse = {
        down: {
            x: 0, y: 0
        },
        up: {
            x: 0, y: 0
        },
        isDown: false,
        locked: false,
        waitForSelect: false,
        reset: function () {
            this.down.x = 0;
            this.down.y = 0;
            this.up.x = 0;
            this.up.y = 0;
            this.isDown = false;
        },
        getCoordinates: function () {
            this.locked = false;
            return {
                x1: this.down.x, y1: this.down.y,
                x2: this.up.x, y2: this.up.y
            };
        }
    };

    $(document).ready(function () {
        $('body').mousedown(function (e) {
            if (Mouse.locked) return;
            if (e.which == 1 && window.getSelection().toString().length == 0) {
                if (!Mouse.isDown) {
                    Mouse.down.x = e.pageX;
                    Mouse.down.y = e.pageY;
                    Mouse.isDown = true;
                }
            }
        });

        $('body').mouseup(function (e) {
            if (Mouse.locked) return;
            if (e.which == 1) {
                if (Mouse.isDown) {
                    if ((Mouse.waitForSelect && window.getSelection().toString().length > 0) || !Mouse.waitForSelect) {
                        Mouse.up.x = e.pageX;
                        Mouse.up.y = e.pageY;
                        Mouse.isDown = false;
                        Mouse.locked = true;
                    } else {
                        Mouse.reset();
                    }
                }
            }
        });
    });
}

// a1f6ed979ad81d7b3ed45a5d64bd2c6f