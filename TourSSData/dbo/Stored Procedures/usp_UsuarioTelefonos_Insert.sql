create proc usp_UsuarioTelefonos_Insert 
	@numero nchar(12),
	@usuarioID bigint
as
begin
	begin tran
		begin try
			insert into UsuarioTelefonos(numero, usuarioID) 
			values(@numero, @usuarioID)
			commit
		end try
		begin catch
			rollback
		end catch
end
