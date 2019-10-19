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

        public string ConnectionString(string value) => 
            ConfigurationManager.ConnectionStrings[value].ConnectionString;

        public DataAccess() { }

        /// <summary>
        /// Metodo para obtener todos los registros de tipo <T> de manera generica
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<T> GetAll<T>(string type)
        {
            IList<T> Items = new List<T>();

            using (IDbConnection db = new SqlConnection(ConnectionString(database)))
            {
                Items = db.Query<T>($"dbo.usp_{type}_GetAll").ToList();

                if (Items is List<ClienteModel>)
                {
                    foreach (var cliente in Items as IList<ClienteModel>)
                    {
                        var p = new DynamicParameters();
                        p.Add("@id", cliente.ID);
                        cliente.Telefonos = db.Query<string>("dbo.usp_TelefonosCliente_Get @id", p).ToList();
                    }
                }
            }
            return Items;
        }

        /// <summary>
        /// Busca cliente(s) en la bd por medio de su cedula, nombre y/o apellido y codigo
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<ClienteModel> BuscarClientes(params string[] param)
        {
            List<ClienteModel> clientes = new List<ClienteModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionString(database)))
                {
                    var p = new DynamicParameters();
                    p.Add("@codigo", param[0]);
                    p.Add("@nombre", param[1]);
                    p.Add("@cedula", param[2]);

                    clientes = db.Query<ClienteModel>("dbo.usp_BuscarClientes @codigo, @nombre, @cedula", p).ToList();

                    if (clientes.Count != 0)
                    {
                        foreach (var cliente in clientes)
                        {
                            var id = new DynamicParameters();
                            id.Add("@id", cliente.ID);
                            cliente.Telefonos = db.Query<string>("dbo.usp_TelefonosCliente_Get @id", id).ToList();
                        }
                    }
                    else return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return clientes;
        }

        public List<ServicioModel> BuscarServicios(int? ubicacionID, int? servicioID, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                List<ServicioModel> servicios = new List<ServicioModel>();
                using (IDbConnection db = new SqlConnection(ConnectionString(database)))
                {
                    var p = new DynamicParameters();
                    p.Add("@paisID", ubicacionID);
                    p.Add("@fechaDesde", fechaDesde);
                    p.Add("@fechaHasta", fechaHasta);
                    p.Add("ID", servicioID);

                    servicios = db.Query<ServicioModel>("dbo.usp_BuscarServicios @paisID, @fechaDesde, @fechaHasta, @ID", p).ToList();
                }
                return servicios;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public string Reservar(IList<ReservacionModel> items, long clienteID, long usuarioID)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString(database)))
                {
                    //var p = new DynamicParameters();
                    //p.Add("@usuarioID", usuarioID);
                    //p.Add("@clienteID", clienteID);
                    //p.Add("@servicios", servicios.AsTableValuedParameter("List"));
                    //p.Add("@fecha", fecha);

                    //var affectedRows = conn.Execute("Test", p, commandType: CommandType.StoredProcedure);
                    //return affectedRows.ToString();
                    conn.Open();

                    // Begin the transaction
                    using(var transaction = conn.BeginTransaction())
                    {
                        //Crea la reservacion
                        var paramMaster = new DynamicParameters();
                        paramMaster.Add("usuarioID", usuarioID);
                        paramMaster.Add("clienteID", clienteID);

                        var affectedRows = conn.Execute("usp_SaveReservacion", paramMaster, commandType: CommandType.StoredProcedure, transaction: transaction);

                        //Obtener ultimo ID registrado en Reservaciones
                        long newID = Convert.ToInt64(conn.ExecuteScalar<object>("SELECT @@IDENTITY", null, transaction: transaction));

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
        /// <param name="u"></param>
        /// <param name="p"></param>
        /// <returns></returns>
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
                    usuario = db.Query<UsuarioModel>("dbo.usp_Usuario_GetByLogin @username, @password", param).SingleOrDefault();
                else
                    usuario = null;
            }
            return (usuario, esValido);
        }

    }
}
