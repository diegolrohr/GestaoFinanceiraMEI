namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterServicoAddCodIssEspecifico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servico", "CodigoIssEspecifico", c => c.String(maxLength: 20, unicode: false));
            DropColumn("dbo.Servico", "TipoTributacaoISS");
            DropColumn("dbo.Servico", "TipoPagamentoImpostoISS");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servico", "TipoPagamentoImpostoISS", c => c.Int());
            AddColumn("dbo.Servico", "TipoTributacaoISS", c => c.Int());
            DropColumn("dbo.Servico", "CodigoIssEspecifico");
        }
    }
}
