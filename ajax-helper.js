/**
 * scottyeatscode <scstauf@gmail.com>
 *
 * jQuery based ajax helper 
 */

Function.prototype.method = function (name, func) {
    this.prototype[name] = func;
    return this;
};

var AjaxHelper = {
    completeAjaxRequestConfiguration: function (app, requestConfiguration) {
        if (!requestConfiguration) {
            return;
        }

        if (!requestConfiguration.hasOwnProperty('error')) {
            requestConfiguration['error'] = function (xhr, status, error) {
                var errorMessage = 'Unspecified Error',
                    fullErrorMessage = '';

                try {
                    errorMessage = xhr.responseJSON.errorMessage;
                }
                catch (err) { }

                fullErrorMessage = 'An error occurred in Ajax request for ' + requestConfiguration.url + '.\r\nResponse was: ' + errorMessage

                console.error(fullErrorMessage);

                if (typeof app._errorHandler !== 'undefined') {
                    app._errorHandler(fullErrorMessage);
                }
            };
        }

        return requestConfiguration;
    }
};

var App = function () {
    this._errorHandler = null;
    this._isjQueryLoaded = function () {
        var jQueryLoaded = typeof jQuery !== 'undefined';
        if (!jQueryLoaded) {
            console.error('jQuery is not loaded');
        }
        return jQueryLoaded;
    }

    this._isjQueryLoaded();
};

// prebuild the AJAX methods
App.method('ajax', function (requestConfiguration) {
    if (!this._isjQueryLoaded()) return;
    return $.ajax(AjaxHelper.completeAjaxRequestConfiguration(this, requestConfiguration));
});

App.method('post', function (requestConfiguration) {
    if (!this._isjQueryLoaded()) return;
    return $.post(AjaxHelper.completeAjaxRequestConfiguration(this, requestConfiguration));
});

App.method('get', function (requestConfiguration) {
    if (!this._isjQueryLoaded()) return;
    return $.get(AjaxHelper.completeAjaxRequestConfiguration(this, requestConfiguration));
});

// use this to set the error handler
App.method('setErrorHandler', function (errorHandler) {   
    this._errorHandler = errorHandler;
});
