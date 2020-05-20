import { BaseSearchModel } from './baseSearchModel';

export class OrderRow {

    public Id: string = "";
    public Code: string = "";
    public Customer_FullBusinessName: string = "";
    public BusinessSubline_Description: string = "";
    public HeadQuarter_Description: string = "";
    public Date: string = "";
    public Status_Code: string = "";
    
}

export class OrderSearchModel extends BaseSearchModel {

    public CustomerId?: number;
    public Author: string = "";   
    public Code: string = "";
    
    public BusinessSublineId: string = "";          //Id sottolinea
    public HeadQuarterId: number =-1;               //Id sede
     
    public Date_From_Str: string = "";
    public Date_To_Str: string = "";

    public Statuses: number[] = [];
    
}