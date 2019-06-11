namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using Fly01.EmissaoNFE.DAL.Migrations.DataInitializer;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            CommandTimeout = 600; //10 minutes timeout
        }

        protected override void Seed(AppDataContext context)
        {
            try
            {
                var estados = new EstadoDataInitializer();
                estados.Initialize(context);
                var ufs = estados.GetLstOfStates();
                new CidadeDataInitializer1().Initialize(context, ufs);
                new CidadeDataInitializer2().Initialize(context, ufs);
                new CidadeDataInitializer3().Initialize(context, ufs);
                new CidadeDataInitializer4().Initialize(context, ufs);
                new CidadeDataInitializer5().Initialize(context, ufs);
                new CidadeDataInitializer6().Initialize(context, ufs);
                new NcmDataInitializer().Initialize(context);
                new TabelaIcmsDataInitializer().Initialize(context);
                new CfopDataInitializer().Initialize(context);
                new NBSDataInitializer().Initialize(context);
                new SiafiDataInitializer().Initialize(context);
                new ResponsavelTecnicoDataInitializer().Initialize(context);
                new PaisDataInitializer().Initialize(context);
            }
            catch (DbEntityValidationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
