using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Models
{
    public class MenuItemViewModel
    {
        public string Title { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }

        public List<MenuItemViewModel> Items { get; set; }

        public bool Authorized { get; set; }

        public bool Visible
        {
            get
            {
                return this.Items.Count == 0 ? this.Authorized : this.Items.Any(i => i.Authorized);
            }
        }

        public MenuItemViewModel(string title)
        {
            this.Title = title;
            this.Items = new List<MenuItemViewModel>();
        }

        public MenuItemViewModel(string title, string action, string controller, string username, bool authorized)
            : this(title, action, controller, "", username, authorized)
        {

        }

        public MenuItemViewModel(string title, string action, string controller, string area, string username, bool authorized)
        {
            this.Title = title;
            this.Action = action;
            this.Controller = controller;
            this.Area = area;
            this.Authorized = authorized;
            this.Items = new List<MenuItemViewModel>();
        }

        public MenuItemViewModel(string title, string action, string controller, bool auhtorized)
        {
            this.Title = title;
            this.Action = action;
            this.Controller = controller;
            this.Authorized = auhtorized;
            this.Items = new List<MenuItemViewModel>();
        }

    }

    public class MenuViewModel : List<MenuItemViewModel>
    {


    }
}