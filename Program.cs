using System;

namespace Solicitud_Inscripcion
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("BIENVENIDO AL SISTEMA DE INSCRIPCIÓN A CURSOS REGULARES - CICLO PROFESIONAL");
            Console.WriteLine("Presionar ENTER para comenzar con la Seleccion de cursos.");
            Console.ReadLine();

            Seleccion_Cursos.InscribirAlumno();

            

        }
    }
}
