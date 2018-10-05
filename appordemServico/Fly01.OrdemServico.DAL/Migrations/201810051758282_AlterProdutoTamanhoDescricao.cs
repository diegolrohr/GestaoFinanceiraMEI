namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterProdutoTamanhoDescricao : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Produto", "Descricao", c => c.String(nullable: false, maxLength: 120, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Produto", "Descricao", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
    }
}
