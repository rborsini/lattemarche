import { BaseSearchModel } from "./baseSearch.model";

export class Trasbordo {

    public Id: number = 0;
    public Targa_Origine: string = "";
    public Targa_Destinazione: string = "";

    public IdTemplateGiro: number = 0;
    public DenominazioneGiro: string = "";

    public Lat!: number;
    public Lng!: number;

    public Data_Str: string = "";

}

export class TrasbordiSearchModel extends BaseSearchModel {

    public DataInizio_Str: string = "";
    public DataFine_Str: string = "";

    public decodeUrl(url: string) {
        var obj = super.parseUrl(url);
        
        this.DataInizio_Str = this.getStringParam(obj, 'DataInizio_Str');
        this.DataFine_Str = this.getStringParam(obj, 'DataFine_Str');
    }

}