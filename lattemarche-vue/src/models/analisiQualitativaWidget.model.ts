import GraficoWidgetModel from './graficoWidget.model';

export class Record {
    public Campione: string = "";
    public CodiceASL: string = "";
    public DataRapporto_Str: string = "";
    public DataAccettazione_Str: string = "";
    public DataPrelievo_Str: string = "";
    public Grasso: number = 0;
    public Proteine: number = 0;
    public CaricaBatterica: number = 0;
    public CelluleSomatiche: number = 0;
}

export class AnalisiQualitativaWidget {
    public Grasso_Proteine: GraficoWidgetModel = new GraficoWidgetModel();
    public CaricaBatterica_CelluleSomatiche: GraficoWidgetModel = new GraficoWidgetModel();
    public Records: Record[] = [];
}