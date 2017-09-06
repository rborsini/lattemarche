using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Attributes
{
    [System.AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ViewItem : System.Attribute
    {
        public string Id { get; set; }
        public string Page { get; set; }
        public string DisplayName { get; set; }

        public ViewItem() { }

        public ViewItem(string id, string page, string displayName)
        {
            this.Id = id;
            this.Page = page;
            this.DisplayName = displayName;
        }

    }
}