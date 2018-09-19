namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPessoaTamanhoCampos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pessoa", "Nome", c => c.String(nullable: false, maxLength: 180, unicode: false));
            AlterColumn("dbo.Pessoa", "Endereco", c => c.String(maxLength: 180, unicode: false));
            AlterColumn("dbo.Pessoa", "Complemento", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Pessoa", "Bairro", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Pessoa", "Contato", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.Pessoa", "Email", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Pessoa", "NomeComercial", c => c.String(maxLength: 180, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pessoa", "NomeComercial", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Pessoa", "Email", c => c.String(maxLength: 70, unicode: false));
            AlterColumn("dbo.Pessoa", "Contato", c => c.String(maxLength: 45, unicode: false));
            AlterColumn("dbo.Pessoa", "Bairro", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.Pessoa", "Complemento", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("dbo.Pessoa", "Endereco", c => c.String(maxLength: 80, unicode: false));
            AlterColumn("dbo.Pessoa", "Nome", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
    }
}
