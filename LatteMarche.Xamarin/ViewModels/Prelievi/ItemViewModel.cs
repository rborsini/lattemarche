using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.Models;

namespace LatteMarche.Xamarin.ViewModels.Prelievi
{
    public class ItemViewModel
    {

        #region Constants

        private const string ELIMINA = "Elimina";

        #endregion

        #region Properties

        public string Id { get; set; }

        public int? IdGiro { get; set; }

        public string Titolo { get; set; }

        public DateTime? DataPrelievo { get; set; }

        public MaterialMenuItem[] Actions
        {
            get
            {
                var actions = new List<MaterialMenuItem>();

                actions.Add(new MaterialMenuItem { Text = ELIMINA });

                return actions.ToArray();
            }
        }

        public ICommand MenuCommand => new Command<MaterialMenuResult>((s) => MenuSelected(s));

        #endregion

        #region Events

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

            switch (commandString)
            {
                case ELIMINA:
                    this.OnItem_Deleting(this, null);
                    break;
            }

            await Task.CompletedTask;
        }

        #endregion

    }
}
