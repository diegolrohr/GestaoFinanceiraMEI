namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPessoaAlterObservacaoMaxLength500 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pessoa", "Observacao", c => c.String(maxLength: 500, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pessoa", "Observacao", c => c.String(maxLength: 100, unicode: false));
        }
    }
}
