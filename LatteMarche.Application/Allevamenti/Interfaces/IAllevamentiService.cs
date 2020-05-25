using System;
using System.Collections.Generic;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Allevamenti.Interfaces
{

    public interface IAllevamentiService 
	{
        DropDownDto DropDown(int? idUtente = (int?)null);

    }

}
