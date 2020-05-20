import { BaseSearchModel } from '../baseSearchModel';

export class Invoice {
    public Id: string = "";

    public Number: number = 0;
    public Date: Date = new Date();
    public DateStr: string = "";
    public Year: number = 0;

    public Customer_Id: number = 0;
    public Customer_BusinessName: string = "";
    public Customer_VAT_Number: string = "";
    public Customer_FC: string = "";

    public RegistrationDate: Date = new Date();
    public RegistrationDateStr: string = "";

    public DueDate: Date = new Date();
    public DueDateStr: string = "";

    public IsClose: boolean = false;

    public Amount: number = 0;
    public VAT: number = 0;
    public Total_Amount: number = 0;
}

export class InvoiceSearchModel extends BaseSearchModel {

    public HeadQuarted_Id?: number;
    public Customer_Id?: number;
    public Customer_BusinessName: string = "";
    public Customer_VAT_Number: string = "";
    public Customer_FC: string = "";

    public Number_From?: number;
    public Number_To?: number;

    public Year?: number;
    public IsClose?: boolean;

    public Date_From_Str: string = "";
    public Date_To_Str: string = "";

    public DueDate_From_Str: string = "";
    public DueDate_To_Str: string = "";
}