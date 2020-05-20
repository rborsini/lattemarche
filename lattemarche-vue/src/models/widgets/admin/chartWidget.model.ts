import { Widget } from '../../widget.model';

export class ChartWidgetModel extends Widget {

    public Months: MonthWidgetModel[] = [];

}

export class MonthWidgetModel  {

    public Year: number = 0;
    public Month: number = 0;

    public Margin_Euro: number = 0;
    public Margin_Perc: number = 0;
    public Cost: number = 0;
    public Revenue: number = 0;

}
