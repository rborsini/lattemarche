export class DropdownItem {

    public Value: any;
    public Text: string = "";

    constructor(value: any, text: string) {
        this.Value = value;
        this.Text = text;
    }

}

export class Dropdown {

    public SelectedValues: string[] = [];
    public Items: DropdownItem[] = [];

}