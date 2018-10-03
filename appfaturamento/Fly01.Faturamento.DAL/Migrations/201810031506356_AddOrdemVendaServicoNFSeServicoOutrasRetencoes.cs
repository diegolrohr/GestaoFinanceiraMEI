namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrdemVendaServicoNFSeServicoOutrasRetencoes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFSeServico", "ValorOutrasRetencoes", c => c.Double(nullable: false));
            AddColumn("dbo.NFSeServico", "DescricaoOutrasRetencoes", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.OrdemVendaServico", "ValorOutrasRetencoes", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemVendaServico", "DescricaoOutrasRetencoes", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVendaServico", "DescricaoOutrasRetencoes");
            DropColumn("dbo.OrdemVendaServico", "ValorOutrasRetencoes");
            DropColumn("dbo.NFSeServico", "DescricaoOutrasRetencoes");
            DropColumn("dbo.NFSeServico", "ValorOutrasRetencoes");
        }
    }
}
