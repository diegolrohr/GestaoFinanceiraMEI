namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterObservacaoDescricaoOS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemServico", "Descricao", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.OrdemServico", "Observacao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdemServico", "Observacao", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.OrdemServico", "Descricao");
        }
    }
}
