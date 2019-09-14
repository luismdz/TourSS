
CREATE proc [dbo].[usp_Servicios_GetAll]
as begin
	select 
		servicioID ID, 
		descripcion Descripcion, 
		cast(precio as decimal(10,2)) Precio, 
		cuposDisponibles CuposDisponibles
	from Servicios
end
