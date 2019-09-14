CREATE TABLE [dbo].[Reservaciones] (
    [reservacionID] BIGINT   IDENTITY (1000, 1) NOT NULL,
    [usuarioID]     BIGINT   NOT NULL,
    [clienteID]     BIGINT   NOT NULL,
    [fecha]         DATETIME DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Reservaciones] PRIMARY KEY CLUSTERED ([reservacionID] ASC),
    CONSTRAINT [FK_Clientes_Reservaciones] FOREIGN KEY ([clienteID]) REFERENCES [dbo].[Clientes] ([clienteID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_Usuarios_Reservaciones] FOREIGN KEY ([usuarioID]) REFERENCES [dbo].[Usuarios] ([usuarioID]) ON UPDATE CASCADE
);

