using System;
using AutoMapper;
using LatteMarche.Core.Models;
//using RB.Date;

namespace LatteMarche.Application.VPrelieviLatte.Dtos
{
    public class VPrelievoLatteDto : EntityDto
    {
        public int Id { get; set; }
        public int IdAllevamento { get; set; }
        public DateTime DataPrelievo { get; set; }
        public DateTime DataUltimaMungitura { get; set; }
        public int NumeroMungiture { get; set; }
        public Decimal Quantita { get; set; }
        public Decimal Temperatura { get; set; }
        public int IdTrasportatore { get; set; }
        public string TrasportatoreCognome { get; set; }
        public int IdAquirente { get; set; }
        public string AcquirenteRagSoc { get; set; }
        public int IdDestinatario { get; set; }
        public string DestinatarioRagSoc { get; set; }
        public DateTime DataConsegna { get; set; }
        public string Scomparto { get; set; }
        public string LottoConsegna { get; set; }
        public int IdLabAnalisi { get; set; }
        public string LabAnalisi { get; set; }
        public string SerialeLabAnalisi { get; set; }

    }

    public class VPrelieviLatteMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<V_PrelievoLatte, VPrelievoLatteDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdAllevamento, opts => opts.MapFrom(src => src.IdAllevamento))//data
                .ForMember(dest => dest.DataPrelievo, opts => opts.MapFrom(src => src.DataPrelievo))//data
                //.ForMember(dest => dest.DataPrelievo, opts => opts.MapFrom(src => DateHelper.FormatDate(src.DataPrelievo)))
                .ForMember(dest => dest.DataUltimaMungitura, opts => opts.MapFrom(src => src.DataUltimaMungitura))//data
                .ForMember(dest => dest.NumeroMungiture, opts => opts.MapFrom(src => src.NumeroMungiture))
                .ForMember(dest => dest.Quantita, opts => opts.MapFrom(src => src.Quantita))
                .ForMember(dest => dest.Temperatura, opts => opts.MapFrom(src => src.Temperatura))
                .ForMember(dest => dest.IdTrasportatore, opts => opts.MapFrom(src => src.IdTrasportatore))
                .ForMember(dest => dest.TrasportatoreCognome, opts => opts.MapFrom(src => src.TrasportatoreCognome.Trim()))
                .ForMember(dest => dest.IdAquirente, opts => opts.MapFrom(src => src.IdAquirente))
                .ForMember(dest => dest.AcquirenteRagSoc, opts => opts.MapFrom(src => src.AcquirenteRagSoc.Trim()))
                .ForMember(dest => dest.IdDestinatario, opts => opts.MapFrom(src => src.IdDestinatario))
                .ForMember(dest => dest.DestinatarioRagSoc, opts => opts.MapFrom(src => src.DestinatarioRagSoc.Trim()))
                .ForMember(dest => dest.DataConsegna, opts => opts.MapFrom(src => src.DataConsegna))//data
                .ForMember(dest => dest.Scomparto, opts => opts.MapFrom(src => src.Scomparto.Trim()))
                .ForMember(dest => dest.LottoConsegna, opts => opts.MapFrom(src => src.LottoConsegna.Trim()))
                .ForMember(dest => dest.IdLabAnalisi, opts => opts.MapFrom(src => src.IdLabAnalisi))
                .ForMember(dest => dest.LabAnalisi, opts => opts.MapFrom(src => src.LabAnalisi.Trim()))
                .ForMember(dest => dest.SerialeLabAnalisi, opts => opts.MapFrom(src => src.SerialeLabAnalisi.Trim()))
                ;
                
        }
    }

}
