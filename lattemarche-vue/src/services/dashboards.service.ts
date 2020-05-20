import axios, { AxiosPromise } from 'axios';
import { Dashboard } from '@/models/dashboard.model';



export class DashboardsService {
    constructor() { }

    public index(): AxiosPromise<Dashboard[]> {
        return axios.get('/api/dashboards/');
    }

}