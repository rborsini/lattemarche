using System;
using AutoMapper;
using LatteMarche.Core.Models;
using RB.Date;


namespace LatteMarche.Application.PrelieviLatte.Dtos
{
    public class PrelievoLatteDto : EntityDto
    {
        private DateHelper dateHelper;

        public int Id { get; set; }
        public int IdAllevamento { get; set; }
        public int IdDestinatario { get; set; }
        public int IdAquirente { get; set; }
        public int IdTrasportatore { get; set; }
        public int IdLabAnalisi { get; set; }
        public DateTime DataPrelievo { get; set; }
        public DateTime DataConsegna { get; set; }
        public DateTime DataUltimaMungitura { get; set; }
        public Decimal Quantita { get; set; }
        public Decimal Temperatura { get; set; }
        public int NumeroMungiture { get; set; }
        public string Scomparto { get; set; }
        public string LottoConsegna { get; set; }
        public string SerialeLabAnalisi { get; set; }

        public string DataPrelievoStr
        {
            get { return new DateHelper().FormatDate(this.DataPrelievo); }
            set { this.DataPrelievo = this.dateHelper.ConvertToDateTime(value).HasValue ? this.dateHelper.ConvertToDateTime(value).Value : DateTime.MinValue; }
        }
        public string OraPrelievo { get { return this.DataPrelievo.ToString("HH:mm"); } }

        public string DataConsegnaStr
        {
            get { return new DateHelper().FormatDate(this.DataConsegna); }
            set { this.DataConsegna = this.dateHelper.ConvertToDateTime(value).HasValue ? this.dateHelper.ConvertToDateTime(value).Value : DateTime.MinValue; }
        }

        public string OraConsegna { get { return this.DataConsegna.ToString("HH:mm"); } }

        public string DataUltimaMungituraStr
        {
            get { return new DateHelper().FormatDate(this.DataUltimaMungitura); }
            set { this.DataUltimaMungitura = this.dateHelper.ConvertToDateTime(value).HasValue ? this.dateHelper.ConvertToDateTime(value).Value : DateTime.MinValue; }
        }

        public string OraUltimaMungitura { get { return this.DataUltimaMungitura.ToString("HH:mm"); } }

        public PrelievoLatteDto()
        {
            this.dateHelper = new DateHelper();
        }

    }

    public class PrelieviLatteMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<PrelievoLatte, PrelievoLatteDto>();
            Mapper.CreateMap<PrelievoLatteDto, PrelievoLatte>();

        }
    }

}
