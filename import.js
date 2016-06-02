/* 
    ImportJS by scstauf
 
    Bare-bones script imports
 
    usage:
    
        ImportJS.importScriptBundle(scriptPaths[, success, failure]);

        ImportJS.importScript(scriptPath[, success, failure]);
 
    examples:
 
        // import multiple scripts at once, success and failure callbacks are optional
        ImportJS.importScriptBundle(
            [
                '/js/jquery.min.js', 
                '/js/interp.js'
            ], 
            function() { 
                console.log('yay') 
            }
        );

        // import a single script, success and failure callbacks are optional
        ImportJS.importScript('/js/jquery.min.js');
        
        // failure function contains exception message
        ImportJS.importScript('/js/not.a.javascript.file', null, function(e) {
            console.log('An error occurred: ' + e);
        });
 */

var ImportJS = {
    importScript: function (scriptPath, success, failure) {
        try {
            if (typeof scriptPath !== 'string' || scriptPath.length === 0 || !scriptPath.endsWith('.js')) {
                throw 'ImportJS could not import invalid script: ' + scriptPath;
            }

            var script = document.createElement('script');
            script.src = scriptPath;
            script.type = 'text/javascript';

            if (success && typeof success === 'function') {
                script.onreadystatechange = success;
                script.onload = success;
            }

            document.head.appendChild(script);
        }
        catch (e) {
            if (failure && typeof failure === 'function') {
                failure(e);
            }

            throw e;
        }
    },
    importScriptBundle: function (scriptPaths, success, failure) {
        try {
            if (typeof scriptPaths !== 'object' || !Array.isArray(scriptPaths)) {
                throw 'ImportJS could not import invalid script bundle.';
            }

            for (var i = 0; i < scriptPaths.length; i++) {
                this.importScript(scriptPaths[i], (i === scriptPaths.length - 1 ? success : null));
            }
        }
        catch (e) {
            if (failure && typeof failure === 'function') {
                failure(e);
            }
        }
    }
};
