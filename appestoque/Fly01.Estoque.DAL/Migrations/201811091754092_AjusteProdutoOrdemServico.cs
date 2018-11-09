namespace Fly01.Estoque.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteProdutoOrdemServico : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Produto", "ObjetoDeManutencao", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Produto", "ObjetoDeManutencao", c => c.Boolean(nullable: false));
        }
    }
}
