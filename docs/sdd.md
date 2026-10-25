# Aplicacion de las seis fases de SDD

La especificacion es la referencia para pedir cambios a la IA y decidir si el resultado cumple. Tener documentos con nombres determinados no prueba por si solo que el proceso se haya seguido. Las evidencias siguientes permiten revisar el trabajo concreto.

| Fase | Aplicacion en este ejercicio | Evidencia |
| --- | --- | --- |
| 1 Requerimientos | Necesidades de la empresa y alcance del prototipo | [requisitos.md](requisitos.md), R01-R06 |
| 2 Especificacion | Historias, reglas, restricciones y 16 criterios verificables | [spec.md](../specs/001-incidencias/spec.md), HU01-HU03 y CA01-CA16 |
| 3 Plan tecnico | Entidad, lista, XML, formularios, pruebas y tareas | [plan.md](../specs/001-incidencias/plan.md) y [tasks.md](../specs/001-incidencias/tasks.md) |
| 4 Implementacion | Codigo generado y ajustado con Codex conforme al comportamiento solicitado | GestionIncidencias y Pruebas; commits y PR |
| 5 Verificacion | Ejecutar pruebas contra las reglas y revisar la interfaz | [trazabilidad.md](trazabilidad.md), Pruebas y Actions |
| 6 Evolucion | Cambiar especificacion, comprobar regresiones e integrar una version identificable | PR #1 y #4, Issue #5 y sus cambios posteriores |

## Ejemplo completo de una regla
R02 exige controlar el avance. CA08 especifica que solo una incidencia Resuelta puede reabrirse y que se borra su fecha de cierre; CA09 exige conservar datos ante un rechazo. El plan asigna esa regla a Incidencia.Reabrir. La IA implementa el metodo y el boton lo utiliza. El PR #1 conserva una prueba roja y su implementacion verde. Actions ejecuta las comprobaciones y Git registra la evolucion.

La ampliacion del Issue #5 toma CA10, CA11, CA12, CA13 y CA15 ya documentados y agrega comprobaciones de los eventos y formularios reales. No cambia las reglas para hacer pasar las pruebas.

## Instruccion de esta ampliacion a la IA
La solicitud fue completar la relacion entre requisitos y pruebas y actualizar proyecto, GitHub, informe y presentacion. Se incorporo la aclaracion del docente: aplicar las seis fases y generar el codigo desde las especificaciones. El trabajo usa Codex; no se atribuye a una sesion de Copilot o del CLI de Spec Kit.

Prompt tecnico reproducible:

> Lee docs/requisitos.md, spec.md y plan.md. Conserva CA01-CA16 y los dos formularios. Agrega pruebas que ejecuten los eventos de consultas, resumen, ingreso, modificacion y cancelacion. Provoca errores de lectura y guardado en archivos de prueba y comprueba que los datos anteriores se conservan. No alteres el XML personal ni agregues reglas de negocio al controlador de listas. Relaciona cada criterio con su verificacion, compila sin advertencias y comunica los fallos reales. Actualiza los documentos y somete el cambio a Actions antes de integrarlo.

## Alcance de la evidencia
El historial permite observar cambios y su verificacion; no registra cada mensaje de la conversacion ni demuestra que todas las especificaciones originales se aprobaran en un commit separado antes de todo el codigo. Esta ampliacion documenta esa limitacion y deja un recorrido revisable sin reconstruir ni inventar una cronologia.

Los archivos de Copilot y Spec Kit son preparacion y referencia. El docente aclaro generacion con IA; no se debe afirmar que se usaron todas las herramientas mencionadas solo por tener sus archivos de configuracion.

La revision del segundo estudiante sigue en el Issue #3. Los resultados automaticos y los documentos no sustituyen su revision ni su participacion en la exposicion.
