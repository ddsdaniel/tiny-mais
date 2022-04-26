@echo off
title Gerar Build do Tiny Mais

:: Chamar a função que solicita a pasta de saída dos arquivos
call :SelecionarPasta "Selecione a pasta para gerar os arquivos" "C:\Users\Daniel\Downloads"

:: Guardar a hora de início do build
set startTime=%time%

:: Obter a branch atual do GIT
for /f "tokens=*" %%i in ('git rev-parse --abbrev-ref HEAD') do set BRANCH=%%i

:: Dividir a branch atual entre Nível (feature, bugfix, hotfix) e o nome da feature, se houver
for /f "tokens=1 delims=/" %%i in ("%BRANCH%") do set NIVEL=%%i
for /f "tokens=2 delims=/" %%i in ("%BRANCH%") do set FEATURE=%%i

:: Concatenar nome do usuário e branch do GIT para obter a pasta destino
call :ConcatenarNomePasta %USERNAME% %NIVEL% %FEATURE%
set PASTA_DESTINO=%Location%\%PASTA_DESTINO%

:: Remover a pasta destino se ela já existir
if exist "%PASTA_DESTINO%" (
	echo.
	echo #### Removendo pasta "%PASTA_DESTINO%"
	rmdir /s /q "%PASTA_DESTINO%"
)

:: Criar a pasta de destino
echo.
echo #### Criando pasta "%PASTA_DESTINO%"
mkdir "%PASTA_DESTINO%"

:: BUILD DO DOTNET com saída na pasta destino
echo.
echo #### Iniciando build do .NET
dotnet publish ..\back-end-tiny-mais\src\TinyMais.WebAPI\ -c Release --nologo -p:PublishProfile=release-win-x64.pubxml --output "%PASTA_DESTINO%\tiny-mais-api"	

:: Comprimir se possível
if exist "%ProgramFiles%\WinRAR\rar.exe" (
	echo.
	echo #### Comprimindo arquivos para "%PASTA_DESTINO%\TinyMais.rar"
	"%ProgramFiles%\WinRAR\rar.exe" a -ep1 -idq -r -y "%PASTA_DESTINO%\TinyMais.rar" "%PASTA_DESTINO%\*"
)

:: Mostrar o tempo de início e fim do build
echo.
echo Compilacao Finalizada
echo Pasta destino: %PASTA_DESTINO% 
echo Hora inicio   : %startTime% 
echo Hora final    : %time%
echo.

:: Abrir pasta destino
start explorer.exe "%PASTA_DESTINO%"
pause

:: Aguardar uma interação do usuário para fechar o console
goto :eof

:: *****************************************************************************
:: Função que recebe três parâmetros e concatena os valores separando por barra
:: *****************************************************************************
:ConcatenarNomePasta
if [%3]==[] (
	set PASTA_DESTINO=%1\%2
) else (
	set PASTA_DESTINO=%1\%2\%3
)
goto :eof
::*****************************************************************************




:: *****************************************************************************
:: Função que abre um diálogo solicitando que o usuário selecione uma pasta
:: *****************************************************************************
:SelecionarPasta
set pwshcmd=powershell -noprofile -command "&{[System.Reflection.Assembly]::LoadWithPartialName('System.windows.forms') | Out-Null;$FolderBrowserDialog = New-Object System.Windows.Forms.FolderBrowserDialog; $FolderBrowserDialog.Description='%1'; $FolderBrowserDialog.SelectedPath='%2'; $FolderBrowserDialog.ShowDialog()|out-null; $FolderBrowserDialog.SelectedPath}"

for /f "delims=" %%I in ('%pwshcmd%') do set "Location=%%I"
goto :eof
:: *****************************************************************************
