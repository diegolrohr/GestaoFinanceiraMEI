# Compras

```PowerShell
Update-Database -ProjectName Fly01.Compras.DAL -StartUpProjectName Fly01.Compras.API -ConfigurationTypeName Fly01.Compras.DAL.Migrations.Configuration -Verbose
```

```PowerShell
Add-Migration -ProjectName Fly01.Compras.DAL -StartUpProjectName Fly01.Compras.API -ConfigurationTypeName Fly01.Compras.DAL.Migrations.Configuration -Name MigrationName
```