namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PessoaCampoNumeroEComplemento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pessoa", "Numero", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.Pessoa", "Complemento", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pessoa", "Complemento");
            DropColumn("dbo.Pessoa", "Numero");
        }
    }
}
