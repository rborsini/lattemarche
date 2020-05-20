import axios, { AxiosPromise } from 'axios';
import { TimeEntryWeek, TimeEntry } from '@/models/timeEntry.model';

export class TimeEntriesService {
    constructor() { }

    public delete(workOrderId: string, assigneeId: number, from: string, to: string,): AxiosPromise<TimeEntry> {
        return axios.delete('/api/timeEntries/delete?taskId=' + workOrderId+'&assigneeId='+ assigneeId+'&from='+ from.substring(0,10)+'&to='+ to.substring(0,10));
    }

    public details(assignee: number, mondayDate: string): AxiosPromise<TimeEntryWeek> {
        return axios.get('/api/timeEntries/details?assignee=' + assignee + '&mondayDate=' + mondayDate.substring(0,10));
    }

    public save(timeEntry: TimeEntry): AxiosPromise<TimeEntry> {
        return axios.post('/api/timeEntries/save', timeEntry);
    }

}