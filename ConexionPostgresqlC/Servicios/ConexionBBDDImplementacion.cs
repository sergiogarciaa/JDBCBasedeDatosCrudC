using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdbcConexionPostgresql.Servicios
{
    /// <summary>
    /// Implementación de la interfaz de conexión a BBDD
    /// </summary>
    internal class ConexionBBDDImplementacion : ConexionBBDDInterfaz
    {
        public NpgsqlConnection generarConexionPostgresql()
        {
            //Se lee la cadena de conexión a la base de datos a traves del archivo de configuración
            string stringConexionPostgresql = ConfigurationManager.ConnectionStrings["stringConexion"].ConnectionString;

            NpgsqlConnection conexion = null;
            string estado = "";

            if (!string.IsNullOrWhiteSpace(stringConexionPostgresql))
            {
                try
                {
                    conexion = new NpgsqlConnection(stringConexionPostgresql);
                    conexion.Open();
                    //Se obtiene el estado de conexión para informarlo por consola
                    estado = conexion.State.ToString();
                    if (estado.Equals("Open")) {

                        Console.WriteLine("[INFORMACIÓN-ConexionPostgresqlImplementacion-generarConexionPostgresql] Estado conexión: " + estado);

                    }
                    else
                    {
                        conexion = null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR-ConexionPostgresqlImplementacion-generarConexionPostgresql] Error al generar la conexión:" + e);
                    conexion = null;
                    return conexion;

                }
            }

            return conexion;

        }
    }
}
