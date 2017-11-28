/**
 * @author Scott Christopher Stauffer <scstauf at gmail dot com>
 */

(function () {
	String.prototype.superSplit = function (splitsArray) {
		var str = this.valueOf();
		var values = [], splits = [];
		var i = 0, j = 0;
		var value = '';

		if (typeof str !== 'string' || str.length === 0) return '';
		
		if (Array.isArray(splitsArray)) splits = splitsArray;
		else if (typeof splitsArray === 'string') splits.push(splitsArray);
		else return '';
		
		for (i = 0; i < str.length; i++) {
			value += str[i];
			
			for (j = 0; j < splits.length; j++) {
				if (value.indexOf(splits[j]) > -1) {
					value = value.replace(splits[j], '');
					
					if (value.length > 0) {
						values.push(value);
					}
					
					value = '';
					break;
				}
			}
		}
		
		if (value.length > 0) values.push(value);
		
		return values;
	}
})();

// example usage
/*

console.log(
	'Hello<br>World<br />!<br/>'.superSplit(['<br>', '<br />', '<br/>'])
);
// -> ["Hello", "World", "!"]

 */
