namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ArquivoCamposRequeridos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Arquivo", "Descricao", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Arquivo", "Conteudo", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.Arquivo", "Md5", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.Arquivo", "Cadastro", c => c.String(nullable: false, maxLength: 30, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Arquivo", "Cadastro", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.Arquivo", "Md5", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Arquivo", "Conteudo", c => c.String(unicode: false));
            AlterColumn("dbo.Arquivo", "Descricao", c => c.String(maxLength: 50, unicode: false));
        }
    }
}
