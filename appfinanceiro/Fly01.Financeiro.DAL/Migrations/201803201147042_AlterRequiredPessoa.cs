namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterRequiredPessoa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pessoa", "InscricaoEstadual", c => c.String(maxLength: 18, unicode: false));
            AddColumn("dbo.Pessoa", "InscricaoMunicipal", c => c.String(maxLength: 18, unicode: false));
            AddColumn("dbo.Pessoa", "ConsumidorFinal", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Pessoa", "TipoDocumento", c => c.String(maxLength: 1, unicode: false));
            AlterColumn("dbo.Pessoa", "CPFCNPJ", c => c.String(maxLength: 18, unicode: false));
            AlterColumn("dbo.Pessoa", "NomeComercial", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pessoa", "NomeComercial", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.Pessoa", "CPFCNPJ", c => c.String(nullable: false, maxLength: 18, unicode: false));
            AlterColumn("dbo.Pessoa", "TipoDocumento", c => c.String(nullable: false, maxLength: 1, unicode: false));
            DropColumn("dbo.Pessoa", "ConsumidorFinal");
            DropColumn("dbo.Pessoa", "InscricaoMunicipal");
            DropColumn("dbo.Pessoa", "InscricaoEstadual");
        }
    }
}
