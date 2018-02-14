using LatteMarche.Application.PrelieviLatte.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void Push();

    }
}
