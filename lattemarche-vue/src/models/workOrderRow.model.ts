import { BaseSearchModel } from './baseSearchModel';

export class WorkOrderRow {

    public Id: string = "";
    public Code: string = "";      
    public BussinessSubline_Description: string = "";                 
    public Customer_FullBusinessName: string = "";                 
    public HeadQuarter_Description: string = "";                 
    public Description: string = "";                 
    public StatusStr: string = ""; 

    public TakeOverDate?: Date;
    public StartDate?: Date;
    public EndDate?: Date;
    public DueDate?: Date;

}


export class WorkOrderSearchModel extends BaseSearchModel {

    public CustomerId?: number;
    public Author: string = "";   
    public Code: string = "";
     
    public HeadQuarterId: number = -1;
    public BusinessSublineId: string = "";

    public SupplierId?: number;
    public AuditorId?: number;

    public StartDate_FromStr: string = "";
    public StartDate_ToStr: string = "";
    
    public Statuses: string[] = [];

}