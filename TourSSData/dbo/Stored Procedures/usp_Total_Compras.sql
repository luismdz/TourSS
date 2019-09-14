/*
create proc usp_Pagos_Insert
(
	@reservacionID bigint,
	@fecha date = null,
	@tipo nchar(3),
	@concepto nvarchar(50) = null
)
as begin
	begin tran
		begin try
			declare @monto money = 0
			
			select @monto = rd.subtotal + rd.itbis 
			from ReservacionDetalle rd
			inner join Reservaciones r on r.reservacionID = rd.reservacionID

		end try
		begin catch

		end catch
end
go
*/
CREATE proc [dbo].[usp_Total_Compras]
as begin
	select 
		r.reservacionID [ID de la Reservacion],
		r.clienteID [ID de Cliente],
		convert(date, r.fecha, 103) [Fecha de la Reservacion],
		cast(sum(subtotal) as decimal(10,2)) Subtotal,
		cast(sum(itbis) as decimal) ITBIS,
		sum(subtotal) + sum(itbis) Total
	from Reservaciones r
	inner join ReservacionDetalle rd on r.reservacionID = rd.reservacionID
	inner join Clientes c on r.clienteID = c.clienteID
	group by r.reservacionID, r.clienteID, r.fecha
end
