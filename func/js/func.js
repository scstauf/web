var Func = {
	isFunc: function (f) {
		return typeof f === 'function';
	},
	analyzeSignature: function (f) {
		var isNative = false,
			sig = '';
			
		sig = f.toString().split('\n')[0];
		isNative = sig.indexOf('[native code]') > -1;
		sig = sig
			.substr(0, sig.indexOf('{'))
			.replace('function', '')
			.trim();			
		
		return {
			sig: sig,
			isNative: isNative,
			isEmpty: (sig.length === 0 || sig[0] === '(')
		};
	},
	fixTabs: function (def) {
		var tokens = def.split('\n'),
			token = '',
			length = tokens.length,
			lastLine = tokens[length - 1],
			termFound = (lastLine.trim() === '}'),
			tabCount = lastLine.split('\t').length - 1,
			wsCount = lastLine.split(' ').length - 1,
			useTabs = tabCount > 0 && wsCount === 0;
			
		if (!termFound) {
			return def;
		}
		
		for (var i in tokens) {
			token = tokens[i];
			
			if (token.length > 0 && token[0] === (useTabs ? '\t' : ' ')) {
				tokens[i] = tokens[i].substr(useTabs ? tabCount : wsCount);
			}
		}
		
		return tokens.join('\n');
	},
	getSignatures: function () {
		var nativeFunc = [],
			nonNativeFunc = [],
			analyzedSigObj = {};
			
		for (var i in window) {
			if (!this.isFunc(window[i])) 
				continue;
			
			analyzedSigObj = this.analyzeSignature(window[i]);
			
			if (analyzedSigObj.isEmpty)
				continue;
			
			if (analyzedSigObj.isNative)
				nativeFunc.push(analyzedSigObj.sig);
			else 
				nonNativeFunc.push(analyzedSigObj.sig);
		}
		
		return {
			nativeFunc: nativeFunc,
			nonNativeFunc: nonNativeFunc
		};
	},
	peekDefinition: function (name, fixTabs) {
		var def = '',
			analyzedSigObj = {};
		
		for (var i in window) {
			if (!this.isFunc(window[i])) 
				continue;
			
			analyzedSigObj = this.analyzeSignature(window[i]);
			
			if (analyzedSigObj.sig.indexOf(name) > -1) {
				def = window[i].toString();
				break;
			}
		}
		
		return fixTabs ? this.fixTabs(def) : def;
	}
};
