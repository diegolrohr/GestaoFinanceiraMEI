namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateConfiguracaoNotificacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConfiguracaoNotificacao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NotificaViaEmail = c.Boolean(nullable: false),
                        NotificaViaSMS = c.Boolean(nullable: false),
                        DiaSemana = c.Int(nullable: false),
                        HoraEnvio = c.Time(nullable: false, precision: 7),
                        ContasAPagar = c.Boolean(nullable: false),
                        ContasAReceber = c.Boolean(nullable: false),
                        EmailDestino = c.String(maxLength: 70, unicode: false),
                        ContatoDestino = c.String(maxLength: 20, unicode: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
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
            DropTable("dbo.ConfiguracaoNotificacao");
        }
    }
}
