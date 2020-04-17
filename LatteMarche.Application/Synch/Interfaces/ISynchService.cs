using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.Synch.Interfaces
{
    public interface ISynchService
    {

        /// <summary>
        /// Download dal cloud verso il server locale
        /// </summary>
        void Pull();

        /// <summary>
        /// Upload dal server locale verso il cloud
        /// </summary>
        List<PrelievoLatte> Push();

    }
}
