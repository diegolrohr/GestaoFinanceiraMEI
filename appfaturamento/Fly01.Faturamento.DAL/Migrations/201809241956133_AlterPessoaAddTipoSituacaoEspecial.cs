namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPessoaAddTipoSituacaoEspecial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pessoa", "SituacaoEspecialNFS", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pessoa", "SituacaoEspecialNFS");
        }
    }
}
