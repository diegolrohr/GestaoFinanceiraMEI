namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterNCMInativo : DbMigration
    {
        public override void Up()
        {
            Sql("update NCM set ativo = 0 where Codigo = '02109900'");
        }
        
        public override void Down()
        {
        }
    }
}
