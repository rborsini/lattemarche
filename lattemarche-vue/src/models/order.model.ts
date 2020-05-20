import { DocumentItem } from './documentItem.model';
import { Attachment } from './attachment.model';

export class Order {
    
    public Id: string = "";
    public Code: string = "";
    public Date_Str: string = "";  
    public Author: string = "";
    public StatusId: number = 0;
    public Status_Code: string = "";
    public LastChangeTimestmap: Date = new Date();
    public BusinessSubLine: string = "";
    public BusinessSublineId: string = "";          //Id sottolinea
    public PaymentTypeId: string = "";              //Id tipo pagamento
    public HeadQuarterId: number =-1;               //Id sede
    public CustomerId: number = 0;                  // Id Cliente
    public Customer_FullBusinessName: string = "";          // Ragione sociale cliente
    public Seller: string = "";
    
    public Items: DocumentItem[] = [];
    public Attachments: Attachment[] = [];
    public PreviousAttachments: Attachment[] = [];
    
}