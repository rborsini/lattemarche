using System;
using System.Collections.Generic;
using LatteMarche.Application.PrelieviLatte.Dtos;
using System.Linq;
using System.Text;

namespace LatteMarche.Service.Data
{
    public class PrelieviRepository : IRepository<PrelievoLatteDto, int>
    {

        private List<PrelievoLatteDto> table;

        public PrelieviRepository(List<PrelievoLatteDto> table)
        {
            this.table = table;
        }

        public void Add(PrelievoLatteDto record)
        {
            this.table.Add(record);
        }

        public void Delete(PrelievoLatteDto record)
        {
            PrelievoLatteDto innerRecord = this.GetById(record.Id);
            this.table.Remove(innerRecord);
        }

        public List<PrelievoLatteDto> GetAll()
        {
            return this.table;
        }

        public PrelievoLatteDto GetById(int key)
        {
            return this.table.FirstOrDefault(p => p.Id == key);
        }

        public void Update(PrelievoLatteDto record)
        {
            PrelievoLatteDto innerRecord = this.GetById(record.Id);

            //innerRecord.Allevatore = record.Allevatore;
            //innerRecord.Autista = record.Autista;
            //innerRecord.Quantita = record.Quantita;
            //innerRecord.Data = record.Data;
        }
    }
}
      
