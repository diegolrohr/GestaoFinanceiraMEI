#Update Database Compras
$cmdCompras = Update-Database -ProjectName Fly01.Compras.DAL -StartUpProjectName Fly01.Compras.API -ConfigurationTypeName Fly01.Compras.DAL.Migrations.Configuration -Verbose

#Update Database Estoque
$cmdEstoque = Update-Database -ProjectName Fly01.Estoque.DAL -StartUpProjectName Fly01.Estoque.API -ConfigurationTypeName Fly01.Estoque.DAL.Migrations.Configuration -Verbose

#Update Database Faturamento
$cmdFaturamento = Update-Database -ProjectName Fly01.Faturamento.DAL -StartUpProjectName Fly01.Faturamento.API -ConfigurationTypeName Fly01.Faturamento.DAL.Migrations.Configuration -Verbose

#Update Database Financeiro
$cmdFinanceiro = Update-Database -ProjectName Fly01.Financeiro.DAL -StartUpProjectName Fly01.Financeiro.API -ConfigurationTypeName Fly01.Financeiro.DAL.Migrations.Configuration -Verbose

#Update Database EmissaoNFE
$cmdEmissaoNFE = Update-Database -ProjectName Fly01.EmissaoNFE.DAL -StartUpProjectName Fly01.EmissaoNFE.API -ConfigurationTypeName Fly01.EmissaoNFE.DAL.Migrations.Configuration -Verbose

#Update Database OrdemServico
$cmdOrdemServico = Update-Database -ProjectName Fly01.OrdemServico.DAL -StartUpProjectName Fly01.OrdemServico.API -ConfigurationTypeName Fly01.OrdemServico.DAL.Migrations.Configuration -Verbose