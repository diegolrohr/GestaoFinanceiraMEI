namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotaFiscalItemTributacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotaFiscalItemTributacao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NotaFiscalItemId = c.Guid(nullable: false),
                        FreteValorFracionado = c.Double(nullable: false),
                        ICMSBase = c.Double(nullable: false),
                        ICMSAliquota = c.Double(nullable: false),
                        ICMSValor = c.Double(nullable: false),
                        IPIBase = c.Double(nullable: false),
                        IPIAliquota = c.Double(nullable: false),
                        IPIValor = c.Double(nullable: false),
                        STBase = c.Double(nullable: false),
                        STAliquota = c.Double(nullable: false),
                        STValor = c.Double(nullable: false),
                        FCPBase = c.Double(nullable: false),
                        FCPAliquota = c.Double(nullable: false),
                        FCPValor = c.Double(nullable: false),
                        COFINSBase = c.Double(nullable: false),
                        COFINSAliquota = c.Double(nullable: false),
                        COFINSValor = c.Double(nullable: false),
                        PISBase = c.Double(nullable: false),
                        PISAliquota = c.Double(nullable: false),
                        PISValor = c.Double(nullable: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalItem", t => t.NotaFiscalItemId)
                .Index(t => t.NotaFiscalItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotaFiscalItemTributacao", "NotaFiscalItemId", "dbo.NotaFiscalItem");
            DropIndex("dbo.NotaFiscalItemTributacao", new[] { "NotaFiscalItemId" });
            DropTable("dbo.NotaFiscalItemTributacao");
        }
    }
}
