using System;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using LatteMarche.Application.PrelieviLatte.Dtos;
using WeCode.Application.Interfaces;
using System.Linq;
using LatteMarche.Application.Utenti.Dtos;

namespace LatteMarche.Application.PrelieviLatte.Interfaces
{

    public interface IPrelieviLatteService : IEntityService<PrelievoLatte, int, PrelievoLatteDto>
    {

        /// <summary>
        /// Recupero query base filtrata in base al ruolo e ai permessi dell'utente
        /// </summary>
        /// <param name="idUtente"></param>
        /// <returns></returns>
        IQueryable<V_PrelievoLatte> PrelieviAutorizzati(int idUtente);

        /// <summary>
        /// Ricerca prelievi latte
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        List<V_PrelievoLatte> Search(PrelieviLatteSearchDto searchDto);

        /// <summary>
        /// Pull prelievi per sincronizzazione
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        List<PrelievoLatte> Pull(DateTime timestamp);

        /// <summary>
        /// Push prelievi per sincronizzazione
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Nuovi prelievi inseriti</returns>
        List<PrelievoLatte> Push(List<PrelievoLatte> list);



    }

}
