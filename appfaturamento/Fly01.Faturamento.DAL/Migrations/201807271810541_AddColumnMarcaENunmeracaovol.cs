namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnMarcaENunmeracaovol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "NumeracaoVolumesTrans", c => c.Int(nullable: false));
            AddColumn("dbo.NotaFiscal", "Marca", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.OrdemVenda", "NumeracaoVolumesTrans", c => c.Int(nullable: false));
            AddColumn("dbo.OrdemVenda", "Marca", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "Marca");
            DropColumn("dbo.OrdemVenda", "NumeracaoVolumesTrans");
            DropColumn("dbo.NotaFiscal", "Marca");
            DropColumn("dbo.NotaFiscal", "NumeracaoVolumesTrans");
        }
    }
}
