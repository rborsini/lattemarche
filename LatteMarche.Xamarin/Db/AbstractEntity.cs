using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Db
{
    public abstract class AbstractEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
    }
}
