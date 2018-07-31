SELECT        
	prelievi.ID_PRELIEVO,
	prelievi.DATA_PRELIEVO,
	prelievi.DATA_CONSEGNA,
	prelievi.QUANTITA,
	prelievi.TEMPERATURA,
	prelievi.DATA_ULTIMA_MUNGITURA,
	prelievi.ID_ALLEVAMENTO,
	trim(utenti_allevamento.COGNOME) + ' ' + trim(utenti_allevamento.NOME)  as DESCR_ALLEVAMENTO,
	utenti_allevamento.PIVA_CF as PIVA_ALLEVAMENTO,
	prelievi.ID_DESTINATARIO,
	destinatari.RAG_SOC_DESTINATARIO,
	prelievi.ID_ACQUIRENTE,
	acquirenti.RAG_SOC_ACQUIRENTE,
	prelievi.ID_LABANALISI,
	prelievi.ID_TRASPORTATORE,
	trasportatori.RAGIONE_SOCIALE as TRASPORTATORE,
	autocisterne.TARGA_MEZZO,
	prelievi.NUMERO_MUNGITURE,
	prelievi.SCOMPARTO,
	prelievi.LOTTO_CONSEGNA,
	prelievi.CODICE_SITRA	
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
	UTENTI as trasportatori on prelievi.ID_TRASPORTATORE = trasportatori.ID_UTENTE
	
	LEFT OUTER JOIN
	AUTOCISTERNA as autocisterne on prelievi.ID_TRASPORTATORE = autocisterne.ID_TRASPORTATORE
