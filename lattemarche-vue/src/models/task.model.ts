export class Task {
    
    public Id: string = "";
    
    public WorkOrderId: string = "";
    public WorkOrder_Code: string = "";
    
    public ArticleId: string = "";
    public Article_Description: string = "";
    public Article_ArticleCode: string = "";

    public Description: string = "";
    
    public SupplierId: number = 0;
    public Supplier_Name: string = "";

    public AuditorId: number = 0;
    public Auditor_Name: string = "";

    public StartDate_Str: string = "";
    public EndDate_Str: string = "";

    public StartDate_Time: string = "";
    public EndDate_Time: string = "";

}