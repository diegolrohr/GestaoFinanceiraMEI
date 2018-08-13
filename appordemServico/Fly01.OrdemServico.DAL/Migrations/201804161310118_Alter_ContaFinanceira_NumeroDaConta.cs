namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Text;

    public partial class Alter_ContaFinanceira_NumeroDaConta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContaFinanceira", "Numero", c => c.Int(nullable: false));

            StringBuilder query = new StringBuilder();

            query.AppendLine("DECLARE @TransactionName varchar(20) = 'Transaction1'; ");
            query.AppendLine("BEGIN TRAN @TransactionName ");
            query.AppendLine("DECLARE @PlatId varchar(200) ");
            query.AppendLine("DECLARE MY_CURSOR CURSOR FOR ");
            query.AppendLine("SELECT DISTINCT PlataformaId ");
            query.AppendLine("FROM ContaFinanceira ");
            query.AppendLine("OPEN MY_CURSOR ");
            query.AppendLine("FETCH NEXT FROM MY_CURSOR INTO @PlatId ");
            query.AppendLine("WHILE @@FETCH_STATUS = 0 ");
            query.AppendLine("BEGIN ");

            //Atualização dos números de Contas a Receber
            query.AppendLine("; with ordemCR as ");
            query.AppendLine("(Select CF.Id Id, Numero, DataInclusao, ROW_NUMBER() over(Order by DataInclusao) numConta ");
            query.AppendLine("from ContaFinanceira CF inner join ContaReceber CR on CF.Id = CR.Id ");
            query.AppendLine("where CF.PlataformaId = @PlatId) ");
            query.AppendLine("update ContaFinanceira set Numero = O.numConta ");
            query.AppendLine("FROM ContaFinanceira C ");
            query.AppendLine("INNER JOIN ordemCR O ON O.Id = C.Id ");

            //Atualização dos números de Contas a Pagar
            query.AppendLine("; with ordemCP as ");
            query.AppendLine("(Select CF.Id Id, Numero, DataInclusao, ROW_NUMBER() over(Order by DataInclusao) numConta ");
            query.AppendLine("from ContaFinanceira CF inner join ContaPagar CP on CF.Id = CP.Id ");
            query.AppendLine("where CF.PlataformaId = @PlatId) ");
            query.AppendLine("update ContaFinanceira set Numero = O.numConta ");
            query.AppendLine("FROM ContaFinanceira C ");
            query.AppendLine("INNER JOIN ordemCP O ON O.Id = C.Id ");
            
            query.AppendLine("FETCH NEXT FROM MY_CURSOR INTO @PlatId ");
            query.AppendLine("END ");
            query.AppendLine("CLOSE MY_CURSOR ");
            query.AppendLine("DEALLOCATE MY_CURSOR ");
            query.AppendLine("COMMIT TRAN @TransactionName; ");

            Sql(query.ToString());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContaFinanceira", "Numero");
        }
    }
}
