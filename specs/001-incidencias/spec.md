# Especificación del sistema de incidencias

## Problema y alcance
Una empresa necesita registrar errores del software y conocer su responsable y avance. Se propone una demostración de escritorio individual. GitHub administra el código y el trabajo del equipo; la aplicación demuestra una función de negocio. Ambos registros tienen propósitos diferentes.

El prototipo usa Windows Forms y .NET Framework 4.7.2. Guarda datos locales en XML. No incluye autenticación, servidor ni acceso simultáneo entre equipos. No sustituye un sistema corporativo.

## Historias y criterios de aceptación

### HU01 Registrar y administrar incidencias
Como responsable del equipo necesito registrar título, descripción, responsable y prioridad para identificar los problemas.
- CA01 Un registro nuevo recibe un código positivo único y estado Pendiente.
- CA02 El sistema rechaza título, descripción o responsable vacíos. La prioridad debe ser Alta, Media o Baja.
- CA03 Modificar conserva código, estado y fechas. Eliminar solicita confirmación.
- CA04 Cerrar y abrir la aplicación conserva los registros válidos.

### HU02 Gestionar la atención
Como responsable necesito controlar el avance para evitar cerrar un problema sin atenderlo.
- CA05 Solo una incidencia Pendiente puede pasar a En atención.
- CA06 Solo una incidencia En atención puede pasar a Resuelta.
- CA07 Resolver registra la fecha y rechaza fechas anteriores al registro.
- CA08 Reabrir solo permite pasar de Resuelta a Pendiente y elimina la fecha de cierre.
- CA09 Un intento inválido no modifica el estado ni la fecha anterior.

### HU03 Consultar la carga de trabajo
Como coordinador necesito filtrar por estado y parte del nombre del responsable, sin distinguir mayúsculas.
- CA10 Los filtros se combinan y muestran todos los registros cuando no hay restricciones.
- CA11 El resumen agrupa todas las incidencias por responsable y cuenta sus estados.

## Calidad y restricciones
- CA12 Si falla el guardado, la lista en memoria conserva sus datos anteriores.
- CA13 Un archivo dañado bloquea la edición para evitar reemplazarlo con una lista vacía.
- CA14 Todas las estructuras de control y eventos de botones usan bloques con llaves.
- CA15 frmPrincipal reúne la tabla de Incidencias y las Consultas. Nuevo y Modificar abren frmAdmin, que permite guardar o cancelar los datos.
- CA16 El pipeline compila, ejecuta pruebas y genera un paquete solo si todo termina correctamente.

## Trazabilidad
HU01: Incidencia.ValidarDatos, TListaIncidencia, ArchivoIncidencia y pestaña Incidencias.
HU02: Incidencia.IniciarAtencion, Resolver y Reabrir, botones correspondientes.
HU03: eventos btnConsultar_Click y btnResumen_Click con LINQ.
CA12 y CA13: GuardarDatos y frmPrincipal_Load.
CA16: .github/workflows/ci.yml.

El origen de las necesidades esta en [requisitos.md](../../docs/requisitos.md). La relacion de todos los criterios con sus verificaciones esta en [trazabilidad.md](../../docs/trazabilidad.md). El recorrido de las seis fases esta en [sdd.md](../../docs/sdd.md).
