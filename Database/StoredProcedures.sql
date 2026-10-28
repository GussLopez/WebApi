-- Ejecutar en la BD de la API (Db103) con SSMS. Requiere dbo.Usuarios y dbo.Roles.
-- PasswordHash debe ser generado por PasswordHasher en C#. NULL al editar conserva el hash.
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE dbo.spGetUsers

AS
BEGIN
 SET NOCOUNT ON;
 SELECT PkUsuario, Nombre, UserName, FkRol FROM dbo.Usuarios ORDER BY PkUsuario;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spGetRoles

AS
BEGIN
 SET NOCOUNT ON;
 SELECT PkRol, Nombre, Estatus FROM dbo.Roles ORDER BY PkRol;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Usuarios_ObtenerPorId
 @Id int
AS
BEGIN
 SET NOCOUNT ON;
 SELECT PkUsuario, Nombre, UserName, FkRol FROM dbo.Usuarios WHERE PkUsuario = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Roles_ObtenerPorId
 @Id int
AS
BEGIN
 SET NOCOUNT ON;
 SELECT PkRol, Nombre, Estatus FROM dbo.Roles WHERE PkRol = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Usuarios_Crear
 @Nombre nvarchar(max), @UserName nvarchar(max), @PasswordHash nvarchar(max), @FkRol int
AS
BEGIN
 SET NOCOUNT ON;
 SET XACT_ABORT ON;
 BEGIN TRY
 BEGIN TRANSACTION;
 IF @FkRol <= 0 OR NOT EXISTS (SELECT 1 FROM dbo.Roles WITH (UPDLOCK, HOLDLOCK) WHERE PkRol = @FkRol)
 THROW 50001, 'El rol indicado no existe.', 1;
 INSERT dbo.Usuarios (Nombre, UserName, Password, FkRol) VALUES (@Nombre, @UserName, @PasswordHash, @FkRol);
 SELECT PkUsuario, Nombre, UserName, FkRol FROM dbo.Usuarios WHERE PkUsuario = CONVERT(int, SCOPE_IDENTITY());
 COMMIT;
 END TRY
 BEGIN CATCH
 IF @@TRANCOUNT > 0 ROLLBACK;
 THROW;
 END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Usuarios_Actualizar
 @Id int, @Nombre nvarchar(max), @UserName nvarchar(max), @FkRol int, @PasswordHash nvarchar(max) = NULL
AS
BEGIN
 SET NOCOUNT ON;
 SET XACT_ABORT ON;
 BEGIN TRY
 BEGIN TRANSACTION;
 IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WITH (UPDLOCK, HOLDLOCK) WHERE PkUsuario = @Id)
 BEGIN
 COMMIT;
 RETURN;
 END;
 IF @FkRol <= 0 OR NOT EXISTS (SELECT 1 FROM dbo.Roles WITH (UPDLOCK, HOLDLOCK) WHERE PkRol = @FkRol)
 THROW 50001, 'El rol indicado no existe.', 1;
 UPDATE dbo.Usuarios SET Nombre = @Nombre, UserName = @UserName, Password = COALESCE(@PasswordHash, Password), FkRol = @FkRol WHERE PkUsuario = @Id;
 SELECT PkUsuario, Nombre, UserName, FkRol FROM dbo.Usuarios WHERE PkUsuario = @Id;
 COMMIT;
 END TRY
 BEGIN CATCH
 IF @@TRANCOUNT > 0 ROLLBACK;
 THROW;
 END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Usuarios_Eliminar
 @Id int
AS
BEGIN
 SET NOCOUNT ON;
 DELETE dbo.Usuarios OUTPUT DELETED.PkUsuario, DELETED.Nombre, DELETED.UserName, DELETED.FkRol WHERE PkUsuario = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Roles_Crear
 @Nombre nvarchar(max), @Estatus bit
AS
BEGIN
 SET NOCOUNT ON;
 INSERT dbo.Roles (Nombre, Estatus) OUTPUT INSERTED.PkRol, INSERTED.Nombre, INSERTED.Estatus VALUES (@Nombre, @Estatus);
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Roles_Actualizar
 @Id int, @Nombre nvarchar(max), @Estatus bit
AS
BEGIN
 SET NOCOUNT ON;
 UPDATE dbo.Roles SET Nombre = @Nombre, Estatus = @Estatus OUTPUT INSERTED.PkRol, INSERTED.Nombre, INSERTED.Estatus WHERE PkRol = @Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Roles_Eliminar
 @Id int
AS
BEGIN
 SET NOCOUNT ON;
 SET XACT_ABORT ON;
 BEGIN TRY
 BEGIN TRANSACTION;
 DECLARE @LockedId int;
 SELECT @LockedId = PkRol FROM dbo.Roles WITH (XLOCK, HOLDLOCK) WHERE PkRol = @Id;
 IF EXISTS (SELECT 1 FROM dbo.Usuarios WITH (UPDLOCK, HOLDLOCK) WHERE FkRol = @Id)
 THROW 50002, 'El rol está asignado a usuarios.', 1;
 DELETE dbo.Roles OUTPUT DELETED.PkRol, DELETED.Nombre, DELETED.Estatus WHERE PkRol = @Id;
 COMMIT;
 END TRY
 BEGIN CATCH
 IF @@TRANCOUNT > 0 ROLLBACK;
 THROW;
 END CATCH
END;
GO
