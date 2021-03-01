using LatteMarche.Xamarin.Rest.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels.Trasbordi
{
    public class ItemViewModel
    {
        public string TargaOrigine { get; set; }
        public DateTime Data { get; set; }
        public int NumeroPrelievi { get; set; }

        public TrasbordoDto Dto { get; set; }

        public Command ImportCommand { get; set; }

        public event EventHandler OnItem_Importing;


        public ItemViewModel()
        {
            this.ImportCommand = new Command(async () => await ExecuteImportCommand());
        }

        private async Task ExecuteImportCommand()
        {
            this.OnItem_Importing(this, null);
            await Task.CompletedTask;
        }
    }
}
