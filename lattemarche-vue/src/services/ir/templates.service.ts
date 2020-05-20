import axios, { AxiosPromise } from 'axios';
import { Template } from '../../models/ir/template.model';

export class TemplatesService {
    constructor() { }

    public index(): AxiosPromise<Template[]> {
        return axios.get('/api/templates');
    }

    public details(id: string): AxiosPromise<Template> {
        return axios.get('/api/templates/details?id=' + id);
    }

    public save(role: Template): AxiosPromise<Template> {
        return axios.post('/api/templates/save', role);
    }

    public delete(id: string): AxiosPromise<Template> {
        return axios.delete('/api/templates/delete?id=' + id);
    }

}