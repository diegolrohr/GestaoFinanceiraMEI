namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Create_Cest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cest",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 650, unicode: false),
                        Segmento = c.String(maxLength: 200, unicode: false),
                        Item = c.String(maxLength: 200, unicode: false),
                        Anexo = c.String(maxLength: 200, unicode: false),
                        NcmId = c.Guid(),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NCM", t => t.NcmId)
                .Index(t => t.NcmId);
            
            AddColumn("dbo.Produto", "CestId", c => c.Guid());
            CreateIndex("dbo.Produto", "CestId");
            AddForeignKey("dbo.Produto", "CestId", "dbo.Cest", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Produto", "CestId", "dbo.Cest");
            DropForeignKey("dbo.Cest", "NcmId", "dbo.NCM");
            DropIndex("dbo.Produto", new[] { "CestId" });
            DropIndex("dbo.Cest", new[] { "NcmId" });
            DropColumn("dbo.Produto", "CestId");
            DropTable("dbo.Cest");
        }
    }
}
