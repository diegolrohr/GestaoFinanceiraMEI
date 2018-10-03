namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterServico : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Iss",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropTable("dbo.Iss");
        }
    }
}
