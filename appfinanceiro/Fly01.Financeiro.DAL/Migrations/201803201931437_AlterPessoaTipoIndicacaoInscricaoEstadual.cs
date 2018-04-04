namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPessoaTipoIndicacaoInscricaoEstadual : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pessoa", "TipoIndicacaoInscricaoEstadual", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pessoa", "TipoIndicacaoInscricaoEstadual");
        }
    }
}
