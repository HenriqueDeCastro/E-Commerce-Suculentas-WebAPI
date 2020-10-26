IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] int NOT NULL IDENTITY,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        [FullName] nvarchar(100) NULL,
        [CPF] nvarchar(20) NULL,
        [DataNascimento] date NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [EmpresaFretes] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(max) NULL,
        CONSTRAINT [PK_EmpresaFretes] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [Estados] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(max) NULL,
        [UF] nvarchar(max) NULL,
        CONSTRAINT [PK_Estados] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [Gastos] (
        [Id] int NOT NULL IDENTITY,
        [Descricao] int NOT NULL,
        [Valor] int NOT NULL,
        [Data] datetime2 NOT NULL,
        CONSTRAINT [PK_Gastos] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [Status] (
        [Id] int NOT NULL IDENTITY,
        [Nome] int NOT NULL,
        CONSTRAINT [PK_Status] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [TipoCategoria] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(max) NULL,
        CONSTRAINT [PK_TipoCategoria] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] int NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] int NOT NULL,
        [RoleId] int NOT NULL,
        [UserId1] int NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId1] FOREIGN KEY ([UserId1]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] int NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [Cidades] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(max) NULL,
        [EstadoId] int NOT NULL,
        CONSTRAINT [PK_Cidades] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Cidades_Estados_EstadoId] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [Categorias] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(max) NULL,
        [Descricao] nvarchar(max) NULL,
        [Ativo] bit NOT NULL,
        [TipoCategoriaId] int NOT NULL,
        CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Categorias_TipoCategoria_TipoCategoriaId] FOREIGN KEY ([TipoCategoriaId]) REFERENCES [TipoCategoria] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [Enderecos] (
        [Id] int NOT NULL IDENTITY,
        [Descricao] nvarchar(max) NULL,
        [Rua] nvarchar(max) NULL,
        [Numero] int NOT NULL,
        [Complemento] nvarchar(max) NULL,
        [CEP] nvarchar(max) NULL,
        [CidadeId] int NOT NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_Enderecos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Enderecos_Cidades_CidadeId] FOREIGN KEY ([CidadeId]) REFERENCES [Cidades] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Enderecos_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [Produtos] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(max) NULL,
        [Descricao] nvarchar(max) NULL,
        [Preco] float NOT NULL,
        [Imagem] nvarchar(max) NULL,
        [Estoque] int NOT NULL,
        [CategoriaId] int NOT NULL,
        [Ativo] bit NOT NULL,
        CONSTRAINT [PK_Produtos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Produtos_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [Vendas] (
        [Id] int NOT NULL IDENTITY,
        [DataVenda] datetime2 NOT NULL,
        [Valor] float NOT NULL,
        [Frete] float NOT NULL,
        [StatusId] int NOT NULL,
        [UserId] int NOT NULL,
        [EmpresaFreteId] int NOT NULL,
        [EnderecoId] int NOT NULL,
        CONSTRAINT [PK_Vendas] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Vendas_EmpresaFretes_EmpresaFreteId] FOREIGN KEY ([EmpresaFreteId]) REFERENCES [EmpresaFretes] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vendas_Enderecos_EnderecoId] FOREIGN KEY ([EnderecoId]) REFERENCES [Enderecos] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vendas_Status_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Status] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vendas_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE TABLE [Pedidos] (
        [ProdutoId] int NOT NULL,
        [VendaId] int NOT NULL,
        [Id] int NOT NULL,
        [Quantidade] int NOT NULL,
        CONSTRAINT [PK_Pedidos] PRIMARY KEY ([VendaId], [ProdutoId]),
        CONSTRAINT [FK_Pedidos_Produtos_ProdutoId] FOREIGN KEY ([ProdutoId]) REFERENCES [Produtos] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Pedidos_Vendas_VendaId] FOREIGN KEY ([VendaId]) REFERENCES [Vendas] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_UserId1] ON [AspNetUserRoles] ([UserId1]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_Categorias_TipoCategoriaId] ON [Categorias] ([TipoCategoriaId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_Cidades_EstadoId] ON [Cidades] ([EstadoId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_Enderecos_CidadeId] ON [Enderecos] ([CidadeId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_Enderecos_UserId] ON [Enderecos] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_Pedidos_ProdutoId] ON [Pedidos] ([ProdutoId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_Produtos_CategoriaId] ON [Produtos] ([CategoriaId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_Vendas_EmpresaFreteId] ON [Vendas] ([EmpresaFreteId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_Vendas_EnderecoId] ON [Vendas] ([EnderecoId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_Vendas_StatusId] ON [Vendas] ([StatusId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    CREATE INDEX [IX_Vendas_UserId] ON [Vendas] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200923014125_init')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200923014125_init', N'3.1.7');
END;

GO

