create proc usp_Clientes_GetAll
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
end
