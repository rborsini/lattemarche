import { Widget } from './widget.model';

export class Dashboard {
    
    public Id: number = 0;
    public IsDefault: boolean = false;
    public Title: string = "";    
    public Widgets: Widget[] = [];

}