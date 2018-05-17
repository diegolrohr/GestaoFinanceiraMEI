namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArquivoRemessaDropColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ArquivoRemessa", "NumeroArquivo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ArquivoRemessa", "NumeroArquivo", c => c.Int(nullable: false));
        }
    }
}
