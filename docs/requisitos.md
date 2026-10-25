# Requerimientos y reglas de negocio

## Caso de la empresa
La empresa del caso pierde versiones, tiene poca coordinacion, encuentra errores al entregar y carece de documentacion suficiente. La propuesta combina Git y GitHub, trabajo mediante PR, especificaciones y CI/CD. El programa C# demuestra ese proceso mediante una funcion acotada: gestionar incidencias de software. El prototipo no pretende sustituir todas las herramientas de la empresa.

## Necesidades de la aplicacion
- R01 Registrar, corregir y eliminar problemas con codigo, titulo, descripcion, responsable y prioridad.
- R02 Controlar atencion, resolucion y reapertura sin permitir transiciones incorrectas.
- R03 Consultar por estado y responsable y conocer la carga de trabajo.
- R04 Conservar los datos al cerrar y evitar cambios parciales si falla el archivo.
- R05 Mantener formularios sencillos y separar la lista de las reglas de negocio.
- R06 Identificar cada cambio y verificarlo antes de distribuir una version.

## Restricciones
Windows Forms, C# y .NET Framework 4.7.2. Datos XML por usuario, sin servidor, autenticacion ni edicion simultanea. frmPrincipal presenta la tabla y consultas; frmAdmin ingresa o modifica. Los controladores de lista almacenan y realizan CRUD. La entidad conserva las reglas.

## Desarrollo guiado por especificaciones
La aclaracion del docente Joffre indica investigar SDD y aplicar el caso con codigo generado por IA a partir de requerimientos precisos. Los estudiantes definen y revisan las especificaciones, comprenden el resultado y comprueban su funcionamiento. El uso de IA forma parte del ejercicio.

La especificacion detallada esta en ../specs/001-incidencias/spec.md. R01 se concreta en CA01-CA04; R02 en CA05-CA09; R03 en CA10-CA11; R04 en CA04, CA12 y CA13; R05 en CA14-CA15; R06 en CA16 y el flujo de revision. El recorrido de las seis fases esta en sdd.md.
