namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDadosExportacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produto", "EXTIPI", c => c.String(maxLength: 3, unicode: false));
            AddColumn("dbo.NFe", "UFSaidaPaisId", c => c.Guid());
            AddColumn("dbo.NFe", "LocalEmbarque", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.NFe", "LocalDespacho", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.Pessoa", "PaisId", c => c.Guid());
            AddColumn("dbo.Pessoa", "IdEstrangeiro", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.OrdemVenda", "UFSaidaPaisId", c => c.Guid());
            AddColumn("dbo.OrdemVenda", "LocalEmbarque", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.OrdemVenda", "LocalDespacho", c => c.String(maxLength: 60, unicode: false));
            CreateIndex("dbo.Pessoa", "PaisId");
            CreateIndex("dbo.OrdemVenda", "UFSaidaPaisId");
            CreateIndex("dbo.NFe", "UFSaidaPaisId");
            AddForeignKey("dbo.Pessoa", "PaisId", "dbo.Pais", "Id");
            AddForeignKey("dbo.OrdemVenda", "UFSaidaPaisId", "dbo.Estado", "Id");
            AddForeignKey("dbo.NFe", "UFSaidaPaisId", "dbo.Estado", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NFe", "UFSaidaPaisId", "dbo.Estado");
            DropForeignKey("dbo.OrdemVenda", "UFSaidaPaisId", "dbo.Estado");
            DropForeignKey("dbo.Pessoa", "PaisId", "dbo.Pais");
            DropIndex("dbo.NFe", new[] { "UFSaidaPaisId" });
            DropIndex("dbo.OrdemVenda", new[] { "UFSaidaPaisId" });
            DropIndex("dbo.Pessoa", new[] { "PaisId" });
            DropColumn("dbo.OrdemVenda", "LocalDespacho");
            DropColumn("dbo.OrdemVenda", "LocalEmbarque");
            DropColumn("dbo.OrdemVenda", "UFSaidaPaisId");
            DropColumn("dbo.Pessoa", "IdEstrangeiro");
            DropColumn("dbo.Pessoa", "PaisId");
            DropColumn("dbo.NFe", "LocalDespacho");
            DropColumn("dbo.NFe", "LocalEmbarque");
            DropColumn("dbo.NFe", "UFSaidaPaisId");
            DropColumn("dbo.Produto", "EXTIPI");
        }
    }
}
