import GraficoWidgetModel from './graficoWidget.model';

export class Record {
    public Id: number = 0;
    public Date!: Date;
    public Data_Str: string = "";
    public Qta_Kg: number = 0;
    public Qta_Lt: number = 0;
    public Temperatura: number = 0;
    public Trasportatore: string = "";
    public Acquirente: string = "";
    public Destinatario: string = "";
    public TipoLatte: string = "";
}

export class AnalisiQuantitativaWidget {
    public AndamentoMensile: GraficoWidgetModel = new GraficoWidgetModel();
    public AndamentoGiornaliero: GraficoWidgetModel = new GraficoWidgetModel();
    public Records: Record[] = [];
}