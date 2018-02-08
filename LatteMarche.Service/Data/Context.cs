using LatteMarche.Application.PrelieviLatte.Dtos;
using System;
using System.Collections.Generic;
using LatteMarche.Service.Data;

namespace LatteMarche.Service.Data
{
    public class Context
    {
        private List<PrelievoLatteDto> prelievi;

        public IRepository<PrelievoLatteDto, int> Repository { get; }

        public Context()
        {
            this.prelievi = new List<PrelievoLatteDto>();
            this.Repository = new PrelieviRepository(this.prelievi);
        }

    }
}