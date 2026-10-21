# Plan técnico

## Estructura
- Entidades/Incidencia.cs contiene atributos privados, constructor vacío, constructor con parámetros, propiedades y reglas del estado.
- Controlador/TListaIncidencia.cs almacena la lista y realiza Agregar, Modificar, Eliminar, getIncidencia y Buscar.
- Controlador/ArchivoIncidencia.cs lee y guarda XML con XmlSerializer.
- Formularios/frmPrincipal.cs enlaza controles y acciones. Las consultas LINQ están en los eventos correspondientes.
- Pruebas/Program.cs ejecuta comprobaciones reproducibles y devuelve 1 si alguna falla.

## Persistencia
Archivo por usuario: %LOCALAPPDATA%/GestionIncidencias/incidencias.xml. Un temporal se serializa antes de reemplazar el archivo anterior. El formulario guarda una copia de la lista antes de aplicar el cambio en memoria. No se distribuyen datos personales de ejemplo.

## Flujo de desarrollo
Main representa una versión que compila. Las ramas feature contienen cambios pequeños. Un pull request debe explicar el problema, indicar sus criterios de aceptación y mostrar el resultado de pruebas. Se propone revisión por el otro estudiante antes de integrar.

Para este equipo de dos personas se escoge GitHub Flow con ramas cortas. Se busca integrar a diario, siguiendo el principio de lotes pequeños de Trunk Based. No se afirma que cualquier rama feature de larga duración sea Trunk Based.

## Entrega
Actions compila con MSBuild en Windows, ejecuta el programa de pruebas y conserva un ZIP como artefacto. Una etiqueta v* repite la validación y publica ese mismo paquete en una release privada. Esta es distribución de una aplicación de escritorio, no despliegue de un servicio web. Para una empresa se añadirían firma de ejecutables, instalador, respaldo y observabilidad.
