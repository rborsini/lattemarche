using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Interfaces
{
    public interface IFileSystem
    {
        /// <summary>
        /// Esporta il file del database nella cartella download
        /// </summary>
        void ExportDb(string dbFilePath);
    }
}
