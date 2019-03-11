namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnNumeroArquivoRemessa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArquivoRemessa", "NumeroArquivoRemessa", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArquivoRemessa", "NumeroArquivoRemessa");
        }
    }
}
