
create proc usp_TelefonoCliente_Insert
(
	@numero nchar(12),
	@clienteID bigint
)
as begin
	begin tran
		begin try
			insert into ClienteTelefonos(numero, clienteID)
			values(@numero, @clienteID)

			commit
		end try
		begin catch
			select
				@@ERROR CodigoError,
				ERROR_MESSAGE() MensajeError,
				ERROR_LINE() LineaError
			rollback tran
		end catch
end
