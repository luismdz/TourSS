CREATE TABLE [dbo].[Comentarios] (
    [comentarioID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [clienteID]    BIGINT         NOT NULL,
    [detalle]      NVARCHAR (200) NOT NULL,
    [fecha]        DATETIME       DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([comentarioID] ASC),
    CONSTRAINT [FK_Clientes_Comentarios] FOREIGN KEY ([clienteID]) REFERENCES [dbo].[Clientes] ([clienteID])
);

