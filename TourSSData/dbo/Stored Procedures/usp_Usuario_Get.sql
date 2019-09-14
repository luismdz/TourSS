create proc usp_Usuario_Get 
	@id bigint
as begin
	select
		usuarioID ID,
		usuarioCodigo Codigo,
		empNombre Nombre,
		empApellido Apellido,
		empGenero Genero,
		empCorreo Correo,
		empEstado Estado,
		r.descripcion [Rol]
	from Usuarios u left join Roles r
	on u.rolID = r.rolID
	where usuarioID = @id
end