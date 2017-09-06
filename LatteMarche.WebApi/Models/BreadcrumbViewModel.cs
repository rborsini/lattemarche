using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Models
{
    public class BreadcrumbViewModel
    {
        public class BreadcrumbItem
        {
            public BreadcrumbItem()
            {
                // Default values
                this.AreaName = "";
                this.ControllerName = "Home";
                this.ActionName = "Index";
                this.DisplayName = "Home";
            }

            public string AreaName { get; set; }
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public string DisplayName { get; set; }

        }

        public BreadcrumbViewModel()
        {
            this.Items = new List<BreadcrumbItem>();
        }

        public string CssClass { get; set; }

        public List<BreadcrumbItem> Items { get; set; }
    }
}