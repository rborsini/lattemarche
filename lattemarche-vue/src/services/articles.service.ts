import axios, { AxiosPromise } from 'axios';
import { Article, ArticleSearchModel } from '../models/article.model';

export class ArticlesService {
    constructor() { }

    public index(): AxiosPromise<Article[]> {
        return axios.get('/api/articles/search');
    }

    public search(parameters?: ArticleSearchModel): AxiosPromise<Article[]> {
        return axios.get('/api/articles/search', { params: parameters });
    }

    public details(id: string): AxiosPromise<Article> {
        return axios.get('/api/articles/details?id=' + id);
    }

}