namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNfeOrdemVendaNFeRefComplementarIsDevolucao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "NFeRefComplementarIsDevolucao", c => c.Boolean(nullable: false));
            AddColumn("dbo.NFe", "TipoNfeComplementar", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.OrdemVenda", "NFeRefComplementarIsDevolucao", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "NFeRefComplementarIsDevolucao");
            DropColumn("dbo.NFe", "TipoNfeComplementar");
            DropColumn("dbo.NotaFiscal", "NFeRefComplementarIsDevolucao");
        }
    }
}
