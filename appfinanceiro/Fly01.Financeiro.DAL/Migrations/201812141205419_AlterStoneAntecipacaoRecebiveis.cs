namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterStoneAntecipacaoRecebiveis : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoneAntecipacaoRecebiveis", "ValorBruto", c => c.Double(nullable: false));
            AddColumn("dbo.StoneAntecipacaoRecebiveis", "TaxaPontual", c => c.Double(nullable: false));
            AddColumn("dbo.StoneAntecipacaoRecebiveis", "Data", c => c.DateTime(nullable: false));
            AddColumn("dbo.StoneAntecipacaoRecebiveis", "StoneId", c => c.Int(nullable: false));
            AddColumn("dbo.StoneAntecipacaoRecebiveis", "StoneBancoId", c => c.Int(nullable: false));
            AlterColumn("dbo.StoneAntecipacaoRecebiveis", "ValorAntecipado", c => c.Double(nullable: false));
            DropColumn("dbo.StoneAntecipacaoRecebiveis", "ValorRecebido");
            DropColumn("dbo.StoneAntecipacaoRecebiveis", "Taxa");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StoneAntecipacaoRecebiveis", "Taxa", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.StoneAntecipacaoRecebiveis", "ValorRecebido", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.StoneAntecipacaoRecebiveis", "ValorAntecipado", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.StoneAntecipacaoRecebiveis", "StoneBancoId");
            DropColumn("dbo.StoneAntecipacaoRecebiveis", "StoneId");
            DropColumn("dbo.StoneAntecipacaoRecebiveis", "Data");
            DropColumn("dbo.StoneAntecipacaoRecebiveis", "TaxaPontual");
            DropColumn("dbo.StoneAntecipacaoRecebiveis", "ValorBruto");
        }
    }
}
