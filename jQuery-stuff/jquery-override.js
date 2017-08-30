/*
$.override by scottyeatscode

This script adds an override() method to jQuery for
overriding built-in jQuery methods.

You can add an override by calling:

    $.override('name', function () { })

You can remove an override by calling:

    $.removeOverride('name')
*/

(function () {
    var override = {
        _overridden: [],
        log: function (message) {
            console.log('$override:', message);
        },
        error: function (err) {
            console.error('$override error:', err.message ? err.message : err);
        },
        override: function (name, method) {
            var originalMethod = null;
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
                || typeof method !== 'function') {
                return;
            }

            if (jQuery[name] && typeof jQuery[name] === 'function') {
                originalMethod = jQuery[name];
            }

            jQuery[name] = method;

            override._overridden.push({
                name: name,
                originalMethod: originalMethod,
                overrideMethod: method
            });
        },
        removeOverride: function (name) {
            var index = override.indexOfOverride(name),
                overridden = override._overridden;

            if (index < 0) {
                return;
            }

            jQuery[name] = overridden[index].originalMethod;
            override._overridden.splice(index, 1);
        },
        isOverridden: function (name) {
            return override.indexOfOverride(name) > -1;
        },
        isjQueryLoaded: function () {
            return jQuery && typeof jQuery !== 'undefined';
        },
        indexOfOverride: function (name) {
            var index = -1,
                overridden = override._overridden;

            if (!name || typeof name !== 'string' || name.trim().length === '') {
                return index;
            }

            for (var i = 0; i < overridden.length; i++) {
                if (name === overridden[i].name) {
                    index = i;
                    break;
                }
            }

            return index;
        }
    };

    if (override.isjQueryLoaded
        && typeof jQuery.override === 'undefined'
        && typeof jQuery._overrides === 'undefined'
        && typeof jQuery.removeOverride === 'undefined') {
        jQuery._overrides = override;
        jQuery.override = jQuery._overrides.override;
        jQuery.removeOverride = jQuery._overrides.removeOverride;
    }

    return override;
})();

/*
* Example code for adding and removing overrides
*/

// add an override
$.override('parseJSON', function (data) {
    if (typeof data !== 'string' || !data) {
        return null;
    }

    if (window.JSON && JSON.parse) {
        return JSON.parse(data.trim() + '');
    }

    return null;
});

// remove an override
//$.removeOverride('parseJSON');
