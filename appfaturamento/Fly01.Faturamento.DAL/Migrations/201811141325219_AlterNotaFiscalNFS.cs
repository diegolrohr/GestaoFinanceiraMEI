namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaFiscalNFS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "InformacoesCompletamentaresNFS", c => c.String(maxLength: 1000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotaFiscal", "InformacoesCompletamentaresNFS");
        }
    }
}
