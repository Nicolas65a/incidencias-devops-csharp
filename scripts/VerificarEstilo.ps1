param([string]$Raiz = (Split-Path $PSScriptRoot -Parent))
$ErrorActionPreference = 'Stop'
$localizador = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
$msbuild = & $localizador -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild/**/Bin/MSBuild.exe' | Select-Object -First 1
if (!$msbuild)
{
    throw 'No se encontro MSBuild de Visual Studio.'
}
$roslyn = Join-Path (Split-Path $msbuild) 'Roslyn'
if (!(Test-Path -LiteralPath $roslyn))
{
    $roslyn = Join-Path (Split-Path (Split-Path $msbuild)) 'Roslyn'
}
foreach ($nombre in @('System.Runtime.CompilerServices.Unsafe.dll', 'System.Memory.dll', 'System.Buffers.dll', 'System.Numerics.Vectors.dll', 'System.Collections.Immutable.dll', 'System.Reflection.Metadata.dll', 'Microsoft.CodeAnalysis.dll', 'Microsoft.CodeAnalysis.CSharp.dll'))
{
    $biblioteca = Join-Path $roslyn $nombre
    if (Test-Path -LiteralPath $biblioteca)
    {
        [Reflection.Assembly]::LoadFrom($biblioteca) | Out-Null
    }
}
[AppDomain]::CurrentDomain.add_AssemblyResolve({
    param($origen, $solicitud)
    $nombre = ([Reflection.AssemblyName]::new($solicitud.Name)).Name
    foreach ($ensamblado in [AppDomain]::CurrentDomain.GetAssemblies())
    {
        if ($ensamblado.GetName().Name -eq $nombre)
        {
            return $ensamblado
        }
    }
    return $null
})
$errores = [Collections.Generic.List[string]]::new()
$tipos = @('IfStatementSyntax', 'ElseClauseSyntax', 'ForStatementSyntax', 'ForEachStatementSyntax', 'WhileStatementSyntax', 'DoStatementSyntax', 'UsingStatementSyntax', 'LockStatementSyntax', 'FixedStatementSyntax')
$archivos = Get-ChildItem -LiteralPath $Raiz -Filter '*.cs' -Recurse | Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' }
foreach ($archivo in $archivos)
{
    $arbol = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText([IO.File]::ReadAllText($archivo.FullName))
    foreach ($nodo in $arbol.GetRoot().DescendantNodes())
    {
        if ($tipos -contains $nodo.GetType().Name)
        {
            if ($nodo.Statement.GetType().Name -ne 'BlockSyntax')
            {
                $linea = $nodo.GetLocation().GetLineSpan().StartLinePosition.Line + 1
                $errores.Add($archivo.Name + ':' + $linea + ' requiere un bloque con llaves.')
            }
        }
        if ($nodo.GetType().Name -eq 'MethodDeclarationSyntax' -and $null -ne $nodo.ExpressionBody)
        {
            $errores.Add($archivo.Name + ': el metodo ' + $nodo.Identifier.Text + ' debe usar cuerpo con llaves.')
        }
    }
}
if ($errores.Count -gt 0)
{
    $errores | Write-Output
    exit 1
}
Write-Output ('CA14: ' + $archivos.Count + ' archivos C# revisados. Estructuras de control y metodos con bloques correctos.')
