import { Invoice, InvoiceSearchModel } from '@/models/ir/invoice.model';
import axios, { AxiosPromise } from 'axios';

export class InvoicesService {

    public search(parameters: InvoiceSearchModel): AxiosPromise<Invoice[]> {
        return axios.get('/api/offers/search', { params: parameters });
    }

} 