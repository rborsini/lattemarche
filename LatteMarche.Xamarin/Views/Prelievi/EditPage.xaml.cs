using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.ViewModels.Prelievi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI;
using XF.Material.Forms.UI.Dialogs;

namespace LatteMarche.Xamarin.Views.Prelievi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        private EditViewModel viewModel;
        private bool firstLoad = true;

        public EditPage(EditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.firstLoad = true;
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            int i = 0;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            if(this.firstLoad)
            {
                this.viewModel.LoadCommand.Execute(null);
                this.firstLoad = false;
            }            
        }

        private void Kg_Unfocused(object sender, FocusEventArgs e)
        {
            this.viewModel.OnKgChanged((sender as MaterialTextField).Text);
        }

        private void Lt_Unfocused(object sender, FocusEventArgs e)
        {
            this.viewModel.OnLtChanged((sender as MaterialTextField).Text);
        }

        private async void Scomparto_Focused(object sender, FocusEventArgs e)
        {

            var ElencoScomparti = new List<string> { "1", "2", "3", "4", "5", "6" };

            (sender as MaterialTextField).Unfocus();

            this.viewModel.Scomparto = String.IsNullOrEmpty(this.viewModel.Scomparto) ? "" : this.viewModel.Scomparto;

            var scompartiSelezionati = this.viewModel.Scomparto.Split('-');
            var indiciSelezionati = new List<int>();

            foreach (var scomparto in this.viewModel.Scomparto.Split('-'))
            {
                if(ElencoScomparti.IndexOf(scomparto) > 0)
                    indiciSelezionati.Add(ElencoScomparti.IndexOf(scomparto));
            }

            var result = await MaterialDialog.Instance.SelectChoicesAsync(title: "Seleziona scomparto", selectedIndices: indiciSelezionati, dismissiveText: "Annulla", choices: ElencoScomparti.ToArray());

            var scomparti = new List<string>();

            if (result != null)
            {
                foreach (var index in result)
                {
                    scomparti.Add(ElencoScomparti[index]);
                }
            }

            this.viewModel.Scomparto = String.Join("-", scomparti);

        }

        /// <summary>
        /// #326256
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Allevamento_ChoiceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var allevamento = (e.SelectedItem as Allevamento);
            if (this.viewModel.Prelievo.IdAllevamento != allevamento.IdAllevamento)
            {
                // acquirente default
                var idAcquirenteDefault = allevamento.IdAcquirenteDefault;
                if (idAcquirenteDefault.HasValue && idAcquirenteDefault.Value != this.viewModel.AcquirenteSelezionato.Id)
                {
                    this.viewModel.AcquirenteSelezionato = this.viewModel.Acquirenti.First(a => a.Id == idAcquirenteDefault.Value);
                }

                // cessionario default
                var idCessionarioDefault = allevamento.IdCessionarioDefault;
                if (idCessionarioDefault.HasValue && idCessionarioDefault.Value != this.viewModel.CessionarioSelezionato.Id)
                {
                    this.viewModel.CessionarioSelezionato = this.viewModel.Cessionari.First(a => a.Id == idCessionarioDefault.Value);
                }

                // destinatario default
                var idDestinatarioDefault = allevamento.IdDestinatarioDefault;
                if (idDestinatarioDefault.HasValue && idDestinatarioDefault.Value != this.viewModel.DestinatarioSelezionato.Id)
                {
                    this.viewModel.DestinatarioSelezionato = this.viewModel.Destinatari.First(a => a.Id == idDestinatarioDefault.Value);
                }

            }


        }


    }
}