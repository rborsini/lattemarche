import $ from 'jquery';

export abstract class BaseSearchModel {

    protected handlers: { (): void; }[] = [];

    public onChange(handler: { (): void }) : void {
        this.handlers.push(handler);
    }

    public offChange(handler: { (): void }) : void {
        this.handlers = this.handlers.filter(h => h !== handler);
    }    

    public clear() {
        this.decodeUrl('');
    }

    public toUrlQueryString(): string {

        var json = JSON.stringify(this, this.replacer);
        var obj = JSON.parse(json);

        return jQuery.param(obj);
    }

    abstract decodeUrl(url: string): void;

    protected trigger() {
        this.handlers.slice(0).forEach(h => h());
    }

    private replacer(key: string, value: any): any {

        if(key == "handlers")
            return undefined;
        else 
            return value;

    }

    protected parseUrl(url: string) : URLSearchParams {        
        url = url.replace('#', '');
        return new URLSearchParams(url);
    }

    protected getNumberParam(params: URLSearchParams, key: string): number {
        if(!params.has(key)) {
            return 0;
        }            
        else {
            return parseInt(params.get(key) as string);
        }
    }

    protected getStringParam(params: URLSearchParams, key: string): string {
        if(!params.has(key)) {
            return '';
        }            
        else {
            return params.get(key) as string;
        }
    }

}