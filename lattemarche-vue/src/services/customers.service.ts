import axios, { AxiosPromise } from 'axios';
import { Customer } from '../models/customer.model';

export class CustomersService {
    constructor() { }

    public getCustomers(): AxiosPromise<Customer[]> {
        return axios.get('/api/customers/search');
    }

}