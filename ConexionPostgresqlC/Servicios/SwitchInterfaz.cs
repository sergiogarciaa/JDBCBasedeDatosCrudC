﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdbcConexionPostgresql.Servicios
{
    internal interface SwitchInterfaz
    {
        public void Switchcase(NpgsqlConnection conexion);
    }
}
