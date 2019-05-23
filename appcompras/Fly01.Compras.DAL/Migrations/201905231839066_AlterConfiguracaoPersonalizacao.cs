namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterConfiguracaoPersonalizacao : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ConfiguracaoPersonalizacao", "ExibirStepProdutosCompras");
            DropColumn("dbo.ConfiguracaoPersonalizacao", "ExibirStepServicosCompras");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConfiguracaoPersonalizacao", "ExibirStepServicosCompras", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConfiguracaoPersonalizacao", "ExibirStepProdutosCompras", c => c.Boolean(nullable: false));
        }
    }
}
