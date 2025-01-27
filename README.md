# Aplicación de Importaciones

## Descripción
Aplicación web para gestión de documentos y plan de órdenes de compra (PO Plan).

## Requisitos del Sistema

### Base de datos
- SQL Server (versión por definir)
- Bases de datos requeridas:
  - GIAX2012_PROD (existente)
  - ProyectosClaude (nueva)

### Funcionalidades Principales
1. Sistema de Autenticación
   - Login mediante cédula y contraseña
   - Registro inicial de usuarios validando contra base de datos GIAX2012_PROD
   - Almacenamiento seguro de credenciales en ProyectosClaude

2. Pantalla Principal
   - Acceso a módulo de Documentos
   - Acceso a módulo de PO Plan
   - Selector de compañía

## Estructura de Base de Datos

### Tabla: CreedencialesApps
- Id (INT, Primary Key)
- Cedula (VARCHAR(20))
- Contrasena (VARCHAR(255))
- Fuente (VARCHAR(50))
- FechaCreacion (DATETIME)
- UltimaModificacion (DATETIME)

## Proceso de Registro
1. Usuario proporciona número de cédula
2. Sistema valida existencia en GIAX2012_PROD.DirPartyTable
3. Si existe, muestra nombre del usuario
4. Usuario crea contraseña segura
5. Sistema almacena credenciales

## Seguridad
- Validación de contraseñas según estándares de seguridad
- Almacenamiento seguro de credenciales
- Gestión de sesiones

## Configuración del Proyecto
[Pendiente: Agregar detalles de configuración según tecnologías seleccionadas]
