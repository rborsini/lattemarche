SELECT 
	allevamenti.ID_ALLEVAMENTO,
	allevamenti.CODICE_ASL,
	allevamenti.CUAA,
	allevamenti.ID_COMUNE,
	allevamenti.INDIRIZZO_ALLEVAMENTO,
	allevamenti.ID_UTENTE,
	utenti.RAGIONE_SOCIALE
FROM            
	ANAGRAFE_ALLEVAMENTO as allevamenti

	left outer join UTENTI as utenti
	on allevamenti.ID_UTENTE = utenti.ID_UTENTE
	