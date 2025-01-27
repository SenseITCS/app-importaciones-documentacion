-- Crear nueva base de datos
CREATE DATABASE ProyectosClaude;
GO

USE ProyectosClaude;
GO

-- Crear tabla de credenciales
CREATE TABLE CreedencialesApps (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Cedula VARCHAR(20) NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
    Fuente VARCHAR(50) NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    UltimaModificacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT UC_Cedula_Fuente UNIQUE (Cedula, Fuente)
);
GO

-- Índices para optimizar búsquedas
CREATE INDEX IX_Cedula ON CreedencialesApps(Cedula);
CREATE INDEX IX_Fuente ON CreedencialesApps(Fuente);
