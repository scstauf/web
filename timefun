function parseJSONDateTime(jsonDate, useDay) {
	var month = 1, day = 1, year = 1900,
		h = 0, m = 0, s = 0, ms = 0, ampm = 'AM',
		timeStamp = '{0} {1}.{2} {3}';

	try {
		var begin = jsonDate.indexOf('(');
		var end = jsonDate.indexOf(')');

		var date = new Date(parseInt(jsonDate.substr(begin + 1, end - begin - 1)));
		month = '' + (date.getMonth() + 1),
		day = '' + (useDay ? date.getDate() : 1),
		year = date.getFullYear(),
		h = date.getHours(),
		m = date.getMinutes(),
		s = date.getSeconds(),
		ms = '' + date.getMilliseconds();

		if (h > 12) {
			h -= 12;
			ampm = 'PM';
		}

		h = '' + h;
		m = '' + m;
		s = '' + s;

		var msPad = '000';
		var timePad = '00';

		month = timePad.substring(0, timePad.length - month.length) + month;
		day = timePad.substring(0, timePad.length - day.length) + day;
		h = timePad.substring(0, timePad.length - h.length) + h;
		m = timePad.substring(0, timePad.length - m.length) + m;
		s = timePad.substring(0, timePad.length - s.length) + s;
		ms = msPad.substring(0, msPad.length - ms.length) + ms;
	}
	catch (err) {
		return null;
	}

	timeStamp = timeStamp.interp(
		[month, day, year].join('/'),
		[h, m, s].join(':'),
		ms,
		ampm
	);

	return timeStamp;
}

function calcTime(timeInSeconds) {
	var h = 0;
	var m = Math.floor(timeInSeconds / 60);
	var s = timeInSeconds % 60;
	var ret = '00:00:00';
	var timePad = '00';

	if (m >= 60) {
		h = Math.floor(m / 60);
		m = m % 60;
	}

	h = '' + h;
	m = '' + m;
	s = '' + s;

	if (!isNaN(h) && !isNaN(m) && !isNaN(s)) {
		h = timePad.substring(0, timePad.length - h.length) + h;
		m = timePad.substring(0, timePad.length - m.length) + m;
		s = timePad.substring(0, timePad.length - s.length) + s;

		ret = '{0}:{1}:{2}'.interp(
			h,
			m,
			s
		);
	}

	return ret;
}

function getQuarter(month) {
	return Math.floor(((month - 1) / 12) / 0.25) + 1;
}

function isValidDate(d) {
	return !isNaN(new Date(Date.parse(d)).getDate());
}
