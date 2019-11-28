using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TourSSLib.Models;

namespace TourSSLibrary
{
    public class DataAccess
    {
        private const string database = "TourSSDB";

        //Propiedades
        public IList<ClienteModel> Clientes
        {
            get
            {
                var clientes = GetAll<ClienteModel>("Clientes");
                var telefonos = TelefonosClientes;

                foreach (var c in clientes)
                {
                    c.Telefonos = telefonos.Where(t => t.clienteID == c.ID).ToList();
                }
                return clientes;
            }

        }

        public IList<UsuarioModel> Usuarios
        {
            get
            {
                var usuarios = GetAll<UsuarioModel>("Usuarios");
                var telefonos = TelefonosUsuarios;

                foreach (var u in usuarios)
                {
                    u.Credencial = Credenciales.Where(x => x.UsuarioID == u.ID).SingleOrDefault();
                    u.Telefonos = telefonos.Where(t => t.usuarioID == u.ID).ToList();
                    u.Rol = Roles.Where(x => x.ID == u.RolID).FirstOrDefault();
                }
                return usuarios;
            }
        }

        public IList<TelefonoModel> TelefonosUsuarios { get => GetAll<TelefonoModel>($"Telefonos_Usuarios").ToList(); }
        public IList<TelefonoModel> TelefonosClientes { get => GetAll<TelefonoModel>($"Telefonos_Clientes").ToList(); }
        public IList<ServicioModel> Servicios { get => GetAll<ServicioModel>("Servicios"); }
        public IList<UbicacionModel> Ubicaciones { get => GetAll<UbicacionModel>("Paises"); }
        public IList<Credencial> Credenciales { get => Query<Credencial>("select * from Credenciales"); }
        public IList<ReservacionModel> Reservaciones { get => Query<ReservacionModel>("select * from Reservaciones"); }
        public IList<ReservacionDetalle> ReservacionDetalle { get => Query<ReservacionDetalle>("select * from ReservacionDetalle"); }
        public List<Rol> Roles { get => Query<Rol>(
            $"select " +
            $"rolID as ID " +
            $",case when descripcion like '%Administrador Base de Datos%'" +
            $"then 'DBA' else upper(descripcion) end as Descripcion " +
            $"from Roles");
        }

        public string ConnectionString(string value) => 
            ConfigurationManager.ConnectionStrings[value].ConnectionString;

        public DataAccess() { }

