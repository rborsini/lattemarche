import axios, { AxiosPromise } from 'axios';
import { OrderRow, OrderSearchModel } from '@/models/orderRow.model';
import { Order } from '@/models/order.model';
import { WorkOrder } from '@/models/workOrder.model';
import { DocumentItem } from '@/models/documentItem.model';
import { DocumentStatus } from '@/models/documentStatus.model';
import { DocumentsService } from './documents.service';

export class OrdersService extends DocumentsService {
    constructor() {
        super();
    }

    public search(parameters: OrderSearchModel): AxiosPromise<OrderRow[]> {
        return axios.get('/api/orders/search', { params: parameters });
    }

    public statuses(currentStatus?: number): AxiosPromise<DocumentStatus[]> {        
        return axios.get('/api/orders/statuses?currentStatus=' + currentStatus);
    }
    
    public details(id: string): AxiosPromise<Order> {
        return axios.get('/api/orders/details?id=' + id);
    }

    public save(order: Order): AxiosPromise<Order> {
        return axios.post('/api/orders/save', order);         
    }

    public transformToWorkOrders(items: DocumentItem[]): AxiosPromise<WorkOrder[]> {
        return axios.post('/api/orders/transform', items);         
    }

    public getCode(orderId: string, businessSublineId: string, date: string): AxiosPromise<string> {
        return axios.get('/api/orders/code?orderId=' + orderId + '&businessSublineId=' + businessSublineId + '&date=' + date);
    }

    public updateItems(order:Order): Promise<Order> {
        return new Promise<Order>((resolve) => {
            return super.updateDocumentItems(order.Items, order.Date_Str)
            .then(response => {
                order.Items=response; 
            });
            resolve(order);
        });
    }

    public delete(order: Order): AxiosPromise<Order> {
        return axios.delete('/api/orders/delete?id='+ order.Id);         
    }

}