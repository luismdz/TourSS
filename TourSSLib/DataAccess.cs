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
        /// Obtiene todos los records existentes en la tabla "Clientes" en la Base de Datos
        /// </summary>
        /// <returns></returns>
        //public List<ClienteModel> GetAllClientes()
        //{
        //    List<ClienteModel> Clientes = new List<ClienteModel>();

        //    using (IDbConnection db = new SqlConnection(ConnectionString(database)))
        //    {
        //        Clientes = db.Query<ClienteModel>("dbo.usp_Clientes_GetAll").ToList();

        //        foreach(var cliente in Clientes)
        //        {
        //            var p = new DynamicParameters();
        //            p.Add("@id", cliente.ID);
        //            cliente.Telefonos = db.Query<string>("dbo.usp_TelefonosCliente_Get @id", p).ToList();
        //        }

        //    }
        //    return Clientes;
        //}

        


        public ClienteModel BuscarClienteCodigo(string codigo)
        {
            var cliente = new ClienteModel();
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionString(database)))
                {
                    cliente = db.QuerySingle<ClienteModel>("dbo.usp_GetCliente_Codigo @codigo", new { Codigo = codigo });

                    var id = new DynamicParameters();
                    id.Add("@id", cliente.ID);

                    cliente.Telefonos = db.Query<string>("dbo.usp_TelefonosCliente_Get @id", id).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return cliente;
        }

        public List<ClienteModel> BuscarClientesNombre(string nombre)
        {
            List<ClienteModel> clientes = new List<ClienteModel>();

            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionString(database)))
                {
                    var p = new DynamicParameters();
                    p.Add("@var", nombre);

                    clientes = db.Query<ClienteModel>("dbo.usp_BuscarClienteByName @var", p).ToList();

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

        public List<ClienteModel> BuscarClientesCedula(string cedula)
        {
            List<ClienteModel> clientes = new List<ClienteModel>();

            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionString(database)))
                {
                    var p = new DynamicParameters();
                    p.Add("@var", cedula);

                    clientes = db.Query<ClienteModel>("dbo.usp_BuscarClienteByCedula @var", p).ToList();

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
        /// Obtiene todos los records existentes en la tabla "Servicios" en la base de datos
        /// </summary>
        /// <returns></returns>
        public List<ServicioModel> GetAllServicios()
        {
            List<ServicioModel> Servicios = new List<ServicioModel>();
            using (IDbConnection db = new SqlConnection(ConnectionString(database)))
            {
                Servicios = db.Query<ServicioModel>("dbo.usp_Servicios_GetAll").ToList();
                
            }
            return Servicios;
        }

        /// <summary>
        /// Obtiene todos los records existentes en la tabla "Usuarios" en la base de datos
        /// </summary>
        /// <returns></returns>
        public List<UsuarioModel> GetAllUsuarios()
        {
            List<UsuarioModel> usuarios = new List<UsuarioModel>();
            using (IDbConnection db = new SqlConnection(ConnectionString(database)))
            {
                usuarios = db.Query<UsuarioModel>("dbo.usp_Usuario_GetAll").ToList();

            }
            return usuarios;
        }
        
        /// <summary>
        /// Valida que exista el Usuario en la base de datos al ingresar sus credenciales
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        //public bool Validar(string username, string pass)
        //{
        //    int output = 1;

        //    using (IDbConnection db = new SqlConnection(ConnectionString(database)))
        //    {
        //        var p = new DynamicParameters();
        //        p.Add("@username", username);
        //        p.Add("@pass", pass);

        //        output = db.Query<int>("dbo.usp_ValidarCredenciales @username, @pass", p).SingleOrDefault();
        //    }
        //    if (output == 0)
        //        return false;
        //    else
        //        return true;
        //}

        public (UsuarioModel, bool) GetUsuarioByLogin(string u, string p)
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

        //public UsuarioModel GetUsuarioByLogin(string u, string p)
        //{
        //    UsuarioModel usuario = new UsuarioModel();

        //    using (IDbConnection db = new SqlConnection(ConnectionString(database)))
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("@username", u);
        //        param.Add("@password", p);

        //        usuario = db.Query<UsuarioModel>("dbo.usp_Usuario_GetByLogin @username, @password", param).SingleOrDefault();
        //    }
        //    return usuario;
        //}


    }
}
