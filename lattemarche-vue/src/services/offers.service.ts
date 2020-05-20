import axios, { AxiosPromise } from 'axios';
import { OfferRow, OfferSearchModel } from '@/models/offerRow.model';
import { Offer } from '@/models/offer.model';
import { Order } from '@/models/order.model';
import { DocumentStatus } from '@/models/documentStatus.model';
import { DocumentsService } from './documents.service';
import { DocumentItem } from '@/models/documentItem.model';

export class OffersService extends DocumentsService {

    constructor() {
        super();
    }

    public search(parameters: OfferSearchModel): AxiosPromise<OfferRow[]> {
        return axios.get('/api/offers/search', { params: parameters });
    }

    public statuses(currentStatus?: number): AxiosPromise<DocumentStatus[]> {
        return axios.get('/api/offers/statuses?currentStatus=' + currentStatus);
    }

    public getCode(offerId: string, businessSublineId: string, date: string): AxiosPromise<string> {
        return axios.get('/api/offers/code?offerId=' + offerId + '&businessSublineId=' + businessSublineId + '&date=' + date);
    }

    public details(id: string): AxiosPromise<Offer> {
        return axios.get('/api/offers/details?id=' + id);
    }

    public save(offer: Offer): AxiosPromise<Offer> {
        return axios.post('/api/offers/save', offer);
    }

    public transformToOrder(items: DocumentItem[], orderId: string = ""): AxiosPromise<Order> {
        return axios.post('/api/offers/transform?orderId=' + orderId, items);
    }

    public updateItems(offer: Offer): Promise<Offer> {
        return new Promise<Offer>((resolve) => {
            return super.updateDocumentItems(offer.Items, offer.Date_Str)
                .then(response => {
                    offer.Items = response;
                });
            resolve(offer);
        });
    }

    public delete(offer: Offer): AxiosPromise<Offer> {
        return axios.delete('/api/offers/delete?id=' + offer.Id);
    }

}