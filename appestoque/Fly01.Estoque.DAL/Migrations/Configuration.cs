using Fly01.Estoque.DAL.Migrations.DataInitializer;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Fly01.Estoque.DAL.Migrations
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
                //new EstadoDataInitializer().Initialize(context);
                //new UnidadeMedidaDataInitializer().Initialize(context);
                //new NCMDataInitializer().Initialize(context);
                //new CestDataInitializer().Initialize(context);
                //new EnquadramentoLegalIPIDataInitializer().Initialize(context);
                new NewCestDataInitializer().Initialize(context);

            }
            catch (DbEntityValidationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
