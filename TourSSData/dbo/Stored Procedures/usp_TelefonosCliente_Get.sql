create proc usp_TelefonosCliente_Get
	@id bigint
as begin
	select
		numero [Telefonos]
	from Clientes c inner join ClienteTelefonos ct
	on c.clienteID = ct.clienteID
	where c.clienteID = @id
	order by c.clienteID
end