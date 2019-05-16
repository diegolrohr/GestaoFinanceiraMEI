using Fly01.OrdemServico.DAL.Migrations.DataInitializer;
using System.Data.Entity.Migrations;

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
            //new NCMDataInitializer().Initialize(context);
            //new NBSDataInitializer().Initialize(context);
            //new UnidadeMedidaDataInitializer().Initialize(context);

            //var estados = new EstadoDataInitializer();
            //estados.Initialize(context);
            //var ufs = estados.GetLstOfStates();
            //new CidadeDataInitializer1().Initialize(context, ufs);
            //new CidadeDataInitializer2().Initialize(context, ufs);
            //new CidadeDataInitializer3().Initialize(context, ufs);
            //new CidadeDataInitializer4().Initialize(context, ufs);
            //new CidadeDataInitializer5().Initialize(context, ufs);
            //new CidadeDataInitializer6().Initialize(context, ufs);
            //new CidadeDataInitializer7().Initialize(context, ufs);
            //new CestDataInitializer().Initialize(context);
            //new EnquadramentoLegalIPIDataInitializer().Initialize(context);
            //new ISSDataInitializer().Initialize(context);
            //new NewCestDataInitializer().Initialize(context);
            //new PaisDataInitializer().Initialize(context);
        }
    }
}
