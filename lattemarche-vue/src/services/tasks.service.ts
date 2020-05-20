import axios, { AxiosPromise } from 'axios';
import { Task } from '@/models/task.model';

export class TasksService {
    constructor() { }

    public details(id: string): AxiosPromise<Task> {
        return axios.get('/api/tasks/details?id=' + id);
    }    

}