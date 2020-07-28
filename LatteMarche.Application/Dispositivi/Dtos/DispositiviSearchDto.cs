using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application.Dtos;

namespace LatteMarche.Application.Dispositivi.Dtos
{
    public class DispositiviSearchDto : BaseSearchDto
    {
        public override bool IsEmpty => this.IsFilterModelEmpty<DispositiviSearchDto>(this);

        public string FullText { get; set; }

    }
}
