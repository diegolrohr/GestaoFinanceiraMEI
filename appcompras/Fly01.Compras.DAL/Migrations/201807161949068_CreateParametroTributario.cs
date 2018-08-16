namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateParametroTributario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParametroTributario",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SimplesNacional = c.Boolean(nullable: false),
                        AliquotaSimplesNacional = c.Double(nullable: false),
                        AliquotaFCP = c.Double(nullable: false),
                        AliquotaISS = c.Double(nullable: false),
                        AliquotaPISPASEP = c.Double(nullable: false),
                        AliquotaCOFINS = c.Double(nullable: false),
                        RegistroSimplificadoMT = c.Boolean(nullable: false),
                        MensagemPadraoNota = c.String(maxLength: 5000, unicode: false),
                        NumeroRetornoNF = c.String(maxLength: 200, unicode: false),
                        TipoAmbiente = c.Int(nullable: false),
                        TipoVersaoNFe = c.Int(nullable: false),
                        TipoModalidade = c.Int(nullable: false),
                        Cnpj = c.String(maxLength: 16, unicode: false),
                        InscricaoEstadual = c.String(maxLength: 18, unicode: false),
                        UF = c.String(maxLength: 2, unicode: false),
                        TipoPresencaComprador = c.Int(nullable: false),
                        HorarioVerao = c.Int(nullable: false),
                        TipoHorario = c.Int(nullable: false),
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
            DropTable("dbo.ParametroTributario");
        }
    }
}
