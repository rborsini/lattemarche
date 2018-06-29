export class Pagina {
    public Title: string = "";
    public Enabled: boolean = false;
    public Items: ViewItem[] = [];
}

export class ViewItem {
    public Title: string = "";
    public Enabled: boolean = false;
    public DisplayName: string = "";
}