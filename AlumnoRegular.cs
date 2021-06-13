using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Solicitud_Inscripcion
{
    class AlumnoRegular
    {
        public int NroRegistro { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public static List<AlumnoRegular> ListaAlumnosRegulares = new List<AlumnoRegular>();


        public static bool ValidarNroRegEnLista(int Registro)
        {
            bool flagADevolver;

            //Si se encuentra el registro del alumno en la lista cargada, se lo considera habilitado para inscribirse y se devuelve el flag en true.

            if (ListaAlumnosRegulares.Find(R => R.NroRegistro == Registro) == null)
            {
                Console.WriteLine("El número de registro " + Registro + " no se encuentra en el listado de alumnos regulares. Intentar nuevamente.");
                flagADevolver = false;
            }
            else
            {
                flagADevolver = true;
            }

            return flagADevolver;

        }

        public static bool ValidarNroRegistro(string registro)
        {
            bool flagADevolver;

            //se valida que sea número y se encuentre entre el rango permitido. Si ambas validaciones están bien, se devuelve el flag en true.
            if (int.TryParse(registro, out int RegistroValidado))
            {
                if (RegistroValidado > 0 && RegistroValidado < 1000000)
                {
                    flagADevolver = true;
                }
                else
                {
                    Console.WriteLine("El codigo ingresado debe estar dentro del rango permitido (1-999999).");
                    flagADevolver = false;
                }
            }
            else
            {
                Console.WriteLine("No se ingresó un número válido. Por favor, intentar nuevamente.");
                flagADevolver = false;
            }

            return flagADevolver;

        }

        public static void CargarAlumnos()
        {

            string Path = @"/Users/ivannafigueroa/Downloads/TP4nuevo-master/bin/Debug/AlumnosRegulares.txt";
            FileInfo FI = new FileInfo(Path);


            if (!FI.Exists)
            {
                Console.WriteLine("No existe el archivo de la ruta: " + Path + ". Por favor, comprobar que sea la ruta correcta.");
            }
            else
            {

                StreamReader sr = FI.OpenText();

                while (!sr.EndOfStream)
                {

                    string p = sr.ReadLine();

                    string[] vector = p.Split(';');




                    AlumnoRegular Alum = new AlumnoRegular();

                    Alum.NroRegistro = Convert.ToInt32(vector[0]);

                    ListaAlumnosRegulares.Add(Alum);

                }
                sr.Close();
            }


        }
    }
}
