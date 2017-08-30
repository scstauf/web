(function () {
    var override = {
        _overridden: [],
        log: function (message) {
            console.log('$override:', message);
        },
        error: function (err) {
            console.error('$override error:', err.message ? err.message : err);
        },
        override: function (name, callback) {
            /*
                exit if:
                    $ is not loaded,
                    method is already overridden,
                    name is not a string or is an empty string,
                    callback is not a function
            */
            if (!override.isjQueryLoaded()
                || override.isOverridden(name)
                || (typeof name !== 'string' || name.trim().length === 0)
                || typeof callback !== 'function') {
                return;
            }

            jQuery[name] = callback;
            override._overridden.push(name);
        },
        isOverridden: function (name) {
            return override._overridden.indexOf(name) > -1;
        },
        isjQueryLoaded: function () {
            return jQuery && typeof jQuery !== 'undefined';
        }
    };

    if (override.isjQueryLoaded
        && typeof jQuery.override === 'undefined'
        && typeof jQuery._overrides === 'undefined') {
        jQuery._overrides = override;
        jQuery.override = jQuery._overrides.override;
    }

    return override;
})();

/*
* Example of usage:
*/

$.override('parseJSON', function (data) {
    if (typeof data !== 'string' || !data) {
        return null;
    }

    if (window.JSON && JSON.parse) {
        return JSON.parse(data.trim() + '');
    }

    return null;
});
