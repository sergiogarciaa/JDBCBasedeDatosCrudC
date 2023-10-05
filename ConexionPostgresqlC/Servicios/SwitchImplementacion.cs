using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdbcConexionPostgresql.Servicios
{
    internal class SwitchImplementacion : SwitchInterfaz
    {
        public void Switchcase(NpgsqlConnection conexion)
        {
            ConsultasCrudInterfaz consultasCrud = new ConsultasCrudImplementacion();
            bool seguir = true;
            bool tipoSelect = false;
            try
            {
                do
                {
                    Console.WriteLine("1. - Select");
                    Console.WriteLine("2. - Insert");
                    Console.WriteLine("3. - Update");
                    Console.WriteLine("4. - Delete");
                    Console.WriteLine("5. - Salir");

                    Console.WriteLine("Elija la opción que desea");
                    int opcion = Convert.ToInt32(Console.ReadLine());
                    switch (opcion)
                    {
                        case 1:

                            Console.WriteLine("Quiere hacer un select de todo o de un libro en concreto?(T = Todo | C = Concreto)");
                            string respuesta = Console.ReadLine();
                            // ---------- Todos los libros ----------------
                            if (respuesta.ToLower() == "t")
                            {
                                try
                                {
                                    /*
                                     *   Se establece el bool como true para que
                                     *      cuando lo pase al Select, sepa que se tienen que mostrar todos
                                     */
                                    tipoSelect = true;
                                    consultasCrud.select(conexion, tipoSelect);

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("[ERROR-ImplementacionSwitch-TodosLosLibros] Se ha producido un error al intentar mostrar todos los libros: " +e);
                                }
                            }
                            //--------- Uno en concreto -----------------
                            else if (respuesta.ToLower() == "c")
                            {
                                try
                                {
                                    /*
                                     *   Se establece el bool como false para que
                                     *      cuando lo pase al Select, sepa que solo se busca a uno
                                     */
                                    tipoSelect = false;
                                    consultasCrud.select(conexion, tipoSelect);
                                    // Pedir cuál quiere ver

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("[ERROR-Implementacion-Switch-TodosLosLibros] Se ha producido un error al intentar mostrar libros en concreto: " + e);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No ha elegido ninguna opción correcta");
                            }
                            break;
                        case 2:
                            consultasCrud.CrearLibros(conexion);
                            break;
                        case 3:
                            consultasCrud.Update(conexion);
                            break;
                        case 4:
                            consultasCrud.Delete(conexion);
                            break;
                        case 5:
                            try
                            {
                                Console.WriteLine("La conexión se ha cerrado.");
                                conexion.Close();
                                seguir = false;
                                break;
                            }
                            catch (NpgsqlException e)
                            {
                                Console.WriteLine("No se ha podido cerrar la conexión con la base de datos.");
                            }
                            break;
                    }
                } while (seguir);
            }catch(Exception e)
            {
                Console.WriteLine("Se ha producido un error en la aplicacion y se ha detenido");
            }
           
        }
    }
}
