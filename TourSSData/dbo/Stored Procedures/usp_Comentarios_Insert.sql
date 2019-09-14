
create proc usp_Comentarios_Insert
(
	@clienteID bigint,
	@detalle nvarchar(200)
)
as begin
	begin tran
		begin try
			insert into Comentarios(clienteID, detalle)
			values(@clienteID, @detalle)

			commit
		end try
		begin catch
			rollback
		end catch
end
