export class Article {
    public Id: string = "";
    public ArticleCode: string = "";
    public CompanyCode: number = 0;
    public Description: string = "";
    public Category: string = "";
    public SubCategory: string = "";
    public UOM: string = "";

    public Price:number=0;
}

export class ArticleSearchModel {
    public Line: string = "";
    public ArticleCode: string[]=[];
    public Date_Str: string = "";
    public SubCategory: string = "";
}