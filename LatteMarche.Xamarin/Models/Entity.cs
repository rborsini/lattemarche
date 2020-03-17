using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Models
{
    public abstract class Entity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
    }
}
