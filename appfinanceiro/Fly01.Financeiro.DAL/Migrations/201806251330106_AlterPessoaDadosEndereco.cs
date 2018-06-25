namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPessoaDadosEndereco : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pessoa", "Endereco", c => c.String(maxLength: 80, unicode: false));
            AlterColumn("dbo.Pessoa", "Complemento", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.Pessoa", "Bairro", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pessoa", "Bairro", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.Pessoa", "Complemento", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Pessoa", "Endereco", c => c.String(maxLength: 50, unicode: false));
        }
    }
}
