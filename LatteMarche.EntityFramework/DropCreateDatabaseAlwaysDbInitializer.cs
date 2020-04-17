using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.EntityFramework
{
    public class DropCreateDatabaseAlwaysDbInitializer : DropCreateDatabaseAlways<LatteMarcheDbContext>
    {

        public override void InitializeDatabase(LatteMarcheDbContext context)
        {
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "ALTER DATABASE [" + context.Database.Connection.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");

            base.InitializeDatabase(context);
        }

        protected override void Seed(LatteMarcheDbContext context)
        {
            base.Seed(context);
        }
    }
}
