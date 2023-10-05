using JdbcConexionPostgresql.Dtos;
using JdbcConexionPostgresql.Servicios;
using Npgsql;

namespace JdbcConexionPostgresql
{
    class Program
    {
        /// <summary>
        /// Método main de la aplicación
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ConexionBBDDInterfaz conexionBBDD = new ConexionBBDDImplementacion();
            SwitchInterfaz switchInterfaz = new SwitchImplementacion();
            NpgsqlConnection conexion = null;
            conexion = conexionBBDD.generarConexionPostgresql();

            if (conexion != null)
            {
                switchInterfaz.Switchcase(conexion);
                Console.WriteLine("[INFORMACIÓN-ConsultasPostgresqlImplementacion-seccionarTodosLibros] Cierre conexión y conjunto de datos");
                conexion.Close();
                
            }

        }
    }
}