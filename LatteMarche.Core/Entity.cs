using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LatteMarche.Core
{
	/// <summary>
	/// Classe base per tutte le entità del modello
	/// </summary>
	public abstract class Entity<TPrimaryKey> : ICloneable, IObjectState
    {
		[Key]
		public virtual TPrimaryKey Id { get; set; }

        [NotMapped]
        public ObjectState ObjectState { get; set; }

        public object Clone()
		{
			return this.MemberwiseClone();
		}

	}
}
