/* stopwatch.js
 * a really simple vanillajs stopwatch for benchmarking
 *
 * scstauf@gmail.com, 2017
 */

if (!window.StopWatch) {
    window.StopWatch = function () {
        var _start = 0;

        function _getTime() {
            return new Date(Date.now()).getTime();
        }

        this.start = function () {
            _start = _getTime();
        };

        this.stop = function () {
            return _getTime() - _start;
        };
    }
}
