namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterDescricaoGrupoProdutoTributario : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GrupoProduto", "Descricao", c => c.String(nullable: false, maxLength: 60, unicode: false));
            AlterColumn("dbo.GrupoTributario", "Descricao", c => c.String(nullable: false, maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GrupoTributario", "Descricao", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AlterColumn("dbo.GrupoProduto", "Descricao", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
    }
}
