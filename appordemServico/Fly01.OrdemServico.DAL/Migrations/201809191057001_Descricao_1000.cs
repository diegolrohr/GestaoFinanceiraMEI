namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Descricao_1000 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrdemServico", "Descricao", c => c.String(maxLength: 1000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrdemServico", "Descricao", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
