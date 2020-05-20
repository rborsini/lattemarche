import { Widget } from '../../widget.model';

export class TableWidgetModel extends Widget {

    public Rows: RowWidgetModel[] = [];

}

export class RowWidgetModel  {

    public Id: string = "";
    public Code: string = "";
    public Status: string = "";
    public CustomerId: number = 0;
    public Customer_FullBusinessName: string = "";
    public BusinessSublineId: number = 0;
    public BusinessSubline_Description: string = "";

    public HeadQuarterId: number = 0;
    public HeadQuarter_Description: string = "";

    public Margin: number = 0;
    public MarginPerc: number = 0;
    public Cost: number = 0;
    public Revenue: number = 0;

}


