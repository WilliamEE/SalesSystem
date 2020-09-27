IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [categoria] (
    [id] int NOT NULL IDENTITY,
    [nombre] varchar(50) NULL,
    [id_padre] int NULL,
    CONSTRAINT [PK_categoria] PRIMARY KEY ([id]),
    CONSTRAINT [FK_categoria_categoria] FOREIGN KEY ([id_padre]) REFERENCES [categoria] ([id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [rol] (
    [id] int NOT NULL IDENTITY,
    [nombre] varchar(50) NULL,
    CONSTRAINT [PK_rol] PRIMARY KEY ([id])
);

GO

CREATE TABLE [SolicitudDeAfiliacion] (
    [id] int NOT NULL IDENTITY,
    [fecha] date NULL,
    [referenciaBancariaUrl] varchar(400) NULL,
    [reciboLuzUrl] varchar(400) NULL,
    [ReciboAguaUrl] varchar(400) NULL,
    [reciboTelefonoUrl] varchar(400) NULL,
    [pagareUrl] varchar(400) NULL,
    [estado] varchar(20) NULL,
    CONSTRAINT [PK_SolicitudDeAfiliacion] PRIMARY KEY ([id])
);

GO

CREATE TABLE [producto] (
    [id] int NOT NULL IDENTITY,
    [nombre] varchar(100) NULL,
    [precio] decimal(18, 2) NULL,
    [imagenUrl] varchar(200) NULL,
    [id_categoria] int NULL,
    [id_perfil] int NULL,
    CONSTRAINT [PK_producto] PRIMARY KEY ([id]),
    CONSTRAINT [FK_producto_categoria] FOREIGN KEY ([id_categoria]) REFERENCES [categoria] ([id]) ON DELETE CASCADE
);

GO

CREATE TABLE [usuario] (
    [id] int NOT NULL IDENTITY,
    [correo] varchar(100) NULL,
    [contraseña] varchar(100) NULL,
    [id_rol] int NULL,
    CONSTRAINT [PK_usuario] PRIMARY KEY ([id]),
    CONSTRAINT [FK_usuario_rol] FOREIGN KEY ([id_rol]) REFERENCES [rol] ([id]) ON DELETE CASCADE
);

GO

CREATE TABLE [perfilDeUsuario] (
    [id] int NOT NULL IDENTITY,
    [nombre] varchar(100) NULL,
    [fotoDePerfil] varchar(400) NULL,
    [id_usuario] int NULL,
    CONSTRAINT [FK_perfilDeUsuario_usuario] FOREIGN KEY ([id_usuario]) REFERENCES [usuario] ([id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_categoria_id_padre] ON [categoria] ([id_padre]);

GO

CREATE INDEX [IX_perfilDeUsuario_id_usuario] ON [perfilDeUsuario] ([id_usuario]);

GO

CREATE INDEX [IX_producto_id_categoria] ON [producto] ([id_categoria]);

GO

CREATE INDEX [IX_usuario_id_rol] ON [usuario] ([id_rol]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200927180557_Migracion inicial', N'3.1.8');

GO

