SELECT        
	prelievi.ID_PRELIEVO,
	prelievi.DATA_PRELIEVO,
	prelievi.DATA_CONSEGNA,
	prelievi.QUANTITA,
	prelievi.TEMPERATURA,
	prelievi.DATA_ULTIMA_MUNGITURA,
	prelievi.ID_ALLEVAMENTO,
	utenti_allevamento.RAGIONE_SOCIALE as DESCR_ALLEVAMENTO,
	utenti_allevamento.PIVA_CF as PIVA_ALLEVAMENTO,
	allevamenti.LATITUDINE as LAT_ALLEVAMENTO,
	allevamenti.LONGITUDINE as LNG_ALLEVAMENTO,
	prelievi.LATITUDINE as LAT_PRELIEVO,
	prelievi.LONGITUDINE as LNG_PRELIEVO,
	autocisterne.TARGA_MEZZO,
	prelievi.ID_DESTINATARIO,
	destinatari.RAG_SOC_DESTINATARIO,
	prelievi.ID_ACQUIRENTE,
	acquirenti.RAG_SOC_ACQUIRENTE,
	prelievi.ID_CESSIONARIO,
	cessionari.RAG_SOC_CESSIONARIO,
	prelievi.ID_LABANALISI,
	prelievi.ID_TRASPORTATORE,
	trasportatori.RAGIONE_SOCIALE as TRASPORTATORE,
	prelievi.NUMERO_MUNGITURE,
	prelievi.SCOMPARTO,
	prelievi.LOTTO_CONSEGNA,
	prelievi.CODICE_SITRA,
	tipo_latte.FATTORE_CONVERSIONE,
    tipo_latte.ID_TIPO_LATTE,
    tipo_latte.DESCRIZIONE AS DESCR_LATTE, 
    tipo_latte.DESCRIZIONE_BREVE AS SIGLA_LATTE
FROM            

	PRELIEVO_LATTE AS prelievi

	LEFT OUTER JOIN
	ANAGRAFE_ALLEVAMENTO as allevamenti on prelievi.ID_ALLEVAMENTO = allevamenti.ID_ALLEVAMENTO

	LEFT OUTER JOIN
	UTENTI as utenti_allevamento on allevamenti.ID_UTENTE = utenti_allevamento.ID_UTENTE

	LEFT OUTER JOIN
	ANAGRAFE_DESTINATARIO as destinatari on prelievi.ID_DESTINATARIO = destinatari.ID_DESTINATARIO

	LEFT OUTER JOIN
	ANAGRAFE_ACQUIRENTE as acquirenti on prelievi.ID_ACQUIRENTE = acquirenti.ID_ACQUIRENTE

	LEFT OUTER JOIN
	ANAGRAFE_CESSIONARIO as cessionari on prelievi.ID_CESSIONARIO = cessionari.ID_CESSIONARIO

	LEFT OUTER JOIN
	UTENTI as trasportatori on prelievi.ID_TRASPORTATORE = trasportatori.ID_UTENTE
	
	LEFT OUTER JOIN
	TIPO_LATTE as tipo_latte on prelievi.ID_TIPO_LATTE = tipo_latte.ID_TIPO_LATTE

	LEFT OUTER JOIN
	AUTOCISTERNA as autocisterne on autocisterne.ID_VEICOLO = prelievi.ID_AUTOCISTERNA
