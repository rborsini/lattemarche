import { Attachment } from './attachment.model';
import { DocumentTransition } from './documentTransition.model';

export class OfferRequest {
    
    public Id: string = "";
    public OfferId: string = "";
    public OfferCode: string = "";
    public CreationDate: string = "";
    public Date_Str: string = "";    
    public Author: string = "";
    public Note: string = "";
    public StatusId: number = 0;
    public Status_Code: string = "";
    public LastChangeTimestmap: Date = new Date();
    public BusinessSublineId: string = "";          //Id sottolinea
    public HeadQuarterId: number =-1;               //Id sede
    public CustomerId: number = 0;                  // Id Cliente
    public Customer_FullBusinessName: string = "";          // Ragione sociale cliente
    public Referent: string = "";
    public Seller: string = "";

    public Attachments: Attachment[] = [];
    public Transitions: DocumentTransition[] = [];


}