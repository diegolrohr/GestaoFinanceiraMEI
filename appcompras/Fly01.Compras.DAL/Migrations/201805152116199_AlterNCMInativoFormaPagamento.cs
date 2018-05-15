namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNCMInativoFormaPagamento : DbMigration
    {
        public override void Up()
        {
            Sql("update NCM set ativo = 0 where Codigo = '02109900'");

            //Ajuste enum TipoFormaPagamento com códigos SEFAZ
            Sql("update FormaPagamento set TipoFormaPagamento = 15 where TipoFormaPagamento = 6");
            Sql("update FormaPagamento set TipoFormaPagamento = 6 where TipoFormaPagamento = 5");
        }
        
        public override void Down()
        {
        }
    }
}
