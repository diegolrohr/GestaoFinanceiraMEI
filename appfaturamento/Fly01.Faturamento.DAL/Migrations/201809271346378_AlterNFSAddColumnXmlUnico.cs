namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNFSAddColumnXmlUnico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFSe", "XMLUnicoTSS", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NFSe", "XMLUnicoTSS");
        }
    }
}
