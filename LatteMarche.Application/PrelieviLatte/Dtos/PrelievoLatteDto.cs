using System;
using AutoMapper;
using LatteMarche.Core.Models;
using RB.Date;


namespace LatteMarche.Application.PrelieviLatte.Dtos
{
    public class PrelievoLatteDto : EntityDto
    {
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

    }

    public class PrelieviLatteMappings
    {
        public static void Configure()
        {
            DateHelper helper = new DateHelper();
            Mapper.CreateMap<PrelievoLatte, PrelievoLatteDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdAllevamento, opts => opts.MapFrom(src => src.IdAllevamento))
                .ForMember(dest => dest.IdTrasportatore, opts => opts.MapFrom(src => src.IdTrasportatore))
                .ForMember(dest => dest.IdAquirente, opts => opts.MapFrom(src => src.IdAquirente))
                .ForMember(dest => dest.IdDestinatario, opts => opts.MapFrom(src => src.IdDestinatario))
                .ForMember(dest => dest.IdLabAnalisi, opts => opts.MapFrom(src => src.IdLabAnalisi))
                .ForMember(dest => dest.DataPrelievo, opts => opts.MapFrom(src => helper.FormatDate(src.DataPrelievo)))
                .ForMember(dest => dest.DataUltimaMungitura, opts => opts.MapFrom(src => helper.FormatDate(src.DataUltimaMungitura)))
                .ForMember(dest => dest.DataConsegna, opts => opts.MapFrom(src => helper.FormatDate(src.DataConsegna)))
                .ForMember(dest => dest.NumeroMungiture, opts => opts.MapFrom(src => src.NumeroMungiture))
                .ForMember(dest => dest.Quantita, opts => opts.MapFrom(src => src.Quantita))
                .ForMember(dest => dest.Temperatura, opts => opts.MapFrom(src => src.Temperatura))          
                .ForMember(dest => dest.Scomparto, opts => opts.MapFrom(src => src.Scomparto.Trim()))
                .ForMember(dest => dest.LottoConsegna, opts => opts.MapFrom(src => src.LottoConsegna.Trim()))
                .ForMember(dest => dest.SerialeLabAnalisi, opts => opts.MapFrom(src => src.SerialeLabAnalisi.Trim()))
                ;
                
        }
    }

}
