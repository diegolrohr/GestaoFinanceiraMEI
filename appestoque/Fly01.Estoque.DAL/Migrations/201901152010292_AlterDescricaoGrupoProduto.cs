namespace Fly01.Estoque.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterDescricaoGrupoProduto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GrupoProduto", "Descricao", c => c.String(nullable: false, maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GrupoProduto", "Descricao", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
    }
}
