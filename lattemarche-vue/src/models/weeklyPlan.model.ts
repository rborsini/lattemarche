import { Task } from './task.model';


export class WeeklyPlan {

    public AuditorPlans: AuditorWeeklyPlan[] = [];
}

export class AuditorWeeklyPlan {

    public AuditorId: number = 0;
    public AuditorName: string = "";

    public Days: AuditorDailyPlan[] = [];

}

export class AuditorDailyPlan {
    public Tasks: Task[] = [];
}