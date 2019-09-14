CREATE TABLE [dbo].[ReservacionDetalle] (
    [reservacionID] BIGINT NOT NULL,
    [servicioID]    BIGINT NOT NULL,
    [precio]        MONEY  DEFAULT ((0)) NOT NULL,
    [cantidad]      INT    NOT NULL,
    [subtotal]      MONEY  DEFAULT ((0)) NOT NULL,
    [itbis]         MONEY  DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ReservacionDetalle] PRIMARY KEY CLUSTERED ([reservacionID] ASC, [servicioID] ASC),
    CONSTRAINT [CHK_Cantidad] CHECK ([cantidad]>(0)),
    CONSTRAINT [FK_Reservaciones_ReservacionDetalle] FOREIGN KEY ([reservacionID]) REFERENCES [dbo].[Reservaciones] ([reservacionID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Servicios_ReservacionDetalle] FOREIGN KEY ([servicioID]) REFERENCES [dbo].[Servicios] ([servicioID]) ON DELETE CASCADE ON UPDATE CASCADE
);

