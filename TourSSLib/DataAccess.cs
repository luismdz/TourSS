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

                    foreach (var cliente in clientes)
                    {
                        var id = new DynamicParameters();
                        id.Add("@id", cliente.ID);
                        cliente.Telefonos = db.Query<string>("dbo.usp_TelefonosCliente_Get @id", id).ToList();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return clientes;
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
