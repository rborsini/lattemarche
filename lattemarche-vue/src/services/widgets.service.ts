import axios, { AxiosPromise } from 'axios';
import { Widget, WidgetParameters } from '@/models/widget.model';

export class WidgetsService {
    constructor() { }

    public details(parameters: WidgetParameters): AxiosPromise<Widget> {
        return axios.get('/api/widgets/details', { params: parameters });
    }

}