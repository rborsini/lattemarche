import { Movement } from './movement.model';
import { Attachment } from './attachment.model';
import { Task } from './task.model';
// import { Schedule } from './schedule.model';

export class WorkOrder {
    public Id: string = "";
    public Code: string = "";
    public Description: string = "";
    public Status: string = "";
    public CustomerId: number = 0;
    public Customer_FullBusinessName: string = "";
    public BusinessSublineId: number = 0;
    public BusinessSubline_Description: string = "";
    public HeadQuarterId: number = 0;
    public HeadQuarter_Description: string = "";
    public Seller: string = "";

    public StartDate?: Date;
    public StartDate_Str: string = "";

    public TakeoverDate?: Date;
    public TakeoverDate_Str: string = "";

    public DueDate?: Date;
    public DueDate_Str: string = "";

    public EndDate?: Date;
    public EndDate_Str: string = "";

    public Movements: Movement[] = [];

    public Margin: number = 0;
    public MarginPerc: number = 0;
    public Cost: number = 0;
    public Revenue: number = 0;

    public Note: string = "";
    public Order_Note: string = "";

    public Attachments: Attachment[] = [];
    public PreviousAttachments: Attachment[] = [];
    public Order_Id: string = "";
    public Order_Code: string = "";

    public Tasks: Task[] = [];
    // public Schedules: Schedule[] = [];

}
