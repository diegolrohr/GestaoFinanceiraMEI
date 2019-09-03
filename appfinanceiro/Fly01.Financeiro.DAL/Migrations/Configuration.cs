using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Fly01.Financeiro.API.Models.DAL;

namespace Fly01.Financeiro.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppDataContext context)
        {
            try
            {
                //   Debugger Seed;
                //  if (System.Diagnostics.Debugger.IsAttached == false)
                //      System.Diagnostics.Debugger.Launch();
                //
                //  new CidadeDataInitializer7().Initialize(context, ufs);

            }
            catch (DbEntityValidationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
