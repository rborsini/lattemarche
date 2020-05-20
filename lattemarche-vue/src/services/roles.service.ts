import axios, { AxiosPromise } from 'axios';
import { Role } from '@/models/role.model';

export class RolesService {
    constructor() { }

    public getRoles(): AxiosPromise<Role[]> {
        return axios.get('/api/roles');
    }

    public getDetails(id: string): AxiosPromise<Role> {
        return axios.get('/api/roles/details?id=' + id);
    }

    public save(role: Role): AxiosPromise<Role> {
        return axios.post('/api/roles/save', role);
    }

}
