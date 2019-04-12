namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDadosExportacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produto", "EXTIPI", c => c.String(maxLength: 3, unicode: false));
            AddColumn("dbo.Pessoa", "PaisId", c => c.Guid());
            AddColumn("dbo.Pessoa", "IdEstrangeiro", c => c.String(maxLength: 20, unicode: false));
            CreateIndex("dbo.Pessoa", "PaisId");
            AddForeignKey("dbo.Pessoa", "PaisId", "dbo.Pais", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pessoa", "PaisId", "dbo.Pais");
            DropIndex("dbo.Pessoa", new[] { "PaisId" });
            DropColumn("dbo.Pessoa", "IdEstrangeiro");
            DropColumn("dbo.Pessoa", "PaisId");
            DropColumn("dbo.Produto", "EXTIPI");
        }
    }
}
