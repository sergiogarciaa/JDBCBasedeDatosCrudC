using JdbcConexionPostgresql.Dtos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdbcConexionPostgresql.Servicios
{
    /// <summary>
    /// Interfaz que define las consultas a la BBDD
    /// </summary>
    internal interface ConsultasCrudInterfaz
    {
        /// <summary>
        /// Método que realiza la selección de libros
        /// </summary>
        /// <param name="conexion">Conexión a la base de datos PostgreSQL</param>
        /// <param name="tipoSelect">Indica si se debe seleccionar todo o un libro en concreto</param>
        /// <returns>Lista de libros</returns>
        public List<LibroDto> select(NpgsqlConnection conexion, bool tipoSelect);

        /// <summary>
        /// Método que realiza la creación de libros
        /// </summary>
        /// <param name="conexion">Conexión a la base de datos PostgreSQL</param>
        /// <returns>Lista de libros</returns>
        public List<LibroDto> CrearLibros(NpgsqlConnection conexion);

        /// <summary>
        /// Método que realiza la actualización de libros
        /// </summary>
        /// <param name="conexion">Conexión a la base de datos PostgreSQL</param>
        /// <returns>Lista de libros</returns>
        public List<LibroDto> Update(NpgsqlConnection conexion);

        /// <summary>
        /// Método que realiza el borrado de libros
        /// </summary>
        /// <param name="conexion">Conexión a la base de datos PostgreSQL</param>
        /// <returns>Lista de libros</returns>
        public List<LibroDto> Delete(NpgsqlConnection conexion);
    }

}
