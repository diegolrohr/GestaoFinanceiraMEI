namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTamanhoDescricaoCategoria : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categoria", "Descricao", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categoria", "Descricao", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
    }
}
