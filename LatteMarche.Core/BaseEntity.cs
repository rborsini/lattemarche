using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Core
{
    /// <summary>
    /// Classe base per tutte le entità del modello
    /// </summary>
    public abstract class BaseEntity : ICloneable, IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
