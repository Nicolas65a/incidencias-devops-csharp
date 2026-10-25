# Matriz de requisitos y verificacion

Esta matriz relaciona la especificacion con el codigo y la evidencia. Una fila no significa cobertura total de todas las entradas posibles. Program.cs contiene las 29 comprobaciones originales de logica y XML; PruebasInterfaz.cs agrega 21 de formularios e integracion. El resultado esperado es 50 aprobadas, 0 fallidas y codigo de salida 0. El registro de Actions confirma el resultado de cada commit ejecutado.

| Criterio | Implementacion | Comprobacion y evidencia |
| --- | --- | --- |
| CA01 Codigo positivo unico y estado inicial | frmPrincipal.btnNuevo_Click, Incidencia | Program: Codigo no positivo y Estado inicial pendiente. Interfaz: Nuevo calcula un codigo unico desde la tabla y Guardar editor devuelve registro pendiente. Carga rechaza duplicados. |
| CA02 Campos obligatorios y prioridad | Incidencia.ValidarDatos, frmAdmin.btnGuardar_Click | Program: Titulo vacio, Descripcion nula, Responsable obligatorio y Prioridad invalida. Interfaz: Guardar campos vacios mantiene abierto el editor. |
| CA03 Modificar conserva identidad y fechas; eliminar confirma | frmAdmin.CrearObjeto, frmPrincipal.GuardarIncidencia y btnEliminar_Click | Interfaz: Modificar conserva codigo estado y fechas; Modificar desde boton conserva fechas al guardar; Rechazar confirmacion conserva registro y archivo; Confirmar eliminacion actualiza lista y archivo. |
| CA04 Persistencia entre aperturas | ArchivoIncidencia, frmPrincipal_Load | Program: Guardar y recuperar XML y Actualizar archivo y conservar cierre. Interfaz: Reabrir formulario recupera lo guardado. |
| CA05 Iniciar solo pendiente | Incidencia.IniciarAtencion | Program: Iniciar atencion, No iniciar atencion dos veces y No atender resuelta. |
| CA06 Resolver solo en atencion | Incidencia.Resolver | Program: No resolver pendiente, Resolver en fecha valida y No volver a resolver. |
| CA07 Fecha de cierre valida | Incidencia.Resolver y ValidarDatos | Program: No cerrar antes del registro, Resolver en fecha valida y Resuelta requiere fecha. |
| CA08 Reabrir solo resuelta y borrar cierre | Incidencia.Reabrir | Program: Reabrir una resuelta, No reabrir pendiente y No reabrir en atencion. PR #1 conserva fases roja y verde. |
| CA09 Rechazo conserva estado y fechas | Metodos de estado de Incidencia | Los casos negativos anteriores comprueban tambien el estado y las fechas posteriores. |
| CA10 Filtros combinados y nombre parcial | frmPrincipal.btnConsultar_Click | Interfaz: Sin filtros muestra todos en orden; Filtro por estado; Nombre parcial sin distinguir mayusculas; Combina ambos filtros; Consulta sin coincidencias. |
| CA11 Resumen por responsable | frmPrincipal.btnResumen_Click | Interfaz: Resumen de todos los estados y responsables, incluso si hay filtros activos en pantalla. |
| CA12 Error de guardado conserva datos | frmPrincipal.GuardarDatos y operaciones con copia previa | Interfaz: Fallo al agregar, modificar, cambiar estado y eliminar. Una ruta bloqueada provoca el error sin tocar datos del usuario. |
| CA13 Error de carga bloquea edicion | ArchivoIncidencia.Cargar, frmPrincipal_Load | Program: Detectar XML danado y Rechazar codigos duplicados al cargar. Interfaz: Archivo danado bloquea todas las acciones de edicion y conserva su contenido. |
| CA14 Bloques con llaves | Archivos C# | Inspeccion de sintaxis con Roslyn registrada en verificacion-estilo.txt de Actions. La compilacion tambien trata advertencias como errores. |
| CA15 Dos formularios con guardar y cancelar | frmPrincipal, frmAdmin | Interfaz: Tabla principal de solo lectura, Cancelar no modifica el objeto original, Guardar editor devuelve registro pendiente y operaciones Nuevo/Modificar desde sus botones. Revision visual complementaria. |
| CA16 Paquete condicionado a verificaciones | .github/workflows/ci.yml | El job valida estilo, compila, ejecuta Pruebas.exe y solo despues comprime. El job de distribucion depende de esa validacion. Un fallo devuelve codigo no cero y bloquea el paquete. |

## Como repetir
En Developer PowerShell, compilar la solucion Release y ejecutar Pruebas/bin/Release/Pruebas.exe. Las pruebas de interfaz requieren Windows y ejecutan los formularios del producto en un hilo STA. Los dialogos se contestan automaticamente solo dentro del hilo de pruebas; no se usa el teclado global ni se interactua con otras aplicaciones. Los archivos temporales se crean al lado del ejecutor y se eliminan al terminar. No se usa el XML personal.

Revisar el resultado completo y no solo la cantidad final. CA14 se verifica por separado con scripts/VerificarEstilo.ps1. La inspeccion visual y la revision humana del Issue #3 son complementarias; esta matriz no las declara realizadas por otra persona.
