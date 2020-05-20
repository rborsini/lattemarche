import axios, { AxiosPromise } from 'axios';
import { Dropdown } from "@/models/dropdown.model";

export class DropdownService {
    constructor() { }

    public getPaymentTypes(): AxiosPromise<Dropdown> {
        return axios.get('/api/paymenttypes/index');
    }

    public getHeadQuarters(): AxiosPromise<Dropdown> {
        return axios.get('/api/headquarters/index');
    }

    public getBusinessSublines(): AxiosPromise<Dropdown> {
        return axios.get('/api/businesssublines/index');
    }

    public getDocumentTypes(): AxiosPromise<Dropdown> {
        return axios.get('/api/documenttypes/index');
    }

    public getOpenOrders(): AxiosPromise<Dropdown> {
        return axios.get('/api/orders/index');
    }

    public getSkills(): AxiosPromise<Dropdown> {
        return axios.get('/api/skills/index');
    }

    public getAuditors(supplierId: number = 0): AxiosPromise<Dropdown> {

        var url = '/api/auditors/dropdown';
        if(supplierId != 0)
            url += '?supplierId=' + supplierId;

        return axios.get(url);
    }

    public getSuppliers(): AxiosPromise<Dropdown> {
        return axios.get('/api/suppliers/dropdown');
    }

    public getUsers(role: string = ""): AxiosPromise<Dropdown> {
        return axios.get('/api/users/dropdown?role=' + role);
    }    

    public getArticles(): AxiosPromise<Dropdown> {
        return axios.get('/api/articles/dropdown');
    }    

    public getMovementTypes(direction: string = ""): AxiosPromise<Dropdown> {
        return axios.get('/api/movementTypes/dropdown?direction=' + direction);
    }     

    public getTimeOptions(): AxiosPromise<Dropdown> {
        return axios.get('/api/dates/timeOptions');
    }   

    public getWorkOrderStatuses(): AxiosPromise<Dropdown> {        
        return axios.get('/api/workOrders/statuses');
    }

    public getWorkOrders(): AxiosPromise<Dropdown> {
        return axios.get('/api/workOrders/dropdown');
    }

    public getTasks(): AxiosPromise<Dropdown> {
        return axios.get('/api/tasks/dropdown');
    }    

}