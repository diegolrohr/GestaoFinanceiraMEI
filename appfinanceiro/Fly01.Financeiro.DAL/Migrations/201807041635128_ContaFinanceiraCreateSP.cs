namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContaFinanceiraCreateSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.ContaReceber_Insert",
                p => new
                    {
                        Id = p.Guid(),
                        ContaFinanceiraRepeticaoPaiId = p.Guid(),
                        ValorPrevisto = p.Double(),
                        ValorPago = p.Double(),
                        CategoriaId = p.Guid(),
                        CondicaoParcelamentoId = p.Guid(),
                        PessoaId = p.Guid(),
                        DataEmissao = p.DateTime(storeType: "date"),
                        DataVencimento = p.DateTime(storeType: "date"),
                        Descricao = p.String(maxLength: 200, unicode: false),
                        Observacao = p.String(maxLength: 200, unicode: false),
                        FormaPagamentoId = p.Guid(),
                        TipoContaFinanceira = p.Int(),
                        StatusContaBancaria = p.Int(),
                        Repetir = p.Boolean(),
                        TipoPeriodicidade = p.Int(),
                        NumeroRepeticoes = p.Int(),
                        DescricaoParcela = p.String(maxLength: 200, unicode: false),
                        Numero = p.Int(),
                        DataDesconto = p.DateTime(storeType: "date"),
                        ValorDesconto = p.Double(),
                        PlataformaId = p.String(maxLength: 200, unicode: false),
                        RegistroFixo = p.Boolean(),
                        DataInclusao = p.DateTime(),
                        DataAlteracao = p.DateTime(),
                        DataExclusao = p.DateTime(),
                        UsuarioInclusao = p.String(maxLength: 200, unicode: false),
                        UsuarioAlteracao = p.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = p.String(maxLength: 200, unicode: false),
                        Ativo = p.Boolean(),
                    },
                body:
                    @"SELECT @Numero = ISNULL(MAX(Numero), 0) + 1 FROM ContaFinanceira WHERE PlataformaId = @PlataformaId AND TipoContafinanceira = 2;

                      INSERT [dbo].[ContaFinanceira]([Id], [ContaFinanceiraRepeticaoPaiId], [ValorPrevisto], [ValorPago], [CategoriaId], [CondicaoParcelamentoId], [PessoaId], [DataEmissao], [DataVencimento], [Descricao], [Observacao], [FormaPagamentoId], [TipoContaFinanceira], [StatusContaBancaria], [Repetir], [TipoPeriodicidade], [NumeroRepeticoes], [DescricaoParcela], [Numero], [DataDesconto], [ValorDesconto], [PlataformaId], [RegistroFixo], [DataInclusao], [DataAlteracao], [DataExclusao], [UsuarioInclusao], [UsuarioAlteracao], [UsuarioExclusao], [Ativo])
                      VALUES (@Id, @ContaFinanceiraRepeticaoPaiId, @ValorPrevisto, @ValorPago, @CategoriaId, @CondicaoParcelamentoId, @PessoaId, @DataEmissao, @DataVencimento, @Descricao, @Observacao, @FormaPagamentoId, @TipoContaFinanceira, @StatusContaBancaria, @Repetir, @TipoPeriodicidade, @NumeroRepeticoes, @DescricaoParcela, @Numero, @DataDesconto, @ValorDesconto, @PlataformaId, @RegistroFixo, @DataInclusao, @DataAlteracao, @DataExclusao, @UsuarioInclusao, @UsuarioAlteracao, @UsuarioExclusao, @Ativo)
                      
                      INSERT [dbo].[ContaReceber]([Id])
                      VALUES (@Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContaReceber_Update",
                p => new
                    {
                        Id = p.Guid(),
                        ContaFinanceiraRepeticaoPaiId = p.Guid(),
                        ValorPrevisto = p.Double(),
                        ValorPago = p.Double(),
                        CategoriaId = p.Guid(),
                        CondicaoParcelamentoId = p.Guid(),
                        PessoaId = p.Guid(),
                        DataEmissao = p.DateTime(storeType: "date"),
                        DataVencimento = p.DateTime(storeType: "date"),
                        Descricao = p.String(maxLength: 200, unicode: false),
                        Observacao = p.String(maxLength: 200, unicode: false),
                        FormaPagamentoId = p.Guid(),
                        TipoContaFinanceira = p.Int(),
                        StatusContaBancaria = p.Int(),
                        Repetir = p.Boolean(),
                        TipoPeriodicidade = p.Int(),
                        NumeroRepeticoes = p.Int(),
                        DescricaoParcela = p.String(maxLength: 200, unicode: false),
                        Numero = p.Int(),
                        DataDesconto = p.DateTime(storeType: "date"),
                        ValorDesconto = p.Double(),
                        PlataformaId = p.String(maxLength: 200, unicode: false),
                        RegistroFixo = p.Boolean(),
                        DataInclusao = p.DateTime(),
                        DataAlteracao = p.DateTime(),
                        DataExclusao = p.DateTime(),
                        UsuarioInclusao = p.String(maxLength: 200, unicode: false),
                        UsuarioAlteracao = p.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = p.String(maxLength: 200, unicode: false),
                        Ativo = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[ContaFinanceira]
                      SET [ContaFinanceiraRepeticaoPaiId] = @ContaFinanceiraRepeticaoPaiId, [ValorPrevisto] = @ValorPrevisto, [ValorPago] = @ValorPago, [CategoriaId] = @CategoriaId, [CondicaoParcelamentoId] = @CondicaoParcelamentoId, [PessoaId] = @PessoaId, [DataEmissao] = @DataEmissao, [DataVencimento] = @DataVencimento, [Descricao] = @Descricao, [Observacao] = @Observacao, [FormaPagamentoId] = @FormaPagamentoId, [TipoContaFinanceira] = @TipoContaFinanceira, [StatusContaBancaria] = @StatusContaBancaria, [Repetir] = @Repetir, [TipoPeriodicidade] = @TipoPeriodicidade, [NumeroRepeticoes] = @NumeroRepeticoes, [DescricaoParcela] = @DescricaoParcela, [DataDesconto] = @DataDesconto, [ValorDesconto] = @ValorDesconto, [PlataformaId] = @PlataformaId, [RegistroFixo] = @RegistroFixo, [DataInclusao] = @DataInclusao, [DataAlteracao] = @DataAlteracao, [DataExclusao] = @DataExclusao, [UsuarioInclusao] = @UsuarioInclusao, [UsuarioAlteracao] = @UsuarioAlteracao, [UsuarioExclusao] = @UsuarioExclusao, [Ativo] = @Ativo
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContaReceber_Delete",
                p => new
                    {
                        Id = p.Guid(),
                    },
                body:
                    @"DELETE [dbo].[ContaReceber]
                      WHERE ([Id] = @Id)
                      
                      DELETE [dbo].[ContaFinanceira]
                      WHERE ([Id] = @Id)
                      AND @@ROWCOUNT > 0"
            );
            
            CreateStoredProcedure(
                "dbo.ContaPagar_Insert",
                p => new
                    {
                        Id = p.Guid(),
                        ContaFinanceiraRepeticaoPaiId = p.Guid(),
                        ValorPrevisto = p.Double(),
                        ValorPago = p.Double(),
                        CategoriaId = p.Guid(),
                        CondicaoParcelamentoId = p.Guid(),
                        PessoaId = p.Guid(),
                        DataEmissao = p.DateTime(storeType: "date"),
                        DataVencimento = p.DateTime(storeType: "date"),
                        Descricao = p.String(maxLength: 200, unicode: false),
                        Observacao = p.String(maxLength: 200, unicode: false),
                        FormaPagamentoId = p.Guid(),
                        TipoContaFinanceira = p.Int(),
                        StatusContaBancaria = p.Int(),
                        Repetir = p.Boolean(),
                        TipoPeriodicidade = p.Int(),
                        NumeroRepeticoes = p.Int(),
                        DescricaoParcela = p.String(maxLength: 200, unicode: false),
                        Numero = p.Int(),
                        DataDesconto = p.DateTime(storeType: "date"),
                        ValorDesconto = p.Double(),
                        PlataformaId = p.String(maxLength: 200, unicode: false),
                        RegistroFixo = p.Boolean(),
                        DataInclusao = p.DateTime(),
                        DataAlteracao = p.DateTime(),
                        DataExclusao = p.DateTime(),
                        UsuarioInclusao = p.String(maxLength: 200, unicode: false),
                        UsuarioAlteracao = p.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = p.String(maxLength: 200, unicode: false),
                        Ativo = p.Boolean(),
                    },
                body:
                    @"SELECT @Numero = ISNULL(MAX(Numero), 0) + 1 FROM ContaFinanceira WHERE PlataformaId = @PlataformaId AND TipoContafinanceira = 1;

                      INSERT [dbo].[ContaFinanceira]([Id], [ContaFinanceiraRepeticaoPaiId], [ValorPrevisto], [ValorPago], [CategoriaId], [CondicaoParcelamentoId], [PessoaId], [DataEmissao], [DataVencimento], [Descricao], [Observacao], [FormaPagamentoId], [TipoContaFinanceira], [StatusContaBancaria], [Repetir], [TipoPeriodicidade], [NumeroRepeticoes], [DescricaoParcela], [Numero], [DataDesconto], [ValorDesconto], [PlataformaId], [RegistroFixo], [DataInclusao], [DataAlteracao], [DataExclusao], [UsuarioInclusao], [UsuarioAlteracao], [UsuarioExclusao], [Ativo])
                      VALUES (@Id, @ContaFinanceiraRepeticaoPaiId, @ValorPrevisto, @ValorPago, @CategoriaId, @CondicaoParcelamentoId, @PessoaId, @DataEmissao, @DataVencimento, @Descricao, @Observacao, @FormaPagamentoId, @TipoContaFinanceira, @StatusContaBancaria, @Repetir, @TipoPeriodicidade, @NumeroRepeticoes, @DescricaoParcela, @Numero, @DataDesconto, @ValorDesconto, @PlataformaId, @RegistroFixo, @DataInclusao, @DataAlteracao, @DataExclusao, @UsuarioInclusao, @UsuarioAlteracao, @UsuarioExclusao, @Ativo)
                      
                      INSERT [dbo].[ContaPagar]([Id])
                      VALUES (@Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContaPagar_Update",
                p => new
                    {
                        Id = p.Guid(),
                        ContaFinanceiraRepeticaoPaiId = p.Guid(),
                        ValorPrevisto = p.Double(),
                        ValorPago = p.Double(),
                        CategoriaId = p.Guid(),
                        CondicaoParcelamentoId = p.Guid(),
                        PessoaId = p.Guid(),
                        DataEmissao = p.DateTime(storeType: "date"),
                        DataVencimento = p.DateTime(storeType: "date"),
                        Descricao = p.String(maxLength: 200, unicode: false),
                        Observacao = p.String(maxLength: 200, unicode: false),
                        FormaPagamentoId = p.Guid(),
                        TipoContaFinanceira = p.Int(),
                        StatusContaBancaria = p.Int(),
                        Repetir = p.Boolean(),
                        TipoPeriodicidade = p.Int(),
                        NumeroRepeticoes = p.Int(),
                        DescricaoParcela = p.String(maxLength: 200, unicode: false),
                        Numero = p.Int(),
                        DataDesconto = p.DateTime(storeType: "date"),
                        ValorDesconto = p.Double(),
                        PlataformaId = p.String(maxLength: 200, unicode: false),
                        RegistroFixo = p.Boolean(),
                        DataInclusao = p.DateTime(),
                        DataAlteracao = p.DateTime(),
                        DataExclusao = p.DateTime(),
                        UsuarioInclusao = p.String(maxLength: 200, unicode: false),
                        UsuarioAlteracao = p.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = p.String(maxLength: 200, unicode: false),
                        Ativo = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[ContaFinanceira]
                      SET [ContaFinanceiraRepeticaoPaiId] = @ContaFinanceiraRepeticaoPaiId, [ValorPrevisto] = @ValorPrevisto, [ValorPago] = @ValorPago, [CategoriaId] = @CategoriaId, [CondicaoParcelamentoId] = @CondicaoParcelamentoId, [PessoaId] = @PessoaId, [DataEmissao] = @DataEmissao, [DataVencimento] = @DataVencimento, [Descricao] = @Descricao, [Observacao] = @Observacao, [FormaPagamentoId] = @FormaPagamentoId, [TipoContaFinanceira] = @TipoContaFinanceira, [StatusContaBancaria] = @StatusContaBancaria, [Repetir] = @Repetir, [TipoPeriodicidade] = @TipoPeriodicidade, [NumeroRepeticoes] = @NumeroRepeticoes, [DescricaoParcela] = @DescricaoParcela, [DataDesconto] = @DataDesconto, [ValorDesconto] = @ValorDesconto, [PlataformaId] = @PlataformaId, [RegistroFixo] = @RegistroFixo, [DataInclusao] = @DataInclusao, [DataAlteracao] = @DataAlteracao, [DataExclusao] = @DataExclusao, [UsuarioInclusao] = @UsuarioInclusao, [UsuarioAlteracao] = @UsuarioAlteracao, [UsuarioExclusao] = @UsuarioExclusao, [Ativo] = @Ativo
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContaPagar_Delete",
                p => new
                    {
                        Id = p.Guid(),
                    },
                body:
                    @"DELETE [dbo].[ContaPagar]
                      WHERE ([Id] = @Id)
                      
                      DELETE [dbo].[ContaFinanceira]
                      WHERE ([Id] = @Id)
                      AND @@ROWCOUNT > 0"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ContaPagar_Delete");
            DropStoredProcedure("dbo.ContaPagar_Update");
            DropStoredProcedure("dbo.ContaPagar_Insert");
            DropStoredProcedure("dbo.ContaReceber_Delete");
            DropStoredProcedure("dbo.ContaReceber_Update");
            DropStoredProcedure("dbo.ContaReceber_Insert");
        }
    }
}
