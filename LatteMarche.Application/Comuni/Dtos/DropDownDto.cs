using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Comuni.Dtos
{
    /// <summary>
    /// Dto per la visualizzazione di dropdown
    /// </summary>
    public class DropDownDto
    {
        public List<string> SelectedValues { get; set; }

        public string SelectedValue
        {
            get { return this.SelectedValues.Count > 0 ? this.SelectedValues.First() : String.Empty; }
            set
            {
                this.SelectedValues.Clear();
                this.SelectedValues.Add(value);
            }
        }

        public List<DropDownItem> Items { get; set; }

        public DropDownDto()
        {
            this.SelectedValues = new List<string>();
            this.Items = new List<DropDownItem>();
        }
    }

    public class DropDownItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public DropDownItem() { }

        public DropDownItem(string value, string text)
        {
            this.Value = value;
            this.Text = text;
        }

    }
}
