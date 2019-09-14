CREATE TABLE [dbo].[Credenciales] (
    [credencialID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [username]     NVARCHAR (20) NOT NULL,
    [password]     NCHAR (8)     NOT NULL,
    [usuarioID]    BIGINT        NOT NULL,
    PRIMARY KEY CLUSTERED ([credencialID] ASC),
    CONSTRAINT [FK_Usuarios_Credenciales] FOREIGN KEY ([usuarioID]) REFERENCES [dbo].[Usuarios] ([usuarioID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UQ_username] UNIQUE NONCLUSTERED ([username] ASC)
);

