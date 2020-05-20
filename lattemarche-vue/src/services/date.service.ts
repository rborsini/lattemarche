import axios, { AxiosPromise } from 'axios';
import { Week } from '@/models/week.model';

export class DateService {
    constructor() { }

    public getWeeks(): AxiosPromise<Week[]> {
        return axios.get('/api/dates/weeks');
    }

      // Restituisce il lunedì della settimana
    public getMonday(): Date {
        let date = new Date();
        var day = date.getDay(),
        diff = date.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
        return new Date(date.setDate(diff));
    }

}