# Rodar UpdateDatabase em Todos os Bancos

```PowerShell
.\RunUpdateDatabase.ps1
```
# Financeiro

```PowerShell
Update-Database -ProjectName MEI.Financeiro.DAL -StartUpProjectName MEI.Financeiro.API -ConfigurationTypeName MEI.Financeiro.DAL.Migrations.Configuration -Verbose
```

```PowerShell
Add-Migration -ProjectName MEI.Financeiro.DAL -StartUpProjectName MEI.Financeiro.API -ConfigurationTypeName MEI.Financeiro.DAL.Migrations.Configuration -Name MigrationName
```
