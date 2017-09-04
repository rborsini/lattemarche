function CompareAsc(text1, text2) {
    text1 = text1 ? text1 : '-';
    text2 = text2 ? text2 : '-';
    var result = text1.localeCompare(text2);
    return result;
}

function CompareDesc(text1, text2) {
    return CompareAsc(text1, text2) * -1;
}

function CompareNumAsc(num1, num2) {
    if (num1 == num2) {
        return 0;
    } else {
        return num1 < num2 ? -1 : 1;
    }
}

function CompareNumDesc(num1, num2) {
    return CompareNumAsc(num1, num2) * -1;
}

function ConvertNum(text) {
    return Number(text);
}

function ConverDate(date) {
    return date ? date.substring(8, 12) + '-' + date.substring(3, 5) + '-' + date.substring(0, 2) : '-';
}

ko.observableArray.fn.sortAsc = function (prop) {
    this.sort(function (obj1, obj2) {
        return CompareAsc(obj1[prop](), obj2[prop]());
    });
};

ko.observableArray.fn.sortDesc = function (prop) {
    this.sort(function (obj1, obj2) {
        return CompareDesc(obj1[prop](), obj2[prop]());
    });
};

ko.observableArray.fn.sortDateAsc = function (prop) {
    this.sort(function (obj1, obj2) {
       return CompareAsc(ConverDate(obj1[prop]()), ConverDate(obj2[prop]()));
    });
};

ko.observableArray.fn.sortDateDesc = function (prop) {
    this.sort(function (obj1, obj2) {
        return CompareDesc(ConverDate(obj1[prop]()), ConverDate(obj2[prop]()));
    });
};

ko.observableArray.fn.sortNumberAsc = function (prop) {
    this.sort(function (obj1, obj2) {
        return CompareNumAsc(ConvertNum(obj1[prop]()), ConvertNum(obj2[prop]()));
    });
};

ko.observableArray.fn.sortNumberDesc = function (prop) {
    this.sort(function (obj1, obj2) {
        return CompareNumDesc(ConvertNum(obj1[prop]()), ConvertNum(obj2[prop]()));
    });
};

ko.bindingHandlers['prop'] = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()) || {};
        for (var propName in value) {
            if (typeof propName == "string") {
                var propValue = ko.utils.unwrapObservable(value[propName]);

                element[propName] = propValue;
            }
        }
    }
};

