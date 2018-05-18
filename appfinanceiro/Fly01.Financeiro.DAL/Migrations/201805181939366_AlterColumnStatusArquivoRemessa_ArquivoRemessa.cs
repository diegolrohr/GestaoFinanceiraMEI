namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterColumnStatusArquivoRemessa_ArquivoRemessa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ArquivoRemessa", "StatusArquivoRemessa", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ArquivoRemessa", "StatusArquivoRemessa", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
    }
}
