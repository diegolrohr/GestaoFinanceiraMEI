namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFormaPagamentoSefaz : DbMigration
    {
        public override void Up()
        {
            //Ajuste enum TipoFormaPagamento com códigos SEFAZ
            Sql("update FormaPagamento set TipoFormaPagamento = 15 where TipoFormaPagamento = 6");
            Sql("update FormaPagamento set TipoFormaPagamento = 6 where TipoFormaPagamento = 5");
        }
        
        public override void Down()
        {
        }
    }
}
