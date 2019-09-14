CREATE TABLE [dbo].[Clientes] (
    [clienteID]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [clienteCodigo]   NVARCHAR (128) NOT NULL,
    [clienteCedula]   NCHAR (13)     NOT NULL,
    [clienteNombre]   NVARCHAR (50)  NOT NULL,
    [clienteApellido] NVARCHAR (50)  NOT NULL,
    [clienteGenero]   NCHAR (1)      NOT NULL,
    [clienteCorreo]   NVARCHAR (100) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED ([clienteID] ASC),
    CONSTRAINT [CHK_clienteCodigo] CHECK ([clienteCodigo] like 'TC-%'),
    CONSTRAINT [CHK_clienteGenero] CHECK ([clienteGenero]='F' OR [clienteGenero]='M'),
    CONSTRAINT [UQ_clienteCedula] UNIQUE NONCLUSTERED ([clienteCedula] ASC),
    CONSTRAINT [UQ_clienteCodigo] UNIQUE NONCLUSTERED ([clienteCodigo] ASC)
);

