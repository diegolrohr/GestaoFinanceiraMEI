using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Fly01.OrdemServico.API.Models.DAL;
using Fly01.OrdemServico.DAL.Migrations.DataInitializer;

namespace Fly01.OrdemServico.DAL.Migrations
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
            }
            catch (DbEntityValidationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
