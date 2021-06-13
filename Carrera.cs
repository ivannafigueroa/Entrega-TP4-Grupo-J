using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Solicitud_Inscripcion
{
    class Carrera
    {
        public int codigoCarrera { get; set; }
        public string nombre { get; set; }


        public static List<Carrera> ListaCarreras = new List<Carrera>();

        public static bool ValidarCarreraIngresada(string CarreraIngresada)
        {
            bool flagADevolver;

            //Si el string ingresado por el usuario no se puede transformar a número, se devuelve un flag en true.

            if ((!int.TryParse(CarreraIngresada, out int num)))
            {
                flagADevolver = true;
            }
            else
            {
                Console.WriteLine("La carrera ingresada no es válida. Por favor, elegir una de las mostradas arriba.");
                flagADevolver = false;
            }

            return flagADevolver;
        }


        public static bool ValidarCarreraEnLista(string CarreraIngresada)
        {
            bool flagADevolver;

            //Se evitan las diferencias en las letras y se valida que la carrera exista en la lista (flag = true)

            if (ListaCarreras.Find(C => C.nombre.ToLower() == CarreraIngresada.ToLower()) == null)
            {
                Console.WriteLine("La carrera " + CarreraIngresada.ToLower() + " no existe. Verificar que el ingreso sea correcto.");
                flagADevolver = false;
            }
            else
            {
                flagADevolver = true;
            }

            return flagADevolver;

        }

        public static void CargarCarreras()
        {

            string Path = @"/Users/ivannafigueroa/Downloads/TP4nuevo-master/bin/Debug/Carreras.txt"; 
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


                    Carrera carrera = new Carrera();

                    carrera.codigoCarrera = Convert.ToInt32(vector[0]);
                    carrera.nombre = vector[1];

                    ListaCarreras.Add(carrera);

                }
                sr.Close();
            }


        }
        public static void MostrarCarreras()
        {
            foreach (Carrera C in ListaCarreras)
            {
                Console.WriteLine(C.nombre);

            }
        }
    }
}
