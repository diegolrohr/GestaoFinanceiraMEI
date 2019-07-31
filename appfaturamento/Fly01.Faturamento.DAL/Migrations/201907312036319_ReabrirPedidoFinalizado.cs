namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReabrirPedidoFinalizado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVenda", "DataReabertura", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "DataReabertura");
        }
    }
}
