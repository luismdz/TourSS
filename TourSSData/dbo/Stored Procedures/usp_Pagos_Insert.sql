
create proc usp_Pagos_Insert
(
	@reservacionID bigint,
	@tipo nchar(3),
	@concepto nvarchar(50)
)
as begin
	begin tran
		begin try
			declare @monto money = 0

			select @monto = rd.subtotal + rd.itbis
			from ReservacionDetalle rd
			inner join Reservaciones r 
			on r.reservacionID = @reservacionID
			
			if @concepto is null
				set @concepto = ''

			insert into Pagos(reservacionID, monto, tipo, concepto)
			values(@reservacionID, @monto, @tipo, @concepto)
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
