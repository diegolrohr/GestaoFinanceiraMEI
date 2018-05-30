namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableArquivoRemessaColumnBancoId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArquivoRemessa", "BancoId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ArquivoRemessa", "BancoId");
            AddForeignKey("dbo.ArquivoRemessa", "BancoId", "dbo.Banco", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArquivoRemessa", "BancoId", "dbo.Banco");
            DropIndex("dbo.ArquivoRemessa", new[] { "BancoId" });
            DropColumn("dbo.ArquivoRemessa", "BancoId");
        }
    }
}
