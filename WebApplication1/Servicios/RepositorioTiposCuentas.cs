using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioid);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        public readonly string connectionString;
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                ($@"Insert Into TiposCuentas (Nombre, UsuarioId,Orden)
                                                    values (@Nombre, @UsuarioId,0);
                                                    SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;

        }
        //Evitar que ingresen datos repetidos
        public async Task<bool> Existe(string nombre, int usuarioid)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>($"select 1 from TiposCuentas where" +
                $" Nombre=@Nombre and UsuarioId=UsuarioId;", new { nombre, usuarioid }); //Traeme el primer valor por defecto y si no hay traeme un cero.;

            return existe == 1;
        }

        //Metodo listar

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"select Id,Nombre,Orden from TiposCuentas 
                                                            where UsuarioId=@UsuarioId", new { usuarioId });
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"Update TiposCuentas set nombre=@nombre where id=@id", tipoCuenta);
        }

        public async Task<TipoCuenta>ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"select id, nombre, orden from TiposCuentas where id =@id and usuarioId=@usuarioId", new { id, usuarioId });
        }

        public async Task Borrar (int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE TiposCuentas where id=@id", new {id});
        }
    }
}
                 