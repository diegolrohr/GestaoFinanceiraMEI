using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Financeiro.DAL.Migrations.DataInitializer;

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

                //new BancoDataInitializer().Initialize(context);

              //  var estados = new EstadoDataInitializer();
              //  estados.Initialize(context);
              //  var ufs = estados.GetLstOfStates();
              //  new CidadeDataInitializer1().Initialize(context, ufs);
              //  new CidadeDataInitializer2().Initialize(context, ufs);
              //  new CidadeDataInitializer3().Initialize(context, ufs);
              //  new CidadeDataInitializer4().Initialize(context, ufs);
              //  new CidadeDataInitializer5().Initialize(context, ufs);
              //  new CidadeDataInitializer6().Initialize(context, ufs);
              //  new CidadeDataInitializer7().Initialize(context, ufs);

            }
            catch (DbEntityValidationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
