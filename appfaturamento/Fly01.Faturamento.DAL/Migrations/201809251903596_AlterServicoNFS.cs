namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterServicoNFS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servico", "IssId", c => c.Guid());
            AddColumn("dbo.Servico", "CodigoTributacaoMunicipal", c => c.String(maxLength: 20, unicode: false));
            CreateIndex("dbo.Servico", "IssId");
            AddForeignKey("dbo.Servico", "IssId", "dbo.Iss", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servico", "IssId", "dbo.Iss");
            DropIndex("dbo.Servico", new[] { "IssId" });
            DropColumn("dbo.Servico", "CodigoTributacaoMunicipal");
            DropColumn("dbo.Servico", "IssId");
        }
    }
}
