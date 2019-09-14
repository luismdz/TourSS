CREATE TABLE [dbo].[Roles] (
    [rolID]       SMALLINT      IDENTITY (1, 1) NOT NULL,
    [descripcion] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([rolID] ASC)
);

