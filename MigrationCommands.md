# Compras

```PowerShell
Update-Database -ProjectName Fly01.Compras.DAL -StartUpProjectName Fly01.Compras.API -ConfigurationTypeName Fly01.Compras.DAL.Migrations.Configuration -Verbose
```

```PowerShell
Add-Migration -ProjectName Fly01.Compras.DAL -StartUpProjectName Fly01.Compras.API -ConfigurationTypeName Fly01.Compras.DAL.Migrations.Configuration -Name MigrationName
```

# Estoque

```PowerShell
Update-Database -ProjectName Fly01.Estoque.DAL -StartUpProjectName Fly01.Estoque.API -ConfigurationTypeName Fly01.Estoque.DAL.Migrations.Configuration -Verbose
```

```PowerShell
Add-Migration -ProjectName Fly01.Estoque.DAL -StartUpProjectName Fly01.Estoque.API -ConfigurationTypeName Fly01.Estoque.DAL.Migrations.Configuration -Name MigrationName
```