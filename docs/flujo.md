# Flujo de trabajo y demostración

## Trabajo diario
1. Convertir un requisito en un Issue con historia y criterios de aceptación.
2. Crear una rama pequeña desde main: feature/nombre-del-cambio.
3. Escribir una prueba que falle por la regla pendiente y ejecutar las pruebas.
4. Implementar la regla y volver a ejecutar compilación y pruebas.
5. Abrir un pull request con explicación y evidencia. Otro estudiante revisa el cambio.
6. Corregir observaciones y comprobar Actions antes de integrar.
7. Etiquetar una versión revisada para distribuir el ejecutable.

Main más ramas cortas corresponde a GitHub Flow. El equipo debe integrar cambios a diario para acercarse a Trunk Based. Git Flow se reserva en la propuesta para productos que necesiten mantener varias versiones y ramas de release.

## Revisión propuesta
Revisar los criterios de aceptación, rechazos de entrada, conservación del estado ante errores, llaves en todos los bloques y separación entre entidad y controlador. La aprobación del segundo estudiante debe ser real. Una revisión automática o la creación de un PR no prueban trabajo de dos personas.

## Git avanzado aplicado
git diff muestra cambios antes de confirmar. git log --graph --oneline --all muestra la historia. git revert crea una reversión trazable de un commit integrado. git cherry-pick permite llevar una corrección a una rama de mantenimiento. git bisect permite encontrar el cambio que introdujo una regresión usando un test reproducible. No se requiere ejecutar estas operaciones sobre una versión estable solo para producir evidencia.

## GitHub Spec Kit
SDD consiste en acordar el comportamiento antes de implementar. Las carpetas specs y .specify contienen los documentos del ejercicio redactados con ese enfoque. No se atribuyen al CLI ejecuciones que no se han realizado.

Para usar la herramienta oficial con Copilot en una rama de prueba, comprobar la referencia vigente y ejecutar:
```powershell
uv tool install specify-cli --from git+https://github.com/github/spec-kit.git
specify init --here --integration copilot --script ps
```
Usar primero una rama o copia de trabajo para revisar los archivos que genere la herramienta. En el agente seleccionado, seguir constitution, specify, clarify, plan, tasks, analyze e implement. Los nombres de comandos dependen de la integración y pueden cambiar. Referencia: https://github.github.com/spec-kit/integrations/cli.html

## Prompt de ejemplo para Copilot
Lee spec.md, CA08 y constitution.md. Propón pruebas para reabrir una incidencia resuelta, rechazar una pendiente y rechazar una en atención. Conserva el estilo de campos privados, constructores y propiedades explícitas. Escribe todos los bloques con llaves. Primero explica los casos y después modifica únicamente Incidencia y Pruebas. Ejecuta la solución y muestra el resultado. No agregues pantallas ni paquetes.

El prompt incluye contexto, objetivo, restricciones y comprobación. Las sugerencias deben revisarse. Este archivo prepara el uso de Copilot, no certifica una sesión que el grupo todavía no haya realizado con esa herramienta.

## Ejemplo para exponer
Registrar una incidencia con responsable Ana y prioridad Alta. Intentar resolverla directamente y explicar el rechazo. Iniciar atención, marcarla resuelta y reabrirla. Filtrar Pendiente y Ana en Consultas. Abrir el PR de reapertura y su workflow para relacionar el comportamiento con CA08.

## Límites
Las pruebas de consola no sustituyen las pruebas de interfaz ni garantizan ausencia de defectos. XML local no sirve para concurrencia corporativa. El despliegue de este taller distribuye un ejecutable privado y no instala automáticamente software en equipos de la empresa.

## Evidencia de SDD
El recorrido aplicado y sus limites estan en [sdd.md](sdd.md). Antes de solicitar una nueva funcion a la IA, acordar los criterios, revisar el plan y preparar casos de aceptacion. Despues relacionar el codigo generado con las pruebas y registrar su evolucion en un PR. La matriz [trazabilidad.md](trazabilidad.md) permite revisar este ejercicio.

## Version distribuible
VERSION identifica la entrega actual. Un push a main valida codigo y pruebas y publica esa version si todavia no existe. Para distribuir cambios posteriores se incrementa VERSION; las versiones existentes se conservan. Tambien se admite etiqueta v* o ejecucion manual con version explicita. La release incluye el ZIP del ejecutable, el informe PDF y la presentacion PPTX de entrega.
