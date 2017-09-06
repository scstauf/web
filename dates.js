// add or subtract days
Date.prototype.addDay = function (day) {
    var now = new Date(this.valueOf());
    this.setDate(now.getDate() + day);
};

// add or subtract months
Date.prototype.addMonth = function (month) {
    var now = new Date(this.valueOf());
    this.setMonth(now.getMonth() + month);
};

// add or subtract years
Date.prototype.addYear = function (year) {
    var now = new Date(this.valueOf());
    this.setFullYear(now.getFullYear() + year);
};

// return true if leap year
Date.prototype.isLeapYear = function () {
    var year = new Date(this.valueOf()).getFullYear();
    return year % 4 === 0 && (year % 100 !== 0 || year % 400 === 0);
};

// return the quarter number
Date.prototype.getQuarter = function () {
    var month = new Date(this.valueOf()).getMonth() + 1;
    return Math.floor(((month - 1) / 12) / 0.25) + 1;
};
