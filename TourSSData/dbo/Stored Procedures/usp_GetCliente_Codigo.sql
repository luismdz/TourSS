create proc usp_GetCliente_Codigo
	@codigo nvarchar(128)
as begin
	select
		clienteID ID,
		clienteCodigo Codigo,
		clienteCedula Cedula,
		clienteNombre Nombre,
		clienteApellido Apellido,
		clienteGenero Genero,
		clienteCorreo Correo
	from Clientes 
	where clienteCodigo = @codigo 
end