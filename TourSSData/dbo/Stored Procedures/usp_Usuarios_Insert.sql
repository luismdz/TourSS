

create proc usp_Usuarios_Insert
(
	@cedula nchar(13),
	@nombre nvarchar(50),
	@apellido nvarchar(50),
	@genero nchar(1),
	@correo nvarchar(100),
	@rolID smallint
)
as begin
	begin tran
		begin try
			declare @codigo nvarchar(128)
			set @codigo = concat('TU-', convert(nvarchar, IDENT_CURRENT('Usuarios') + 1))

			insert into Usuarios(usuarioCodigo, empCedula, empNombre, empApellido, empGenero, empCorreo, rolID)
			values(@codigo, @cedula, @nombre, @apellido, @genero, @correo, @rolID)
			commit
		end try
		begin catch
			rollback
		end catch
end
