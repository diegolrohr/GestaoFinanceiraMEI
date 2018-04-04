Para gerar pacotes nuget, você deve:

1. Baixar o arquivo Nuget.exe através do link: https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
2. Salvar o arquivo Nuget.exe na pasta C:\nuget
3. Editar a variável de ambiente PATH inserindo (e não substituindo) o valor ";C:\nuget"
4. Alterar o arquivo via bloco de notas "Fly01.EmissaoNFE.Domain.nuspec" com a nova <version> e inserir demais observações (caso queira).
4.1 SALVAR
4.2 FECHAR
5. Compile a solução Fly01.EmissaoNFE.Domain
6. Executar o arquivo PackNuget.bat
7. Mover o arquivo com a extensão ".nupkg" com a nova versão para a pasta \\vs1saas01\Packages

!!!ATENÇÃO!!!
No arquivo .nuspec em <files> inserir todos as DLLs dos projetos que estiverem dentro da solution.

Exemplo:
	<file src="..\Fly01.Utils\bin\Debug\Fly01.Utils.dll" target="lib\net45" />
