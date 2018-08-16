namespace Fly01.Estoque.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterProdutoAddColumnObjetoDeManutencao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produto", "ObjetoDeManutencao", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produto", "ObjetoDeManutencao");
        }
    }
}
