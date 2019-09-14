CREATE TABLE [dbo].[ClienteTelefonos] (
    [clienteTelefonoID] BIGINT     IDENTITY (1, 1) NOT NULL,
    [numero]            NCHAR (12) NOT NULL,
    [clienteID]         BIGINT     NOT NULL,
    [telefonoEstado]    CHAR (1)   DEFAULT ('A') NOT NULL,
    CONSTRAINT [PK_ClienteTelefonos] PRIMARY KEY CLUSTERED ([clienteTelefonoID] ASC),
    CONSTRAINT [CHK_ClienteTeleEstado] CHECK ([telefonoEstado]='I' OR [telefonoEstado]='A'),
    CONSTRAINT [FK_Clientes_Telefono] FOREIGN KEY ([clienteID]) REFERENCES [dbo].[Clientes] ([clienteID]) ON DELETE CASCADE ON UPDATE CASCADE,
    UNIQUE NONCLUSTERED ([numero] ASC)
);

