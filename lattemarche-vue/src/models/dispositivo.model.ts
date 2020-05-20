export class Dispositivo {
    public Id: string = "";
    public Attivo: boolean = false;
    public DataRegistrazione: Date = new Date();
    public DataUltimoDownload: Date = new Date();
    public DataUltimoUpload: Date = new Date();
    public Lat: number = 0;
    public Lng: number = 0;
    public VersioneApp: string = "";
    public IdTrasportatore: number = 0;
    public Trasportatore_RagioneSociale: string = "";
}