
create proc usp_Reservacion_Insert
(
	@usuarioID bigint,
	@clienteID bigint,
	@servicioID bigint,
	@cantidad int,
	@fecha datetime = null
)
as begin
	begin tran
		begin try
			declare
				@reservacionID bigint,
				@precio money = 0,
				@subtotal money = 0,
				@itbis money = 0,
				@cupos_disp int = 0

				if @fecha is null
					set @fecha = GETDATE()

				insert into Reservaciones(usuarioID, clienteID, fecha)
				values(@usuarioID, @clienteID, @fecha)
				print 'Reservacion creada'

				select
					@precio = s.precio,
					@cupos_disp = s.cuposDisponibles
				from Servicios s
				where s.servicioID = @servicioID

				set @reservacionID = SCOPE_IDENTITY()
				set	@subtotal = @precio * @cantidad
				set @itbis = @subtotal * 0.18

				if @cantidad <= @cupos_disp
				begin
					insert into ReservacionDetalle(reservacionID, servicioID, precio, cantidad, subtotal, itbis)
					values(@reservacionID, @servicioID, @precio, @cantidad, @subtotal, @itbis)
					print 'Detalle de Reservacion insertada'

					update Servicios
					set cuposDisponibles -= @cantidad
					where servicioID = @servicioID

					commit tran
				end
				else
				begin
					print 'Lo sentimos, cantidad disponible = ' + convert(nvarchar, @cupos_disp)
					rollback
				end
		end try
		begin catch
			select
				@@ERROR CodigoError,
				ERROR_MESSAGE() MensajeError,
				ERROR_LINE() LineaError
			rollback tran
		end catch
		select @reservacionID
end
