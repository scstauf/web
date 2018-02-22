var _ = function (query) {
  // sets the context
	function _query(selector) {
		var isDocument = selector === document,
			n = !isDocument ? document.querySelectorAll(selector) : document, 
			res = [];
		
		if (n && typeof n !== 'undefined') {
			if (!isDocument && n.length && n.length === 1) 
				res = n[0];
			else
				res = n;
		}
		
		this.context = res;
		
		return this;
	}
	
  // registering event handlers
	_query.prototype.on = function (eventName, handler) {
		this.context.addEventListener(eventName, handler);
	}
	
  // append html string to the context
	_query.prototype.append = function (html) {
		this.context.innerHTML += html;
	}
	
  // for storing data in the context
	_query.prototype.data = function (key, value) {
		if (typeof this.context.data === 'undefined')
			this.context.data = {};
		
		if (typeof key !== 'undefined') {
			if (typeof value !== 'undefined') 
				this.context.data[key] = value;
			else
				return this.context.data[key];
		}
		
		return this.context.data;
	}
	
	return new _query(query);
};
