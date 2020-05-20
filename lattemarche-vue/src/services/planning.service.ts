import { WeeklyPlan } from '@/models/weeklyPlan.model';
import axios, { AxiosPromise } from 'axios';

export class PlanningService {

    constructor() { }

    public loadWeek(mondayDate: string): AxiosPromise<WeeklyPlan> {
        return axios.get('/api/planning/loadweek?mondayDate=' + mondayDate.substring(0,10));
    }

}