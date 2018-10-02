namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class DeleteIBPT_2018 : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM IBPTNCM");
        }

        public override void Down()
        {
        }
    }
}

