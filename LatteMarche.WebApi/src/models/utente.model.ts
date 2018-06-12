export class Utente {
    public Id: number = 0;
    public Nome: string = "";
    public Cognome: string = "";
    public PivaCF: string = "";
    public Indirizzo: string = "";
    public Username: string = "";
    public Password?: string = "";
    public Abilitato: boolean = false;
    public Visibile: boolean = false;
    public RagioneSociale: string = "";
    public CodiceAllevatore: string = "";
    public QuantitaLatte: number = 0;
    public Telefono: string = "";
    public Cellulare: string = "";
    public Sesso: string = "";
    public NumeroComunicazione: string = "";
    public Note: string = "";
    public SiglaProvincia: string = "";
    public IdComune: number = 0;
    public IdProfilo: number = 0;
    public IdTipoLatte: number = 0;
}