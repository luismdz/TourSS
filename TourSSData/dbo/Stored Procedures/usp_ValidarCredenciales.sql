﻿create proc usp_ValidarCredenciales
	@username nvarchar(20),
	@pass nchar(8)
as begin
	select count(1)
	from credenciales
	where username = @username and password = @pass
end