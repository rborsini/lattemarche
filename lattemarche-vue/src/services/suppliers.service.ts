import axios, { AxiosPromise } from 'axios';
import { Supplier } from '@/models/supplier.model';

export class SuppliersService {
    constructor() { }

    public index(): AxiosPromise<Supplier[]> {
        return axios.get('/api/suppliers/');
    }

    public details(id: string): AxiosPromise<Supplier> {
        return axios.get('/api/suppliers/details?id=' + id);
    }

    public save(employee: Supplier): AxiosPromise<Supplier> {
        return axios.post('/api/suppliers/save', employee);
    }

    public delete(id: number): AxiosPromise<Supplier> {
        return axios.delete('/api/suppliers/delete?id=' + id);
    }

}