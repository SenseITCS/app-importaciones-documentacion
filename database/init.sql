-- Verificar si la base de datos existe y crearla si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ProyectosClaude')
BEGIN
    CREATE DATABASE ProyectosClaude;
END;
GO

USE ProyectosClaude;
GO

-- Crear tabla de credenciales si no existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreedencialesApps]') AND type in (N'U'))
BEGIN
    CREATE TABLE CreedencialesApps (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Cedula VARCHAR(20) NOT NULL,
        Contrasena VARCHAR(255) NOT NULL,
        Fuente VARCHAR(50) NOT NULL,
        FechaCreacion DATETIME DEFAULT GETDATE(),
        UltimaModificacion DATETIME DEFAULT GETDATE(),
        CONSTRAINT UC_Cedula_Fuente UNIQUE (Cedula, Fuente)
    );

    -- Crear índices para optimizar búsquedas
    CREATE INDEX IX_Cedula ON CreedencialesApps(Cedula);
    CREATE INDEX IX_Fuente ON CreedencialesApps(Fuente);
END;
GO

-- Insertar datos de prueba (opcional)
-- INSERT INTO CreedencialesApps (Cedula, Contrasena, Fuente)
-- VALUES ('123456789', 'HashedPassword123', 'ImportacionesApp');
-- GO