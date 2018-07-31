using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.EntityFramework
{
    public abstract class ViewMigration : DbMigration
    {

        public override abstract void Up();

        public virtual void UpView(string viewName)
        {
            if (String.IsNullOrEmpty(viewName))
                return;

            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Scripts\" + viewName + ".sql");
            var selectScript = File.ReadAllText(sqlFile);

            var dropView = $"if exists(select 1 from sys.views where name='{viewName}' and type='v') drop view {viewName}; ";

            Sql(dropView);

            var createView = $"create view {viewName} as {selectScript}";

            Sql(createView);
        }
    }
}

