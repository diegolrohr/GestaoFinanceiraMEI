namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPessoa_TamanhoString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pessoa", "Numero", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pessoa", "Numero", c => c.String(maxLength: 20, unicode: false));
        }
    }
}
