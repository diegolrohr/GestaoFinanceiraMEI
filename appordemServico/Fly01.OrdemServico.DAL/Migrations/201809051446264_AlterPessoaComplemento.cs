namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPessoaComplemento : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pessoa", "Complemento", c => c.String(maxLength: 150, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pessoa", "Complemento", c => c.String(maxLength: 60, unicode: false));
        }
    }
}
