using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Solicitud_Inscripcion
{
    class Materia
    {
        public string nombre { get; set; }
        public int codigo { get; set; }
        public int carga_horaria { get; set; }
        public string carrera { get; set; }

        public static List<Materia> ListaMateriasPorCarrera = new List<Materia>();

        public static void ObtenerMateriasDisponibles(string CarreraIngresada)
        {
            //Se recorre la lista de materias por carrera y solo se le muestran al usuario las materias que pertenecen a la carrera que seleccionó.
            foreach (Materia M in ListaMateriasPorCarrera)
            {
                if (M.carrera == CarreraIngresada)
                {
                    Console.WriteLine($"Materia: {M.nombre} || Código: {M.codigo} || Horas Semanales: {M.carga_horaria}/n");
                }
            }

        }

        public static void CargarMaterias()
        {

            string Path = @"/Users/ivannafigueroa/Downloads/TP4nuevo-master/bin/Debug/MateriasPorCarrera.txt";
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


                    Materia mat = new Materia();

                    mat.nombre = vector[0];
                    mat.codigo = Convert.ToInt32(vector[1]);
                    mat.carga_horaria = Convert.ToInt32(vector[2]);
                    mat.carrera = vector[3];

                    ListaMateriasPorCarrera.Add(mat);

                }
                sr.Close();
            }
        }

        public static bool ValidarCodigoMateria(string CodigoMateriaIngresado)
        {
            bool flagADevolver;

            //Se valida el tipo del valor ingresado y que se encuentre entre el rango permitido
            if (int.TryParse(CodigoMateriaIngresado, out int CodigoMateriaValidado))
            {
                if (CodigoMateriaValidado > 0 && CodigoMateriaValidado < 1000)
                {
                    flagADevolver = true;
                }
                else
                {
                    Console.WriteLine("El código de materia ingresado no se encuentra entre el rango permitido (1-999). Por favor, intentar nuevamente.");
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
        public static bool ValidarMateriasElegidas(int CodigoMateria)
        {
            bool flagADevolver;

            //Se valida que la materia elegida no haya sido elegida y guardada anteriormente en la solicitud de inscripción.
            if (Solicitud_Inscripcion.ListaCursosConfirmados.Find(M => M.codigoMateria == CodigoMateria) == null)
            {
                flagADevolver = true;
            }
            else
            {
                Console.WriteLine("La materia que desea seleccionar ya fue agregada anteriormente a su solicitud de inscripción.");
                flagADevolver = false;
            }

            return flagADevolver;

        }


        public static bool ValidarCodigoMateriaenLista(int CodigoMateriaValidado)
        {
            bool flagADevolver;

            //Se valida que el código ingresado exista en la lista de materias por carrera.
            if (ListaMateriasPorCarrera.Find(M => M.codigo == CodigoMateriaValidado) == null)
            {
                Console.WriteLine("El código de materia ingresado no corresponde a una materia válida. Por favor, verificar el ingreso.");
                flagADevolver = false;
            }
            else
            {
                flagADevolver = true;
            }

            return flagADevolver;

        }
    }
}
