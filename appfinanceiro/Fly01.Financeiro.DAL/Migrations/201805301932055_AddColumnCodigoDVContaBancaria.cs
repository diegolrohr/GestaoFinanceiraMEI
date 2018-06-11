namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnCodigoDVContaBancaria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContaBancaria", "CodigoDV", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContaBancaria", "CodigoDV");
        }
    }
}
