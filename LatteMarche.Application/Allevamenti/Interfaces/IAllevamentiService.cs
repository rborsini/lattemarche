using System;
using System.Collections.Generic;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Allevamenti.Interfaces
{

    public interface IAllevamentiService : IEntityService<Allevamento, int, AllevamentoDto>
	{
	    List<AllevamentoDto> GetAllevamentiSitra();

        List<AllevamentoRowDto> Search(AllevamentiSearchDto searchDto);

	}

}
