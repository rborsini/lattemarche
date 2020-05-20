import axios, { AxiosPromise } from 'axios';
import { WorkOrderRow, WorkOrderSearchModel } from '@/models/workOrderRow.model';
import { WorkOrder } from '@/models/workOrder.model';
import { Dropdown } from '@/models/dropdown.model';

export class WorkOrdersService {
    constructor() { }

    public search(parameters?: WorkOrderSearchModel): AxiosPromise<WorkOrderRow[]> {
        return axios.get('/api/workOrders/search', { params: parameters });
    }

    public details(id: string): AxiosPromise<WorkOrder> {
        return axios.get('/api/workOrders/details?id=' + id);
    }

    public save(workOrder: WorkOrder): AxiosPromise<WorkOrder> {
        return axios.post('/api/workOrders/save', workOrder);         
    }

    public delete(workOrder: WorkOrder): AxiosPromise<WorkOrder> {
        return axios.delete('/api/workOrders/delete?id='+ workOrder.Id);         
    }

}