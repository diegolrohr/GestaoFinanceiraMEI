namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableCnabAddColumnPessoaId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cnab", "PessoaId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cnab", "PessoaId");
        }
    }
}
