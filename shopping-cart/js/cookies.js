var Cookies = {};

(function () {
    this.clear = function (name) {
        this.create(name, '');
    };
    
    this.clearAll = function () {
        var cookies = document.cookie.split(';');
        for (var i = 0; i < cookies.length; i++) {
            this.clear(cookies[i].split('=')[0]);
        }
    };
        
    this.create = function (name, value, expiry, makeGlobal) {
        var expires = '',
            global = '',
            date = new Date();

        if (expiry) {
            date.setTime(date.getTime() + (expiry * 24 * 60 * 60 * 1000));
            expires = '; expires=' + date.toGMTString();
        }
        
        if (makeGlobal) {
            global = '; path=/';
        }
        
        document.cookie = name + '=' + value + expires + global;
    };
    
    this.get = function (name) {
        var value = '',
            cookie = document.cookie,
            cookies = [],
            currentCookie = null;
        
        if (-1 < cookie.indexOf(name)) {
            cookies = cookie.split(';');
            
            for (var i = 0; i < cookies.length; i++) {
                currentCookie = cookies[i];
                
                if (-1 < currentCookie.indexOf(name)) {
                    value = currentCookie.split('=')[1];
                    break;
                }
            }
        }
        
        return value;
    };
    
    this.set = function (name, value, expiry, makeGlobal) {
        this.clear(name);
        this.create(name, value, expiry, makeGlobal);
    };
    
}).apply(Cookies);
