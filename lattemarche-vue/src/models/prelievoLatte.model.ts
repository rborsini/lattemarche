import $ from 'jquery';
import { BaseSearchModel } from './baseSearch.model';
import { Trasbordo } from './trasbordo.model';

export class PrelievoLatte {
    public DataConsegna: string = "";
    public DataConsegnaStr: string = "";
    public DataPrelievo: string = "";
    public DataPrelievoStr: string = "";
    public DataUltimaMungitura: string = "";
    public DataUltimaMungituraStr: string = "";
    public Id: number = 0;
    public IdAcquirente: number = 0;
    public IdAllevamento: number = 0;
    public IdDestinatario: number = 0;
    public IdLabAnalisi: number = 0;
    public IdTrasportatore: number = 0;
    public IdTipoLatte: number = 0;
    public DescrizioneLatte: string = "";
    public SiglaLatte: string = "";
    public LastChange: string = "";
    public LastOperation: number = 0;
    public LottoConsegna: string = "";
    public NumeroMungiture: number = 0;
    public OraConsegna: string = "";
    public OraPrelievo: string = "";
    public OraUltimaMungitura: string = "";
    public Quantita: number = 0;
    public QuantitaLitri: number = 0;
    public Scomparto: string = "";
    public SerialeLabAnalisi: string = "";
    public Temperatura: number = 0;
    public IdAutocisterna: number = 0;
    public Lat: number = 0;
    public Lng: number = 0;
    public Allevamento_Lat: number = 0;
    public Allevamento_Lng: number = 0;    
    public DistanzaAllevamento: number = 0;
    public DistanzaAllevamento_Str: string = "";
    public DeviceId: string = "";
    public CodiceSitra: string = "";

    public Trasbordo: Trasbordo = new Trasbordo();
}



export class PrelieviLatteSearchModel extends BaseSearchModel {

    public DataPeriodoInizio_Str: string = "";
    public DataPeriodoFine_Str: string = "";

    public DataConsegnaInizio_Str: string = "";
    public DataConsegnaFine_Str: string = "";

    public IdAllevamento: number = 0;
    public IdTrasportatore?: number = 0;
    public IdAcquirente?: number = 0;
    public IdDestinatario?: number = 0;
    public IdCessionario?: number = 0;
    public IdTipoLatte?: number = 0;

    public LottoConsegna: string = "";
    public CodiceGiro: string = "";

    public decodeUrl(url: string) {

        var obj = super.parseUrl(url);
        
        this.DataPeriodoInizio_Str = this.getStringParam(obj, 'DataPeriodoInizio_Str');
        this.DataPeriodoFine_Str = this.getStringParam(obj, 'DataPeriodoFine_Str');

        this.DataConsegnaInizio_Str = this.getStringParam(obj, 'DataConsegnaInizio_Str');
        this.DataConsegnaFine_Str = this.getStringParam(obj, 'DataConsegnaFine_Str');        

        this.IdAllevamento = this.getNumberParam(obj, 'IdAllevamento');
        this.IdTrasportatore = this.getNumberParam(obj, 'IdTrasportatore');
        this.IdAcquirente = this.getNumberParam(obj, 'IdAcquirente');
        this.IdDestinatario = this.getNumberParam(obj, 'IdDestinatario');
        this.IdCessionario = this.getNumberParam(obj, 'IdCessionario');
        this.IdTipoLatte = this.getNumberParam(obj, 'IdTipoLatte');

        this.LottoConsegna = this.getStringParam(obj, 'LottoConsegna');
        this.CodiceGiro = this.getStringParam(obj, 'CodiceGiro');

    }

}