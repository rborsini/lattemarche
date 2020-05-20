import axios, { AxiosPromise } from 'axios';
import { Auditor } from '@/models/auditor.model';

export class AuditorsService {
    constructor() { }

    public index(): AxiosPromise<Auditor[]> {
        return axios.get('/api/auditors/');
    }

    public details(id: string): AxiosPromise<Auditor> {
        return axios.get('/api/auditors/details?id=' + id);
    }

    public save(employee: Auditor): AxiosPromise<Auditor> {
        return axios.post('/api/auditors/save', employee);
    }

    public delete(id: number): AxiosPromise<Auditor> {
        return axios.delete('/api/auditors/delete?id=' + id);
    }

}