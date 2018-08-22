namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiasPadraoEntregatoDiasPrazoEntrega : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametroOrdemServico", "DiasPrazoEntrega", c => c.Int(nullable: false));
            DropColumn("dbo.ParametroOrdemServico", "DiasPadraoEntrega");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ParametroOrdemServico", "DiasPadraoEntrega", c => c.Int(nullable: false));
            DropColumn("dbo.ParametroOrdemServico", "DiasPrazoEntrega");
        }
    }
}
