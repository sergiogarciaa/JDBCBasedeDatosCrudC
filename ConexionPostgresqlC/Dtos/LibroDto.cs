using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdbcConexionPostgresql.Dtos
{
    internal class LibroDto
    {

        //Atributos

        private long id_libro;
        private string titulo;
        private string autor;
        private string isbn;        
        private int edicion;

        //Getters y setters

        public long Id_libro { get => id_libro; set => id_libro = value; }
        public string Titulo { get => titulo; set => titulo = value; }
        public string Autor { get => autor; set => autor = value; }
        public string Isbn { get => isbn; set => isbn = value; }
        public int Edicion { get => edicion; set => edicion = value; }


        //Constructores
        //Si se genera un constructor con campos el vacía debe definirse de forma explícita
        public LibroDto(long id_libro, string titulo, string autor, string isbn, int edicion)
        {
            this.id_libro = id_libro;
            this.titulo = titulo;
            this.autor = autor;
            this.isbn = isbn;
            this.edicion = edicion;
        }


        // ToString
        public override string ToString()
        {
            return "[ID_Libro = " + id_libro + " | Titulo = " + titulo + " | Autor=" + autor + " | Isbn=" + isbn
                + " | Edicion=" + edicion + "]";
        }
        public String dosCampos()
        {
            return "[ID Libro = " + id_libro + " | Titulo = " + titulo + "]";
        }

    }
}
