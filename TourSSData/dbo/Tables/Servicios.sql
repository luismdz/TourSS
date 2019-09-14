CREATE TABLE [dbo].[Servicios] (
    [servicioID]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [descripcion]      NVARCHAR (100) DEFAULT ('') NOT NULL,
    [precio]           MONEY          NOT NULL,
    [cuposDisponibles] TINYINT        DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Servicio] PRIMARY KEY CLUSTERED ([servicioID] ASC),
    CONSTRAINT [CHK_Cupos] CHECK ([cuposDisponibles]>=(0)),
    CONSTRAINT [CHK_Precio] CHECK ([precio]>(0))
);

