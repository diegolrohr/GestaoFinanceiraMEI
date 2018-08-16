namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPedidoTransporte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedido", "NumeracaoVolumesTrans", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.Pedido", "Marca", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.Pedido", "TipoEspecie", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedido", "TipoEspecie");
            DropColumn("dbo.Pedido", "Marca");
            DropColumn("dbo.Pedido", "NumeracaoVolumesTrans");
        }
    }
}
