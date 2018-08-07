namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTablesNotaFiscalAndOrdemVenda : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NotaFiscal", "NumeracaoVolumesTrans", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.NotaFiscal", "TipoEspecie", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.OrdemVenda", "NumeracaoVolumesTrans", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.OrdemVenda", "TipoEspecie", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrdemVenda", "TipoEspecie", c => c.Int(nullable: false));
            AlterColumn("dbo.OrdemVenda", "NumeracaoVolumesTrans", c => c.Int(nullable: false));
            AlterColumn("dbo.NotaFiscal", "TipoEspecie", c => c.Int(nullable: false));
            AlterColumn("dbo.NotaFiscal", "NumeracaoVolumesTrans", c => c.Int(nullable: false));
        }
    }
}
