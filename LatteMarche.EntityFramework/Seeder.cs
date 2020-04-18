using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.EntityFramework
{
    public class Seeder
    {
        public static string Comuni_Data()
        {
            return @"
                SET IDENTITY_INSERT COMUNI ON
                    INSERT [dbo].[COMUNI] ([ID_COMUNE], [DESCRIZIONE], [PROVINCIA], [CAP], [ISTAT6]) VALUES (252, N'FILOTTRANO                                        ', N'AN', N'60024', N'042019')
                SET IDENTITY_INSERT COMUNI OFF
            ";
        }

        public static string TipoLatte_Data()
        {
            return @"
                SET IDENTITY_INSERT TIPO_LATTE ON
                    INSERT [dbo].[TIPO_LATTE] ([ID_TIPO_LATTE], [DESCRIZIONE], [DESCRIZIONE_BREVE], [FATTORE_CONVERSIONE], [FLAG_INVIO_SITRA]) VALUES (1, N'QM-Alta Qualità', N'QM-AQ', CAST(1.030 AS Decimal(18, 3)), 1)
                    INSERT [dbo].[TIPO_LATTE] ([ID_TIPO_LATTE], [DESCRIZIONE], [DESCRIZIONE_BREVE], [FATTORE_CONVERSIONE], [FLAG_INVIO_SITRA]) VALUES (2, N'Alta Qualità', N'AQ', CAST(1.030 AS Decimal(18, 3)), 0)
                    INSERT [dbo].[TIPO_LATTE] ([ID_TIPO_LATTE], [DESCRIZIONE], [DESCRIZIONE_BREVE], [FATTORE_CONVERSIONE], [FLAG_INVIO_SITRA]) VALUES (3, N'Fresco Alimentare', N'FA', CAST(1.030 AS Decimal(18, 3)), 0)
                    INSERT [dbo].[TIPO_LATTE] ([ID_TIPO_LATTE], [DESCRIZIONE], [DESCRIZIONE_BREVE], [FATTORE_CONVERSIONE], [FLAG_INVIO_SITRA]) VALUES (4, N'Caseificazione', N'CAS', CAST(1.030 AS Decimal(18, 3)), 0)
                    INSERT [dbo].[TIPO_LATTE] ([ID_TIPO_LATTE], [DESCRIZIONE], [DESCRIZIONE_BREVE], [FATTORE_CONVERSIONE], [FLAG_INVIO_SITRA]) VALUES (6, N'Latte di Pecora', N'PEC', CAST(1.037 AS Decimal(18, 3)), 0)
                    INSERT [dbo].[TIPO_LATTE] ([ID_TIPO_LATTE], [DESCRIZIONE], [DESCRIZIONE_BREVE], [FATTORE_CONVERSIONE], [FLAG_INVIO_SITRA]) VALUES (7, N'Jersey', N'JE', CAST(1.035 AS Decimal(18, 3)), 0)
                SET IDENTITY_INSERT TIPO_LATTE OFF
            ";
        }

        public static string Profilo_Data()
        {
            return @"
                SET IDENTITY_INSERT PROFILO ON
                    INSERT [dbo].[PROFILO] ([ID_PROFILO], [DESCRIZIONE_PROFILO]) VALUES (1, N'Admin     ')
                    INSERT [dbo].[PROFILO] ([ID_PROFILO], [DESCRIZIONE_PROFILO]) VALUES (2, N'Redatore')
                    INSERT [dbo].[PROFILO] ([ID_PROFILO], [DESCRIZIONE_PROFILO]) VALUES (3, N'Allevatore       ')
                    INSERT [dbo].[PROFILO] ([ID_PROFILO], [DESCRIZIONE_PROFILO]) VALUES (4, N'Laboratorio')
                    INSERT [dbo].[PROFILO] ([ID_PROFILO], [DESCRIZIONE_PROFILO]) VALUES (5, N'Trasportatore')
                    INSERT [dbo].[PROFILO] ([ID_PROFILO], [DESCRIZIONE_PROFILO]) VALUES (6, N'Destinatario')
                    INSERT [dbo].[PROFILO] ([ID_PROFILO], [DESCRIZIONE_PROFILO]) VALUES (7, N'Acquirente')
                SET IDENTITY_INSERT PROFILO OFF
            ";
        }

        public static string V_Allevatori_Schema()
        {
            return @"
                CREATE VIEW V_Allevatori AS
                SELECT        
	                dbo.ANAGRAFE_ALLEVAMENTO.ID_ALLEVAMENTO, 
	                dbo.ANAGRAFE_ALLEVAMENTO.ID_UTENTE, 
	                dbo.ANAGRAFE_ALLEVAMENTO.INDIRIZZO_ALLEVAMENTO, 
	                dbo.ANAGRAFE_ALLEVAMENTO.ID_COMUNE, 
                    dbo.UTENTI.RAGIONE_SOCIALE, 	
	                dbo.UTENTI.NOME, 
	                dbo.UTENTI.COGNOME, 
	                dbo.UTENTI.ID_TIPO_LATTE,
	                dbo.ANAGRAFE_ALLEVAMENTO.IDSITRA_STABILIMENTO_ALLEVAMENTO, 
	                dbo.ANAGRAFE_ALLEVAMENTO.CODICE_ASL,
	                dbo.COMUNI.PROVINCIA, 
                    dbo.COMUNI.DESCRIZIONE
                FROM            

	                dbo.ANAGRAFE_ALLEVAMENTO 
	
	                INNER JOIN
                    dbo.UTENTI ON dbo.ANAGRAFE_ALLEVAMENTO.ID_UTENTE = dbo.UTENTI.ID_UTENTE 
	
	                INNER JOIN
                    dbo.COMUNI ON dbo.ANAGRAFE_ALLEVAMENTO.ID_COMUNE = dbo.COMUNI.ID_COMUNE  
            ";
        }

        public static string V_PrelieviLatte_Schema()
        {
            return @"
                CREATE VIEW V_PrelieviLatte AS
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
                 UTENTI as trasportatori on prelievi.ID_TRASPORTATORE = trasportatori.ID_UTENTE

                 LEFT OUTER JOIN
                 AUTOCISTERNA as autocisterne on prelievi.ID_TRASPORTATORE = autocisterne.ID_TRASPORTATORE

                 LEFT OUTER JOIN
                 TIPO_LATTE as tipo_latte on utenti_allevamento.ID_TIPO_LATTE = tipo_latte.ID_TIPO_LATTE
            ";
        }

        public static string V_Trasportatori_Schema()
        {
            return @"
                CREATE VIEW V_Trasportatori AS
                SELECT DISTINCT 

                 dbo.UTENTI.NOME, 
                 dbo.UTENTI.COGNOME, 
                 dbo.UTENTI.INDIRIZZO, 
                 dbo.UTENTI.TELEFONO,
                 dbo.UTENTI.CELLULARE, 
                 dbo.UTENTI.ID_UTENTE, 
                 dbo.UTENTI.PIVA_CF,
                 dbo.UTENTI.RAGIONE_SOCIALE,
                 dbo.COMUNI.DESCRIZIONE, 
                 dbo.COMUNI.PROVINCIA

                FROM            

                 dbo.UTENTI 

                 INNER JOIN
                    dbo.COMUNI ON dbo.UTENTI.ID_COMUNE = dbo.COMUNI.ID_COMUNE 

                 INNER JOIN
                    dbo.PROFILO ON dbo.UTENTI.ID_PROFILO = dbo.PROFILO.ID_PROFILO 

                 CROSS JOIN
                    dbo.PROFILO AS PROFILO_1

                WHERE        
                 (dbo.PROFILO.DESCRIZIONE_PROFILO = 'Trasportatore')
            ";
        }

    }
}
