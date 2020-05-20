import axios, { AxiosPromise } from 'axios';
import { OfferRequest } from '@/models/offerRequest.model';
import { DocumentStatus } from '@/models/documentStatus.model';

export class OfferRequestsService {
    constructor() { }

    public statuses(currentStatus?: number): AxiosPromise<DocumentStatus[]> {        
        return axios.get('/api/offerRequests/statuses?currentStatus=' + currentStatus);
    }

    public details(id: string): AxiosPromise<OfferRequest> {
        return axios.get('/api/offerRequests/details?id=' + id);
    }

    public save(offerRequest: OfferRequest): AxiosPromise<OfferRequest> {
        return axios.post('/api/offerRequests/save', offerRequest);         
    }

    public transformToOrder(offerRequest: OfferRequest): AxiosPromise<OfferRequest> {
        return axios.post('/api/offerRequests/transform', offerRequest);         
    }

    public delete(offerRequest: OfferRequest): AxiosPromise<OfferRequest> {
        return axios.delete('/api/offerRequests/delete?id='+ offerRequest.Id);         
    }

}