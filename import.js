/* 
    ImportJS by scstauf
 
    Bare-bones script imports
 
    usage:
    
        ImportJS.importScriptBundle(scriptPaths[, callback]);

        ImportJS.importScript(scriptPath[, callback]);
 
    examples:
 
        // import multiple scripts at once, callback is optional
        ImportJS.importScriptBundle(
            [
                '/js/jquery.min.js', 
                '/js/interp.js'
            ], 
            function() { 
                console.log('yay') 
            }
        );

        // import a single script, callback is optional
        ImportJS.importScript('/js/jquery.min.js');
 */

var ImportJS = {
    importScript: function (scriptPath, callback) {
        if (typeof scriptPath !== 'string' || scriptPath.length === 0 || scriptPath.indexOf('.js') === -1) {
            throw 'ImportJS could not import invalid script: ' + scriptPath;
        }

        try {
            var script = document.createElement('script');
            script.src = scriptPath;
            script.type = 'text/javascript';

            if (callback && typeof callback === 'function') {
                script.onreadystatechange = callback;
                script.onload = callback;
            }

            document.head.appendChild(script);
        }
        catch (err) {
            console.error(
                'ImportJS could not import script: ' + scriptPath +
                '\r\nError: ' + err.message +
                '\r\nStack: ' + err.stack
            );

            throw err;
        }
    },
    importScriptBundle: function (scriptPaths, callback) {
        if (typeof scriptPaths !== 'object' || !Array.isArray(scriptPaths)) {
            throw 'ImportJS could not import invalid script bundle.';
        }
        
        for (var i = 0; i < scriptPaths.length; i++) {
            this.importScript(scriptPaths[i], (i === scriptPaths.length - 1 ? callback : null));
        }
    }
};