        /// <summary>
        /// Query dinamico para retornar informacion de la DB
        /// </summary>
        public List<T> Query<T>(string qry)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString(database)))
            {
                var result = db.Query<T>(qry).ToList();
                return result;
            }
        }

        /// <summary>
        /// Metodo para obtener todos los registros de tipo <T> de manera generica
        /// </summary>
        private IList<T> GetAll<T>(string type)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString(database)))
            {
                var Items = db.Query<T>($"dbo.usp_{type}_GetAll").ToList();
                return Items;
            }
        }

        public long Insert<T>(T item)
        {
            using (var conn = new SqlConnection(ConnectionString(database)))
            {
                conn.Open();

                // Begin the transaction
                using (var transaction = conn.BeginTransaction())
                {
                    var param = new DynamicParameters();
                    int affectedRows = 0;
                    long newID = 0;

                    if (item is ClienteModel)
                    {
                        var c = item as ClienteModel;

                        param.Add("cedula", c.Cedula);
                        param.Add("nombre", c.Nombre);
                        param.Add("apellido", c.Apellido);
                        param.Add("genero", c.Genero);
                        param.Add("correo", c.Correo);

                        affectedRows = conn.Execute("usp_Clientes_Insert", param, commandType: CommandType.StoredProcedure, transaction: transaction);

                        newID = Convert.ToInt64(conn.ExecuteScalar<object>("select MAX(clienteID) from Clientes", null, transaction: transaction));

                        foreach (var t in c.Telefonos)
                        {
                            affectedRows = conn.Execute($"insert into ClienteTelefonos(clienteID, numero) values({newID}, '{t.Numero}')", transaction: transaction);
                        }
                    }
                    
                    else if (item is UsuarioModel)
                    {
                        var c = item as UsuarioModel;

                        param.Add("cedula", c.Cedula);
                        param.Add("nombre", c.Nombre);
                        param.Add("apellido", c.Apellido);
                        param.Add("genero", c.Genero);
                        param.Add("correo", c.Correo);
                        param.Add("rolID", c.Rol.ID);

                        affectedRows = conn.Execute("usp_Usuarios_Insert", param, commandType: CommandType.StoredProcedure, transaction: transaction);

                        newID = Convert.ToInt64(conn.ExecuteScalar<object>("select MAX(usuarioID) from Usuarios", null, transaction: transaction));

                        foreach (var t in c.Telefonos)
                        {
                            affectedRows = conn.Execute($"insert into UsuarioTelefonos(usuarioID, numero) values({newID}, '{t.Numero}')", transaction: transaction);
                        }
                    }
                    else if(item is ServicioModel)
                    {
                        var s = item as ServicioModel;

                        param.Add("descripcion", s.Descripcion);
                        param.Add("precio", s.Precio);
                        param.Add("cupos", s.CuposDisponibles);
                        param.Add("fechaDesde", s.fechaDesde.Date);
                        param.Add("fechaHasta", s.fechaHasta.Date);
                        param.Add("paisID", s.UbicacionID);

                        affectedRows = conn.Execute("usp_Servicios_Insert", param, commandType: CommandType.StoredProcedure, transaction: transaction);
                        newID = Convert.ToInt64(conn.ExecuteScalar<object>("select MAX(servicioID) from Servicios", null, transaction: transaction));
                    }
                    else if(item is Credencial)
                    {
                        var c = item as Credencial;
                        affectedRows = conn.Execute($"insert into Credenciales(username, password, usuarioID) VALUES('{c.Username}', '{c.Password}', {c.UsuarioID})", 
                            transaction: transaction);

                        transaction.Commit();
                        return affectedRows;
                    }
                    // Commit tran
                    transaction.Commit();
                    return newID;
                }
            }
        }

        public int Edit<T>(T item)
        {
            using (var conn = new SqlConnection(ConnectionString(database)))
            {
                var param = new DynamicParameters();

                if (item is ClienteModel)
                {
                    var s = item as ClienteModel;
                    param.Add("id", s.ID);
                    param.Add("nombre", s.Nombre);
                    param.Add("apellido", s.Apellido);
                    param.Add("genero", s.Genero);
                    param.Add("correo", s.Correo);

                    var affectedRows = conn.Execute("usp_Editar_Cliente", param, commandType: CommandType.StoredProcedure);
                    return affectedRows;
                }

                else if (item is UsuarioModel)
                {
                    var s = item as UsuarioModel;
                    param.Add("id", s.ID);
                    param.Add("nombre", s.Nombre);
                    param.Add("apellido", s.Apellido);
                    param.Add("genero", s.Genero);
                    param.Add("correo", s.Correo);

                    var affectedRows = conn.Execute("usp_Editar_Usuario", param, commandType: CommandType.StoredProcedure);
                    return affectedRows;
                }
                else if(item is ServicioModel)
                {
                    var s = item as ServicioModel;
                    param.Add("id", s.ID);
                    param.Add("descripcion", s.Descripcion);
                    param.Add("precio", s.Precio);
                    param.Add("cupos", s.CuposDisponibles);
                    param.Add("fechaDesde", s.fechaDesde);
                    param.Add("fechaHasta", s.fechaHasta);
                    param.Add("paisID", s.UbicacionID);

                    var affectedRows = conn.Execute("usp_Editar_Servicio", param, commandType: CommandType.StoredProcedure);
                    return affectedRows;
                }
                else
                    return 0;
                
            }
        }
        /// <summary>
        /// Retornar informacion de las ventas para el reporte
        /// </summary>
        /// <param name="tipoDeReporte"></param>
        /// <returns></returns>
        public List<ReporteModel> GenerarReporte(int? tipoDeReporte)
        {
            List<ReporteModel> registros = new List<ReporteModel>();
            List<ReservacionDetalle> detalles = new List<ReservacionDetalle>();
            try
            { 
                using (IDbConnection db = new SqlConnection(ConnectionString(database)))
                {
                    //
                    switch(tipoDeReporte)
                    {
                        case 1:
                            registros = db.Query<ReporteModel>($"Select * from dbo.Summary_Sales_by_Dates").ToList();
                            break;
                        case 2:
                            registros = db.Query<ReporteModel>($"usp_Sales_by_Customers").ToList();
                            break;

                    }

                    foreach (var r in registros)
                    {
                        r.Reservacion = Reservaciones.Where(o => o.ReservacionID == r.ReservacionID).FirstOrDefault();
                        r.Cliente = Clientes.Where(c => c.ID == r.ClienteID).FirstOrDefault();
                        r.Detalles = ReservacionDetalle.Where(d => d.ReservacionID == r.ReservacionID).ToList();
                    }
                    return registros;
                }
            }
            catch(SqlException)
            {
                return null;
            }  
        }

            /// <summary>
            /// Inserta en Reservacion y ReservacionDetalle
            /// </summary>
            /// <param name="items">Lista de Servicios a reservar</param>
            /// <param name="clienteID">Id del cliente de la reservacion</param>
            /// <param name="usuarioID">Id del usuario que genera la reservacion</param>
            /// <returns></returns>
        public string Reservar(IList<ReservacionDetalle> items, long clienteID, long usuarioID)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString(database)))
                {
                    conn.Open();

                    // Begin the transaction
                    using(var transaction = conn.BeginTransaction())
                    {
                        //Crea la reservacion
                        var paramMaster = new DynamicParameters();
                        paramMaster.Add("usuarioID", usuarioID);
                        paramMaster.Add("clienteID", clienteID);

                        var affectedRows = conn.Execute("usp_SetReservacion", paramMaster, commandType: CommandType.StoredProcedure, transaction: transaction);

                        //Obtener ultimo ID registrado en Reservaciones
                        long newID = Convert.ToInt64(conn.ExecuteScalar<object>("select max(reservacionID) from Reservaciones", null, transaction: transaction));
        
                        //Iterar elementos de la lista de Reservaciones
                        foreach(var item in items)
                        {
                            var paramDetails = new DynamicParameters();
                            paramDetails.Add("@reservacionID", newID);
                            paramDetails.Add("@servicioID", item.Servicio.ID);
                            paramDetails.Add("@cantidad", item.Cantidad);

                            // Insertar linea en ReservacionDetalle
                            affectedRows = conn.Execute("usp_SetReservacionDetalle", paramDetails, commandType: CommandType.StoredProcedure, transaction: transaction);
                        }

                        // Commit tran
                        transaction.Commit();
                        return newID.ToString();
                    }
                }
            }
            catch (SqlException e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// Valida credenciales de usuario para entrar en el sistema
        /// </summary>
        /// <param name="u"> Username </param>
        /// <param name="p"> Password </param>
        public (UsuarioModel, bool) GetUserByLogin(string u, string p)
        {
            UsuarioModel usuario = new UsuarioModel();
            bool esValido = false;

            using (IDbConnection db = new SqlConnection(ConnectionString(database)))
            {
                var param = new DynamicParameters();
                param.Add("@username", u);
                param.Add("@password", p);

                esValido = db.Query<bool>("dbo.usp_ValidarCredenciales @username, @password", param).SingleOrDefault();

                if (esValido)
                {
                    usuario = db.Query<UsuarioModel>("dbo.usp_Usuario_GetByLogin @username, @password", param).SingleOrDefault();
                    usuario.Rol = Roles.Where(x => x.ID == usuario.RolID).SingleOrDefault();
                }
                
                else
                    usuario = null;
            }
            return (usuario, esValido);
        }

    }
}
