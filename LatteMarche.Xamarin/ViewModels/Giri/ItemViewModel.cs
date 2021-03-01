using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.Models;

namespace LatteMarche.Xamarin.ViewModels.Giri
{
    public class ItemViewModel
    {
        #region Constants

        private const string CHIUDI = "Chiudi";
        private const string TRASBORDA = "Trasborda";
        private const string RIAPRI = "Riapri";
        private const string STAMPA = "Stampa";
        private const string ELIMINA = "Elimina";

        #endregion

        #region Properties

        public Color BackgroundColor => this.DataConsegna.HasValue ? Color.FromHex("#CCC") : Color.FromHex("#FFF");

        public int Id { get; set; }

        public int? IdTemplateGiro { get; set; }

        public string Titolo { get; set; }

        public string CodiceLotto { get; set; }

        public string SubTotaleStr { get; set; }

        public DateTime DataCreazione { get; set; }

        public DateTime? DataConsegna { get; set; }

        public DateTime? DataUpload { get; set; }

        public MaterialMenuItem[] Actions
        {
            get
            {
                var actions = new List<MaterialMenuItem>();

                if (!this.DataConsegna.HasValue && Connectivity.NetworkAccess != NetworkAccess.None)
                    actions.Add(new MaterialMenuItem { Text = CHIUDI });

                if (!this.DataConsegna.HasValue && Connectivity.NetworkAccess != NetworkAccess.None)
                    actions.Add(new MaterialMenuItem { Text = TRASBORDA });

                if (this.DataConsegna.HasValue)
                    actions.Add(new MaterialMenuItem { Text = STAMPA });

                if (!this.DataUpload.HasValue && !this.DataConsegna.HasValue)
                    actions.Add(new MaterialMenuItem { Text = ELIMINA });

                if (this.DataConsegna.HasValue && !this.DataUpload.HasValue)
                    actions.Add(new MaterialMenuItem { Text = RIAPRI });

                return actions.ToArray();
            }
        }

        public ICommand MenuCommand => new Command<MaterialMenuResult>((s) => MenuSelected(s));

        #endregion

        #region Events

        public event EventHandler OnItem_Closing;
        public event EventHandler OnItem_Transfering;
        public event EventHandler OnItem_Opening;
        public event EventHandler OnItem_Printing;
        public event EventHandler OnItem_Deleting;

        #endregion

        #region Methods

        /// <summary>
        /// Comandi singolo item
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private async Task MenuSelected(MaterialMenuResult i)
        {
            var commandString = this.Actions[i.Index].Text;

            switch(commandString)
            {
                case CHIUDI:
                    this.OnItem_Closing(this, null);
                    break;
                case TRASBORDA:
                    this.OnItem_Transfering(this, null);
                    break;
                case RIAPRI:
                    this.OnItem_Opening(this, null);
                    break;
                case STAMPA:
                    this.OnItem_Printing(this, null);
                    break;
                case ELIMINA:
                    this.OnItem_Deleting(this, null);
                    break;
            }

            await Task.CompletedTask;
        }

        #endregion

    }
}
