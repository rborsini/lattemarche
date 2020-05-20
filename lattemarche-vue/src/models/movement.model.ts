import { MovementType } from './movementType.model';

export class Movement {
    
    public Id: number = 0;
    public WorkOrderId: string = "";
    public Date: Date = new Date();
    public ArticleId: string = "";
    public Date_Str: string = "";
    public Description: string = "";  
    public MovementTypeId: string = "";
    public MovementType_Description: string = "";
    public MovementType_Direction: string = "";
    public Quantity: number = 0;
    public TotalAmount: number = 0;
    public UnitAmount: number = 0;
    public Recipient: string = "";
    public SubRecipient: string = "";
    public Note: string = "";
    public InvoiceNumber: string = "";
    public InvoiceDate: Date = new Date();
    public InvoiceDate_Str: string = "";
}