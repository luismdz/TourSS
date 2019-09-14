CREATE TABLE [dbo].[Pagos] (
    [pagoID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [reservacionID] BIGINT        NOT NULL,
    [monto]         MONEY         NOT NULL,
    [tipo]          NCHAR (3)     DEFAULT ('CON') NOT NULL,
    [fechaPago]     DATETIME      DEFAULT (getdate()) NOT NULL,
    [concepto]      NVARCHAR (50) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Pagos] PRIMARY KEY CLUSTERED ([pagoID] ASC),
    CONSTRAINT [CHK_Monto] CHECK ([monto]>(0)),
    CONSTRAINT [CHK_TipoPago] CHECK ([tipo]='CRE' OR [tipo]='CON'),
    CONSTRAINT [FK_Reservaciones_Pagos] FOREIGN KEY ([reservacionID]) REFERENCES [dbo].[Reservaciones] ([reservacionID])
);

