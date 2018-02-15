using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Sitra.Dtos
{
    public class SitraDto
    {
        public AttributoLotto Lotto { get; set; }
        public List<Attributo> AttributiBaseList { get; set; }
        public List<Attributo> AttributiList { get; set; }

        public SitraDto()
        {
            this.Lotto = new AttributoLotto();
            this.AttributiBaseList = new List<Attributo>();
            this.AttributiList = new List<Attributo>();
        }

    }

    public class AttributoLotto
    {
        public string CodiceLotto { get; set; }
        public string DataProduzione { get; set; }
        public int Quantita { get; set; }
        public int IdUnitaMisura { get; set; }
        public string Referenza { get; set; }
        public string CodOperatore { get; set; }
        public int IdProdotto { get; set; }
        public long CUAA { get; set; }
    }

    public class Attributo
    {
        public int IdAttributo { get; set; }
        public string NomeAttributo { get; set; }
        public string ValoreAttributo { get; set; }
        public string TipoDiDato { get; set; }
        public string DescrizioneAttributo { get; set; }
        public bool Obbligatorio { get; set; }
    }
}
