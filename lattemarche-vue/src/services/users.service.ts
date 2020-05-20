import axios, { AxiosPromise } from 'axios';
import { User } from '@/models/user.model';
import { ChangePassword } from '@/models/changePassword.model';

export class UsersService {
    constructor() { }

    public index(): AxiosPromise<User[]> {
        return axios.get('/api/users');
    }
    
    public details(id: string): AxiosPromise<User> {
        return axios.get('/api/users/details?id=' + id);
    }

    public save(utente: User): AxiosPromise<User> {
        return axios.post('/api/users/save', utente);
    }

    public delete(id: number): AxiosPromise<User> {
        return axios.delete( '/api/users/delete?id=' + id);
    }

    public changePassword(model: ChangePassword): AxiosPromise<string> {
        return axios.post('/api/users/changepassword', model);
    }
    
}
