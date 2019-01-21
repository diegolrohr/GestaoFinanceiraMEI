namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterStone : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Stone", newName: "StoneAntecipacaoRecebiveis");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.StoneAntecipacaoRecebiveis", newName: "Stone");
        }
    }
}
