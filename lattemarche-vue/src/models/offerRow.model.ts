import { BaseSearchModel } from './baseSearchModel';

export class OfferRow {
    
    public Id: string = "";
    public Code: string = "";
    public Customer_FullBusinessName: string = "";
    public BusinessSubline_Description = "";
    public HeadQuarter_Description = "";
    public Date: string = "";
    public Status_Code: string = "";
    public IsClosed_Descr: string = "";

}

export class OfferSearchModel extends BaseSearchModel {

    public CustomerId?: number;
    public Author: string = "";   
    public Code: string = "";
     
    public HeadQuarterId: number = -1;
    public BusinessSublineId: string = "";

    public Date_From_Str: string = "";
    public Date_To_Str: string = "";
    
    public Statuses: number[] = [];
    public ClosedStatuses: number[] = [];
    
}