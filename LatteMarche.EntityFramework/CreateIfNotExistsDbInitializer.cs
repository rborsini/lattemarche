using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.EntityFramework
{
    public class CreateIfNotExistsDbInitializer : CreateDatabaseIfNotExists<LatteMarcheDbContext>
    {
        protected override void Seed(LatteMarcheDbContext context)
        {
            base.Seed(context);
        }
    }
}
