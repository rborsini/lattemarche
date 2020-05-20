export class TimeEntryWeek {

    public AuditorId?: number;
    public Tasks: TimeEntryTaskWeek[] = [];

}

export class TimeEntryTaskWeek {

    public WorkOrderId: string = "";
    public TaskId: string = "";
    public Description: string = "";
    public Days: TimeEntry[] = new Array<TimeEntry>(7);
    public Sum: number = 0;

    constructor(){
        for(var i = 0; i<7; i++) {
            this.Days[i] = new TimeEntry()
        }
    }

}

export class TimeEntry {
    public Id: string = "";

    public Description: string = "";

    public Quantity: number = 0;

    public Date: Date = new Date();

    public TaskId: string = "";

    public AssigneeId: number = 0;

    public UserId: string = "";

    public Timestamp: Date = new Date();
}