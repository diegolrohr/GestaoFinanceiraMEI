namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConfiguracaoPersonalizacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConfiguracaoPersonalizacao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EmiteNotaFiscal = c.Boolean(nullable: false, defaultValue: true),
                        ExibirStepProdutosVendas = c.Boolean(nullable: false, defaultValue: true),
                        ExibirStepProdutosCompras = c.Boolean(nullable: false, defaultValue: true),
                        ExibirStepServicosVendas = c.Boolean(nullable: false, defaultValue: true),
                        ExibirStepServicosCompras = c.Boolean(nullable: false, defaultValue: true),
                        ExibirStepTransportadoraVendas = c.Boolean(nullable: false, defaultValue: true),
                        ExibirStepTransportadoraCompras = c.Boolean(nullable: false, defaultValue: true),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        RegistroFixo = c.Boolean(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ConfiguracaoPersonalizacao");
        }
    }
}
