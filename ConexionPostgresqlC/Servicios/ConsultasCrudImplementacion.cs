using JdbcConexionPostgresql.Dtos;
using JdbcConexionPostgresql.Util;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdbcConexionPostgresql.Servicios
{
    /// <summary>
    /// Implementación de la interfaz de consultas a BBDD
    /// </summary>
    internal class ConsultasCrudImplementacion : ConsultasCrudInterfaz
    {

        ADto aDto = new ADto();
        List<LibroDto> listaLibros = new List<LibroDto>();
        NpgsqlCommand consulta = new NpgsqlCommand();

        String queryAll = "SELECT * FROM \"gbp_almacen\".\"gbp_alm_cat_libros\"";

        public List<LibroDto> select(NpgsqlConnection conexion, Boolean tipoSelect)
        {

            try
            {
                consulta.Connection = conexion;
                if (tipoSelect)
                {
                    //Se define y ejecuta la consulta Select
                    // muestra todo
                    consulta.CommandText = queryAll;
                    NpgsqlDataReader resultadoConsulta = consulta.ExecuteReader();

                    //Paso de DataReader a lista de alumnoDTO
                    listaLibros = aDto.readerALibroDto(resultadoConsulta);

                    foreach (LibroDto libro in listaLibros)
                    {
                        Console.WriteLine(libro.ToString());
                    }

                    int cont = listaLibros.Count();
                    Console.WriteLine("[INFORMACIÓN-ConsultasBBDD-seleccionarTodosLibros] Número de libros: " + cont);
                    // DisposeAsync para limpiar y poder reutilizarlo.
                    resultadoConsulta.DisposeAsync();
                    
                }
                else if (!tipoSelect)
                {
                    // Hago una query de todos para poder mostrar los id y libros que existen
                    consulta.CommandText = queryAll;
                    NpgsqlDataReader resultadoConsulta = consulta.ExecuteReader();

                    // Llamada a la conversión a DTO
                    listaLibros = aDto.readerALibroDto(resultadoConsulta);

                    // Muestra los id y libros, para luego pedir al usuario que pida uno en concreto
                    Console.WriteLine("-------------------------------------");
                    foreach (LibroDto libro in listaLibros)
                    {
                        Console.WriteLine(libro.dosCampos() + "\t");
                    }
                    Console.WriteLine("-------------------------------------");
                    // -----------------------------------------------
                    resultadoConsulta.DisposeAsync();


                    Console.WriteLine("Cuál desea ver?");
                    int libroElegido = Convert.ToInt32(Console.ReadLine());

                    String query = "SELECT * FROM gbp_almacen.gbp_alm_cat_libros WHERE id_libro = @libroElegido";
                    consulta.CommandText = query;
                    // Añadir valores, Consulta Parametizada
                    consulta.Parameters.AddWithValue("@libroElegido", libroElegido);
                    NpgsqlDataReader resultadoConsultaDetallada = consulta.ExecuteReader();
                    listaLibros = aDto.readerALibroDto(resultadoConsultaDetallada);
                    // Mostrar libro elegido
                    foreach (LibroDto libro in listaLibros)
                    {
                        Console.WriteLine("ID Libro: " + libro.Id_libro);
                        Console.WriteLine("Titulo: " + libro.Titulo);
                        Console.WriteLine("Autor: " + libro.Autor);
                        Console.WriteLine("Edicion: " + libro.Edicion);
                        Console.WriteLine("ISBN: " + libro.Isbn);
                    }
                    resultadoConsulta.DisposeAsync();
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("[ERROR-ConsultasBBDDImplementacion-seleccionarLibro/s] Error generando o ejecutando la consulta SQL: " + e);
            }
            Console.WriteLine("[INFO-ConsultasBBDD-seleccionarTodosLibros] Cerrando el lector de datos.");
            return listaLibros;
        }


        public List<LibroDto> CrearLibros(NpgsqlConnection conexion)
        {

            try
            {
                Console.WriteLine("Va a ingresar solo un libro o más de uno? [U = uno | M = mas libros]");
                string ingresos = Console.ReadLine();
                listaLibros.Clear();
                // 1 LIBRO
                if (ingresos.ToLower() == "u")
                {
                    Console.WriteLine("Ingrese los detalles del libro:");
                    Console.Write("Título: ");
                    string titulo = Console.ReadLine();
                    Console.Write("Autor: ");
                    string autor = Console.ReadLine();
                    Console.Write("ISBN: ");
                    string isbn = Console.ReadLine();
                    Console.Write("Edición: ");
                    int edicion = int.Parse(Console.ReadLine());

                    // Crear un objeto LibroDto con los detalles ingresados
                    LibroDto librosNuevos = new LibroDto(0, titulo, autor, isbn, edicion);
                    listaLibros.Add(librosNuevos);
                }
                // MUCHOS LIBROS
                else
                {
                    bool seguir = true;
                    while (seguir)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                     
                            Console.WriteLine("Ingrese los detalles del libro:");
                            Console.Write("Título: ");
                            string titulo = Console.ReadLine();
                            Console.Write("Autor: ");
                            string autor = Console.ReadLine();
                            Console.Write("ISBN: ");
                            string isbn = Console.ReadLine();
                            Console.Write("Edición: ");
                            int edicion = int.Parse(Console.ReadLine());

                            // Crear un objeto LibroDto con los detalles ingresados
                            LibroDto nuevoLibro = new LibroDto(0, titulo, autor, isbn, edicion);
                            listaLibros.Add(nuevoLibro);
                        }
                        
                        Console.Write("¿Desea ingresar otro libro? (S/N): ");
                        string respuesta = Console.ReadLine();
                        if (respuesta.ToLower() != "s")
                        {
                            seguir = false;
                            break; // Salir del bucle si el usuario no quiere ingresar más libros
                        }
                    }
                }
                // Insertar todos los libros ingresados en la base de datos
                if (listaLibros.Count > 0)
                {
                    Console.WriteLine("Guardando los libros en la base de datos...");
                    foreach (LibroDto libro in listaLibros)
                    {
                        InsertarLibro(conexion, libro);
                    }
                    Console.WriteLine("Todos los libros se han guardado en la base de datos.");
                }
                else
                {
                    Console.WriteLine("No se ingresaron libros para guardar.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR-IMPL-CrearLibro] No se ha podido realizar la creación de un nuevo libro.");
            }
            return listaLibros;
        }

        /// <summary>
        /// Método para insertar un libro en la base de datos
        /// </summary>
        /// <param name="conexionGenerada">Conexion con la BBDD</param>
        /// <param name="libro"></param>

        private void InsertarLibro(NpgsqlConnection conexion, LibroDto libro)
        {
            // Consulta SQL para insertar un nuevo libro
            try
            {
                string sql = "INSERT INTO gbp_almacen.gbp_alm_cat_libros (titulo, autor, isbn, edicion) VALUES (@titulo, @autor, @isbn, @edicion)";
                consulta = new NpgsqlCommand(sql, conexion);

                // Establecer los valores de los parámetros de la consulta
                consulta.Parameters.AddWithValue("@titulo", libro.Titulo);
                consulta.Parameters.AddWithValue("@autor", libro.Autor);
                consulta.Parameters.AddWithValue("@isbn", libro.Isbn);
                consulta.Parameters.AddWithValue("@edicion", libro.Edicion);

                int filasAfectadas = consulta.ExecuteNonQuery();

                // Comprobar si se insertó correctamente (una fila afectada indica éxito)
                if (filasAfectadas > 0)
                {
                    Console.WriteLine("El libro '" + libro.Titulo + "' se ha creado correctamente.");
                }
                else
                {
                    Console.WriteLine("No se pudo crear el libro '" + libro.Titulo + "'.");
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Error al crear el libro '" + libro.Titulo + "': " + e.Message);
            }
        }

        public List<LibroDto> Delete(NpgsqlConnection conexion)
        {

            try
            {
                consulta.Connection = conexion;

                consulta.CommandText = queryAll;
                NpgsqlDataReader resultadoConsulta = consulta.ExecuteReader();

                // Llamada a la conversión a DTO
                listaLibros = aDto.readerALibroDto(resultadoConsulta);

                // Muestra los id y libros, para luego pedir al usuario que pida uno en concreto
                Console.WriteLine("-------------------------------------");
                for (int i = 0; i < listaLibros.Count; i++)
                {
                    Console.WriteLine(listaLibros[i].dosCampos() + "\t");
                }
                Console.WriteLine("-------------------------------------");

                Console.WriteLine("Cuál desea eliminar?");
                int libroSeleccionado = Convert.ToInt32(Console.ReadLine());
                
                resultadoConsulta.DisposeAsync();
                string sqlBorrar = "DELETE FROM \"gbp_almacen\".\"gbp_alm_cat_libros\" WHERE id_libro = @libroSeleccionado";
                consulta.CommandText = sqlBorrar;
                consulta.Parameters.AddWithValue("@libroSeleccionado", libroSeleccionado);
                int filasAfectadas = consulta.ExecuteNonQuery();

                // Comprobar si la eliminación ha salido bien
                if (filasAfectadas > 0)
                {
                    Console.WriteLine("Libro borrado correctamente.");

                    // Se compara el ID del libro (Libro.IdLibro) con el valor de libroSeleccionado.
                    // Si la condición es verdadera (es decir, si el ID del libro coincide con libroSeleccionado),
                    // se eliminará el libro de la lista
                    listaLibros.RemoveAll(libro => libro.Id_libro == libroSeleccionado);

                    return listaLibros;
                }

                return listaLibros;
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("[ERROR-ImplemCrud-Delete] No se ha podido realizar la operación de borrar: " + e.Message);
                return listaLibros;
            }
        }

        public List<LibroDto> Update(NpgsqlConnection conexion)
        {
            try
            {
                consulta.Connection = conexion;

                consulta.CommandText = queryAll;
                NpgsqlDataReader resultadoConsulta = consulta.ExecuteReader();

                // Llamada a la conversión a DTO
                listaLibros = aDto.readerALibroDto(resultadoConsulta);

                // Muestra los id y libros, para luego pedir al usuario que pida uno en concreto
                Console.WriteLine("-------------------------------------");
                for (int i = 0; i < listaLibros.Count; i++)
                {
                    Console.WriteLine(listaLibros[i].dosCampos() + "\t");
                }
                Console.WriteLine("-------------------------------------");

                resultadoConsulta.DisposeAsync();

                // Elige el libro
                Console.WriteLine("Qué libro desea modificar");
                int libroElegido = Convert.ToInt32(Console.ReadLine());

                if (LibroEncontrado(conexion, libroElegido))
                {
                    Console.WriteLine("Nuevo título:");
                    string nuevoTitulo = Console.ReadLine();
                    Console.WriteLine("Nuevo autor:");
                    string nuevoAutor = Console.ReadLine();
                    Console.WriteLine("Nuevo ISBN:");
                    string nuevoISBN = Console.ReadLine();
                    Console.WriteLine("Nueva Edición:");
                    int nuevaEdicion = Convert.ToInt32(Console.ReadLine());

                   String query = "UPDATE gbp_almacen.gbp_alm_cat_libros SET titulo = @nuevoTitulo, autor = @nuevoAutor, isbn = @nuevoISBN, edicion = @nuevaEdicion WHERE id_libro = @libroElegido";
                    // Preparar la declaración parametrizada SQL
                   consulta.CommandText = query;

                    // Establecer los valores de los parámetros de la consulta
                    consulta.Parameters.AddWithValue("@nuevoTitulo", nuevoTitulo);
                    consulta.Parameters.AddWithValue("@nuevoAutor", nuevoAutor);
                    consulta.Parameters.AddWithValue("@nuevoISBN", nuevoISBN);
                    consulta.Parameters.AddWithValue("@nuevaEdicion", nuevaEdicion); // Nueva edición
                    consulta.Parameters.AddWithValue("@libroElegido", libroElegido); // ID del libro a actualizar

                    // Ejecutar la consulta de actualización
                    int filasAfectadas = consulta.ExecuteNonQuery();

                    // Comprobar si la actualización ha salido bien
                    if (filasAfectadas > 0)
                    {
                        Console.WriteLine("Libro actualizado correctamente.");

                        // Recuperar el libro actualizado de la base de datos
                        query = "SELECT * FROM gbp_almacen.gbp_alm_cat_libros WHERE id_libro = @libroElegido";
                        consulta.CommandText = query;
                        resultadoConsulta = consulta.ExecuteReader();

                        // Convertir el resultado a un objeto LibroDto y agregarlo a la lista
                        listaLibros = aDto.readerALibroDto(resultadoConsulta);

                        resultadoConsulta.DisposeAsync();
                        return listaLibros;
                    }
                }
                else
                {
                    Console.WriteLine("[Error-IMPLcrud-UpdateLibro] No se pudo actualizar el libro, podría no haber sido encontrado.");
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("[ERROR-IMPLCrud-Update] Error al actualizar el libro: " + e.Message);
            }

            return listaLibros;
        }

        private bool LibroEncontrado(NpgsqlConnection conexion, int libroID)
        {
            string query = "SELECT * FROM gbp_almacen.gbp_alm_cat_libros WHERE id_libro = @libroID";
            // Preparar la declaración parametrizada SQL
            try
            {
                NpgsqlCommand declaracionParametrizada = new NpgsqlCommand(query, conexion);
                declaracionParametrizada.Parameters.AddWithValue("@libroID", libroID);
                NpgsqlDataReader resultadoConsulta = declaracionParametrizada.ExecuteReader();

                if (resultadoConsulta.Read())
                {
                    resultadoConsulta.DisposeAsync();
                    return true;
                }
                else
                {
                    resultadoConsulta.DisposeAsync();
                    return false;
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("[ERROR-IMPLcrud-buscarLibro] Error al buscar el libro elegido: " + e.Message);
                return false;
            }
        }
    }
}
