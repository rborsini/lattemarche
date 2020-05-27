import $ from 'jquery';

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
}

export class PrelieviLatteSearchModel {


    public DataPeriodoInizio_Str: string = "";
    public DataPeriodoFine_Str: string = "";

    public IdAllevamento?: number;
    public IdTrasportatore?: number;
    public IdAcquirente?: number;
    public IdDestinatario?: number;
    public IdTipoLatte?: number;

    public ToUrlQueryString(): string {
        return jQuery.param(this);
    }

}