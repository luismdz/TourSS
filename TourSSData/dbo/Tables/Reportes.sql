CREATE TABLE [dbo].[Reportes] (
    [reporteID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [usuarioID]   BIGINT         NOT NULL,
    [fecha]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [descripcion] NVARCHAR (100) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Reportes] PRIMARY KEY CLUSTERED ([reporteID] ASC),
    CONSTRAINT [FK_Usuario_Reporte] FOREIGN KEY ([usuarioID]) REFERENCES [dbo].[Usuarios] ([usuarioID])
);

