using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdbcConexionPostgresql.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos para generar conexiones a base de datos
    /// </summary>
    internal interface ConexionBBDDInterfaz
    {
        public NpgsqlConnection generarConexionPostgresql();
    
    }
}
