namespace Fly01.Estoque.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_Produto_EnquadramentoLegalIpi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produto", "EnquadramentoLegalIPIId", c => c.Guid());
            CreateIndex("dbo.Produto", "EnquadramentoLegalIPIId");
            AddForeignKey("dbo.Produto", "EnquadramentoLegalIPIId", "dbo.EnquadramentoLegalIPI", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Produto", "EnquadramentoLegalIPIId", "dbo.EnquadramentoLegalIPI");
            DropIndex("dbo.Produto", new[] { "EnquadramentoLegalIPIId" });
            DropColumn("dbo.Produto", "EnquadramentoLegalIPIId");
        }
    }
}
