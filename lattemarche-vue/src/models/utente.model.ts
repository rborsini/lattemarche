import { Allevamento } from './allevamento.model';
import { Autocisterna } from './autocisterna.model';
import { BaseSearchModel } from './baseSearch.model';

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

    public IdAcquirente?: number;
    public IdDestinatario?: number;
    public IdCessionario?: number;
    public IdAziendaTrasporti?: number;

    public Allevamenti: Allevamento[] = [];
    public Autocisterne: Autocisterna[] = [];

}

export class UtentiSearchModel extends BaseSearchModel {

    public IdProfilo: number = 0;
    public RagioneSociale: string = "";
    public Nome: string = "";
    public Cognome: string = "";
    public Username: string = "";

    public decodeUrl(url: string) {

        var obj = super.parseUrl(url);

        console.log("obj", obj);
        //this.idAllevamento = 

    }    

}