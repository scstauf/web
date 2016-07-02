var ShoppingCart = {};

(function () {
    this.items = [];
    this.cookie = '_sc_json';
    
    this.add = function (item, desc, qty, price) {
        if (!this.contains(item) && qty > 0) {
            this.items.push({ 
                item: item,
                desc: desc,
                qty: qty,
                price: price
            });
        
            this.store();
        }
    };

    this.update = function (item, qty) {
        var pos = this.indexOf(item);
        
        if (pos > -1) {
            if (qty < 1) 
                this.delete(item);
            else
                this.items[pos].qty = qty;
        }
        
        this.store();
    };
    
    this.delete = function (item) {
        var pos = this.indexOf(item);

        if (pos > -1)
            this.items.splice(pos, 1);
        
        this.store();
    };
    
    this.store = function () {
        Cookies.set(this.cookie, JSON.stringify(this.items), true, true);
    };
    
    this.restore = function () {
        try {
            var json = Cookies.get(this.cookie);

            if (json && json.length > 0)
                this.items = JSON.parse(json);
        }
        catch (err) {
            console.error('Could not restore cart from json.\r\nAre cookies enabled?');
        }
    };
    
    this.canRestore = function () {
        return Cookies.get(this.cookie).length > 0;
    };
    
    this.clear = function () {
        this.items = [];
        this.store();
    };
    
    this.contains = function (item) {
        return this.indexOf(item) > -1;
    };
    
    this.indexOf = function (item) {
        var pos = -1;
        for (var i = 0; i < this.items.length; i++) {
            if (this.items[i].item === item) {
                pos = i;
                break;
            }
        }
        return pos;
    };
    
}).apply(ShoppingCart);
