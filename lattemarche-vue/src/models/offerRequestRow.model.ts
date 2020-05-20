import { BaseSearchModel } from './baseSearchModel';

export class OfferRequestRow {
    
    public Id: string = "";
    public BusinessSubline_Description = "";
    public Customer_FullBusinessName: string = "";          
    public Referent: string = "";
    public Author: string = "";
    public HeadQuarter_Description = "";
    public Date: string = "";          
    public Status_Code: string = "";         
}


export class OfferRequestSearchModel extends BaseSearchModel {

    public HeadQuarterId: number = -1;
    public CustomerId?: number;
    public Date_From_Str: string = "";
    public Date_To_Str: string = "";

    public BusinessSublineId: string = "";
    public Referent: string = "";   
    public Author: string = "";   
    public Statuses: number[] = [];



}