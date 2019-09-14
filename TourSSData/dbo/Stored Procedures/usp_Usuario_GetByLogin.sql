CREATE proc [dbo].[usp_Usuario_GetByLogin]
	@username nvarchar(20)
as begin
	select 
		u.usuarioID ID,
		usuarioCodigo Codigo,
		empNombre Nombre,
		empApellido Apellido,
		empGenero Genero,
		empCorreo Correo,
		empEstado Estado,
		r.descripcion [Rol]
	from Usuarios u 
	left join Roles r on u.rolID = r.rolID
	inner join Credenciales c on u.usuarioID = c.usuarioID
	where username = @username
end