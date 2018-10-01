namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNovosImpostosGrupoTributario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GrupoTributario", "RetemISS", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "CalculaCSLL", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "RetemCSLL", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "CalculaINSS", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "RetemINSS", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "CalculaImpostoRenda", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "RetemImpostoRenda", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GrupoTributario", "RetemImpostoRenda");
            DropColumn("dbo.GrupoTributario", "CalculaImpostoRenda");
            DropColumn("dbo.GrupoTributario", "RetemINSS");
            DropColumn("dbo.GrupoTributario", "CalculaINSS");
            DropColumn("dbo.GrupoTributario", "RetemCSLL");
            DropColumn("dbo.GrupoTributario", "CalculaCSLL");
            DropColumn("dbo.GrupoTributario", "RetemISS");
        }
    }
}
