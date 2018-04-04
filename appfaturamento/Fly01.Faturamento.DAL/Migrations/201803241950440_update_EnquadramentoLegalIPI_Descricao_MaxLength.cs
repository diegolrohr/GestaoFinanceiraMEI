namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_EnquadramentoLegalIPI_Descricao_MaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EnquadramentoLegalIPI", "Descricao", c => c.String(maxLength: 600, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EnquadramentoLegalIPI", "Descricao", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
