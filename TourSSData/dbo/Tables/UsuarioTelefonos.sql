CREATE TABLE [dbo].[UsuarioTelefonos] (
    [usuarioTelefonoID] BIGINT     IDENTITY (1, 1) NOT NULL,
    [numero]            NCHAR (12) NOT NULL,
    [usuarioID]         BIGINT     NOT NULL,
    [telefonoEstado]    CHAR (1)   DEFAULT ('A') NOT NULL,
    CONSTRAINT [PK_EmpTelefono] PRIMARY KEY CLUSTERED ([usuarioTelefonoID] ASC),
    CONSTRAINT [CHK_empTelefonoEstado] CHECK ([telefonoEstado]='I' OR [telefonoEstado]='A'),
    CONSTRAINT [FK_Usuario_Telefono] FOREIGN KEY ([usuarioID]) REFERENCES [dbo].[Usuarios] ([usuarioID]) ON DELETE CASCADE ON UPDATE CASCADE,
    UNIQUE NONCLUSTERED ([numero] ASC)
);

