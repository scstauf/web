// just having fun

var DOM = {};

(function () {
    this.version = '0.0.1';

    this.ErrorType = {
        InvalidParameter: 'Invalid Parameter'
    };

    this.error = function (type, message, target) { throw new Error(type + ': ' + message + ': ' + target); };

    this.create = function (node, tag, prop) {
        var parent = null, child = null, i = null, o = null, p = null;

        if ((typeof node !== 'object' && typeof node !== 'string') ||
            (typeof node === 'object' && Array.isArray(node) && node.length > 1)) {
            this.error(this.ErrorType.InvalidParameter, 'node', node);
            return;
        }

        if (typeof tag !== 'string') {
            this.error(this.ErrorType.InvalidParameter, 'tag', tag);
            return;
        }

        if (typeof node === 'object' || typeof node === 'string') {
            if (typeof node === 'string') parent = this.get(node);
            else parent = node;

            parent = parent[0];
        }

        child = document.createElement(tag);

        if (prop && typeof prop === 'object') {
            if (Array.isArray(prop)) {
                for (i in prop) {
                    o = prop[i];
                    for (p in o) child.setAttribute(p, o[p]);
                }
            }
            else for (i in prop) child.setAttribute(i, prop[i]);
        }

        parent.appendChild(child);
    };

    this.delete = function (selector) {
        this.get(selector).forEach(function (el) {
            console.log(el.parentElement.removeChild(el));
        });
    };

    this.get = function (selector) { return document.querySelectorAll(selector); };

    this.ready = function (callback) { document.addEventListener('DOMContentLoaded', callback); }

}).apply(DOM);
