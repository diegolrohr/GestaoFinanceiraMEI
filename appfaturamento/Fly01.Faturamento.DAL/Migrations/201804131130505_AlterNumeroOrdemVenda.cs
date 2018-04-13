namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNumeroOrdemVenda : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrdemVenda", "Numero", c => c.Int(nullable: false));
            AddColumn("dbo.OrdemVenda", "NumeroAux", c => c.Int(nullable: false));
            Sql("update OrdemVenda set NumeroAux = Numero");
            DropColumn("dbo.OrdemVenda", "Numero");
            RenameColumn(table: "dbo.OrdemVenda", name: "NumeroAux", newName: "Numero");
        }

        public override void Down()
        {
            AlterColumn("dbo.OrdemVenda", "Numero", c => c.Int(nullable: false, identity: true));
        }
    }
}
