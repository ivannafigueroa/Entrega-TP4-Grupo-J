using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Solicitud_Inscripcion
{
    class Curso
    {
        public int codigoMateria { get; set; }
        public int codigoCurso { get; set; }
        public string Profesor { get; set; }
        public string Dias { get; set; }
        public string horario { get; set; }

        public static List<Curso> ListaCursos = new List<Curso>();
        public static List<Curso> ListaCursosDeMaterias = new List<Curso>();
        public static List<Curso> Lista_Inscripcion_Cursos = new List<Curso>();

        public static void MostrarCursosDeMateria()
        {
            foreach (Curso curso in ListaCursosDeMaterias)
            {
                Console.WriteLine($"Código de curso : {curso.codigoCurso} || Profesor: {curso.Profesor} || Días de cursada: {curso.Dias} || Horario: {curso.horario}");
            }

        }

        public static bool ValidarCodigoCurso(string CodigoCursoIngresado)
        {
            bool flagADevolver;

            //Se valida el tipo del valor ingresado y que se encuentre entre el rango permitido
            if (int.TryParse(CodigoCursoIngresado, out int CodigoCursoValidado))
            {
                if (CodigoCursoValidado > 0 && CodigoCursoValidado < 1000)
                {
                    flagADevolver = true;
                }
                else
                {
                    Console.WriteLine("El código de curso ingresado no se encuentra entre el rango permitido (1-999). Por favor, intentar nuevamente.");
                    flagADevolver = false;
                }
            }
            else
            {
                Console.WriteLine("El ingreso no es válido. Por favor, intentar nuevamente.");
                flagADevolver = false;
            }

            return flagADevolver;

        }


        public static bool ValidarCodigoCursoEnLista(int CodigoCursoValidado)
        {
            bool flagADevolver;

            if (ListaCursosDeMaterias.Find(C => C.codigoCurso == CodigoCursoValidado) == null)
            {
                Console.WriteLine("El código de curso ingresado no corresponde a un curso válido. Por favor, verificar el ingreso.");
                flagADevolver = false;
            }
            else
            {
                flagADevolver = true;
            }

            return flagADevolver;

        }


        public static void CargarCursosCorrespondientesAMateria(int CodigoMateria)
        {

            string Path = @"/Users/ivannafigueroa/Downloads/TP4nuevo-master/bin/Debug/Cursos.txt";
            FileInfo FI = new FileInfo(Path);

            if (!FI.Exists)
            {
                Console.WriteLine("No existe el archivo de la ruta: " + Path);
            }
            else
            {

                StreamReader sr = FI.OpenText();

                while (!sr.EndOfStream)
                {

                    string p = sr.ReadLine();
                    string[] vector = p.Split(';');

                    Curso curso = new Curso();

                    curso.codigoMateria = Convert.ToInt32(vector[0]);

                    //verifico de agregar a la lista de cursos que mostraré al usuario, solo los cursos que coincidan con el código de materia ingresado
                    if (curso.codigoMateria == CodigoMateria)
                    {
                        curso.codigoCurso = Convert.ToInt32(vector[1]);
                        curso.Profesor = vector[2];
                        curso.Dias = vector[3];
                        curso.horario = vector[4];

                        ListaCursosDeMaterias.Add(curso);
                    }
                }
                sr.Close();
            }

        }
    }
}
