create proc usp_Servicios_Insert
	@descripcion nvarchar(100),
	@precio money,
	@cupos tinyint
as begin
	begin tran
		begin try
			insert into Servicios(descripcion, precio, cuposDisponibles)
			values(@descripcion, @precio, @cupos)
			commit
		end try
		begin catch
			select
				@@ERROR CodigoError,
				ERROR_MESSAGE() MensajeError,
				ERROR_LINE() LineaError

				rollback
		end catch
end