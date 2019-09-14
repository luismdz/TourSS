-- Stored Procedures

create proc usp_Clientes_Insert
(
	@cedula nchar(13),
	@nombre nvarchar(50),
	@apellido nvarchar(50),
	@genero nchar(1),
	@correo nvarchar(100)
)
as begin
	begin tran
		begin try
			declare @codigo nvarchar(128)
			set @codigo = concat('TC-', convert(nvarchar, IDENT_CURRENT('Clientes') + 1))

			insert into Clientes(clienteCodigo, clienteCedula, clienteNombre, clienteApellido, clienteGenero, clienteCorreo)
			values(@codigo, @cedula, @nombre, @apellido, @genero, @correo)
			commit tran
		
		end try
		begin catch
			select
				@@ERROR CodigoError,
				ERROR_MESSAGE() MensajeError,
				ERROR_LINE() LineaError
			rollback tran
		end catch
		
end
