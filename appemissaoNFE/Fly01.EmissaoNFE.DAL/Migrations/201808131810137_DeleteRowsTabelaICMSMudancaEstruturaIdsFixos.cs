namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRowsTabelaICMSMudancaEstruturaIdsFixos : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM dbo.tabelaICMS");
        }
        
        public override void Down()
        {
        }
    }
}
