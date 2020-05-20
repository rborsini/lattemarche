import axios, { AxiosPromise } from 'axios';
import { DocumentItem } from '@/models/documentItem.model';
import { ArticlesService } from './articles.service';
import { ArticleSearchModel, Article } from '@/models/article.model';

export class DocumentsService {
    constructor() { }

    private articlesService: ArticlesService = new ArticlesService();

    protected updateDocumentItems(items: DocumentItem[], date_str: string): Promise<DocumentItem[]> {

        return new Promise<DocumentItem[]>((resolve) => {
            var parameters: ArticleSearchModel = new ArticleSearchModel();
            parameters.ArticleCode = items.map(i => i.ArticleCode);
            parameters.Date_Str = date_str;
            this.articlesService.search(parameters)
                .then(response => {
                    items.forEach(item => {
                        var article: any = response.data.find(a => item.ArticleCode == a.ArticleCode);
                        if (article != undefined) {
                            item.SuggestedPrice = article.Price;
                        } else {
                            item.UnitPrice = 0;
                        }
                    });
                    resolve(items);
                })
        });
        
    }


}