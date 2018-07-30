namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnTipoEspecieNFe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "TipoEspecie", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotaFiscal", "TipoEspecie");
        }
    }
}
