CREATE TABLE [dbo].[Usuarios] (
    [usuarioID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [usuarioCodigo] NVARCHAR (128) NOT NULL,
    [empCedula]     NCHAR (13)     NOT NULL,
    [empNombre]     NVARCHAR (50)  NOT NULL,
    [empApellido]   NVARCHAR (50)  NOT NULL,
    [empGenero]     CHAR (1)       NOT NULL,
    [empCorreo]     NVARCHAR (100) DEFAULT ('') NOT NULL,
    [empEstado]     CHAR (1)       DEFAULT ('A') NOT NULL,
    [rolID]         SMALLINT       NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([usuarioID] ASC),
    CONSTRAINT [CHK_empEstado] CHECK ([empEstado]='I' OR [empEstado]='A'),
    CONSTRAINT [CHK_empGenero] CHECK ([empGenero]='F' OR [empGenero]='M'),
    CONSTRAINT [CHK_usuarioCodigo] CHECK ([usuarioCodigo] like 'TU-%'),
    CONSTRAINT [FK_Usuarios_Roles] FOREIGN KEY ([rolID]) REFERENCES [dbo].[Roles] ([rolID]),
    CONSTRAINT [UQ_empCedula] UNIQUE NONCLUSTERED ([empCedula] ASC),
    CONSTRAINT [UQ_usuarioCodigo] UNIQUE NONCLUSTERED ([usuarioCodigo] ASC)
);

