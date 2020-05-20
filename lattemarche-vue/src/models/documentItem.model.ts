export class DocumentItem {
    
    public Id: string = "";
    public DocumentId: string = "";
    public Position: number = 0;
    public ArticleId: string = "";
    public ArticleCode: string = "";
    public Description: string = "";
    public UOM: string = "";
    public Quantity: number = 1;
    public UnitPrice: number = 0;
    public TotalPrice: number = 0;
    //Usato solo in locale, non presente sul server
    public SuggestedPrice: number = 0;
    public Origin_Id: string = "";
    public Origin_Item_Id: string = "";
    public Destination_Id: string = "";
    public Destination_Item_Id: string = "";
    public WorkOrderId: string = "";
    public Note: string = "";

    public isSelected: boolean = false;
}