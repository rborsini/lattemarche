var DropdownItem = /** @class */ (function () {
    function DropdownItem(value, text) {
        this.Value = "";
        this.Text = "";
        this.Value = value;
        this.Text = text;
    }
    return DropdownItem;
}());
export { DropdownItem };
var Dropdown = /** @class */ (function () {
    function Dropdown() {
        this.SelectedValues = [];
        this.Items = [];
    }
    return Dropdown;
}());
export { Dropdown };
