Para gerar pacotes nuget, voc� deve:

1. Baixar o arquivo Nuget.exe atrav�s do link: https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
2. Salvar o arquivo Nuget.exe na pasta C:\nuget
3. Editar a vari�vel de ambiente PATH inserindo (e n�o substituindo) o valor ";C:\nuget"
4. Alterar o arquivo via bloco de notas "Fly01.EmissaoNFE.Domain.nuspec" com a nova <version> e inserir demais observa��es (caso queira).
4.1 SALVAR
4.2 FECHAR
5. Compile a solu��o Fly01.EmissaoNFE.Domain
6. Executar o arquivo PackNuget.bat
7. Mover o arquivo com a extens�o ".nupkg" com a nova vers�o para a pasta \\vs1saas01\Packages

!!!ATEN��O!!!
No arquivo .nuspec em <files> inserir todos as DLLs dos projetos que estiverem dentro da solution.

Exemplo:
	<file src="..\Fly01.Utils\bin\Debug\Fly01.Utils.dll" target="lib\net45" />
