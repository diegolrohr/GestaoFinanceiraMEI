namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdemServico_Cliente : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.OrdemServico", name: "PessoaId", newName: "ClienteId");
            RenameIndex(table: "dbo.OrdemServico", name: "IX_PessoaId", newName: "IX_ClienteId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.OrdemServico", name: "IX_ClienteId", newName: "IX_PessoaId");
            RenameColumn(table: "dbo.OrdemServico", name: "ClienteId", newName: "PessoaId");
        }
    }
}
