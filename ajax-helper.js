/**
 * scottyeatscode <scstauf@gmail.com>
 *
 * jQuery based ajax helper 
 */

Function.prototype.method = function (name, func) {
    this.prototype[name] = func;
    return this;
};

var _App = function () {
    this._errorHandler = null;
    this._isjQueryLoaded = function () {
        var jQueryLoaded = typeof jQuery !== 'undefined';
        if (!jQueryLoaded) 
            console.error('jQuery is not loaded');
        return jQueryLoaded;
    }

    this._isjQueryLoaded();
};

var AjaxHelper = {
    completeAjaxRequestConfiguration: function (requestConfiguration) {
        if (!requestConfiguration) return;

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

                if (typeof this._errorHandler !== 'undefined')
                    this._errorHandler(fullErrorMessage);
            };
        }
        
        return requestConfiguration;
    }
};

_App.method('ajax', function (requestConfiguration) {
    if (!this._isjQueryLoaded()) return; 
    $.ajax(AjaxHelper.completeAjaxRequestConfiguration(requestConfiguration));
});

_App.method('post', function (requestConfiguration) {
    if (!this._isjQueryLoaded()) return;
    $.post(AjaxHelper.completeAjaxRequestConfiguration(requestConfiguration));
});

_App.method('get', function (requestConfiguration) {
    if (!this._isjQueryLoaded()) return;
    $.get(AjaxHelper.completeAjaxRequestConfiguration(requestConfiguration));
});

_App.method('setErrorHandler', function (errorHandler) {
    this._errorHandler = errorHandler;
});
