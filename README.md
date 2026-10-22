# Gestión de incidencias en C#

Demostración para Proyecto Parcial I de Programación Avanzada. Incluye una aplicación Windows Forms, especificaciones, pruebas y CI/CD.

## Abrir el programa
1. Abrir GestionIncidencias.sln en Visual Studio 2026.
2. Tener instalada la carga Desarrollo de escritorio de .NET y el targeting pack .NET Framework 4.7.2.
3. Establecer GestionIncidencias como proyecto de inicio y pulsar F5.

frmPrincipal muestra la tabla y las acciones. Nuevo y Modificar abren frmAdmin para ingresar o editar los datos. Consultas filtra registros y muestra el resumen por responsable. Los códigos son automáticos y se muestran como 001, 002, etc.

Los datos quedan en %LOCALAPPDATA%/GestionIncidencias/incidencias.xml. No se incluyen registros personales ni datos precargados. Es un prototipo local, sin usuarios simultáneos ni servidor.

## Compilar y comprobar
En Developer PowerShell de Visual Studio:
```powershell
msbuild GestionIncidencias.sln /t:Rebuild /p:Configuration=Release /warnaserror
& .\Pruebas\bin\Release\Pruebas.exe
```
La consola termina con código 0 si todas las comprobaciones pasan. Es un ejecutor sencillo propio, no un proyecto MSTest. Actions verifica su código de salida.

## Documentación del repositorio
- specs/001-incidencias/spec.md: historias y criterios de aceptación.
- specs/001-incidencias/plan.md: decisiones técnicas y alcance.
- specs/001-incidencias/tasks.md: tareas y pendientes reales.
- .specify/memory/constitution.md: principios usados para redactar las especificaciones.
- .github/copilot-instructions.md: convenciones para Copilot.
- docs/flujo.md: ramas, revisión, entrega y uso propuesto de Spec Kit.

Estos archivos Markdown documentan el flujo exigido por el taller. El informe y la presentación se entregan aparte del código.

## Automatización
El workflow Compilar probar y entregar valida cada push y pull request. Una etiqueta v* publica una release privada con el paquete previamente validado. Un artefacto de Actions es una descarga temporal y una release es una versión distribuible.

También se puede distribuir desde Actions > Compilar probar y entregar > Run workflow, usando main e indicando una versión nueva como v1.0.0. Si se deja la versión vacía, solo se compila, se prueba y se prepara el artefacto.

Para trabajar con el historial remoto, clonar el repositorio privado desde GitHub. La carpeta de entrega contiene el código fuente, mientras GitHub conserva los commits, las ramas y los pull requests.
