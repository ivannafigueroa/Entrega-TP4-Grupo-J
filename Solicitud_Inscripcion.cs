using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Solicitud_Inscripcion
{
    class Solicitud_Inscripcion
    {
        public int NroRegistro { get; set; }
        public string Carrera { get; set; }
        public int codigoMateria { get; set; }
        public int codigoCurso { get; set; }
        public int codigoCursoAlt { get; set; }

        public static List<Solicitud_Inscripcion> ListaCursosConfirmados = new List<Solicitud_Inscripcion>();

        //Con Curso alternativo
        public static void AgregarIngresosASolicitudDeInscripcionConCA(int NroRegistro, string CarreraIngresada, int CodigoMateria, int CodigoCurso, int CodigoCursoAlt)
        {

            Solicitud_Inscripcion SolicitudInscripcion = new Solicitud_Inscripcion();

            //Guardo en una nueva solicitud todos los datos de la inscripción que realizó el usuario
            SolicitudInscripcion.NroRegistro = NroRegistro;
            SolicitudInscripcion.Carrera = CarreraIngresada;
            SolicitudInscripcion.codigoMateria = CodigoMateria;
            SolicitudInscripcion.codigoCurso = CodigoCurso;
            SolicitudInscripcion.codigoCursoAlt = CodigoCursoAlt;

            ListaCursosConfirmados.Add(SolicitudInscripcion);

        }

        //Sin curso alternativo
        public static void AgregarIngresosASolicitudDeInscripcionSinCA(int NroRegistro, string CarreraIngresada, int CodigoMateria, int CodigoCurso, int SinCursoAlt)
        {

            Solicitud_Inscripcion SolicitudInscripcion = new Solicitud_Inscripcion();

            //Guardo en una nueva solicitud todos los datos de la inscripción que realizó el usuario
            SolicitudInscripcion.NroRegistro = NroRegistro;
            SolicitudInscripcion.Carrera = CarreraIngresada;
            SolicitudInscripcion.codigoMateria = CodigoMateria;
            SolicitudInscripcion.codigoCurso = CodigoCurso;
            SolicitudInscripcion.codigoCursoAlt = SinCursoAlt;

            ListaCursosConfirmados.Add(SolicitudInscripcion);

        }

        public string Formato()
        {
            return string.Format("{0};{1};{2};{3};{4}", NroRegistro, Carrera,codigoMateria, codigoCurso,codigoCursoAlt);

        }


        public static void GuardarSolicitud()
        {
            string Path = @"/Users/ivannafigueroa/Downloads/TP4nuevo-master/bin/Debug/Solicitud_Inscripcion.txt";
            FileInfo FI = new FileInfo(Path);



            //StreamWriter SW = new StreamWriter(Path);

            StreamWriter SW;

            SW = File.AppendText(Path);



            foreach (Solicitud_Inscripcion S in ListaCursosConfirmados)
            {

                SW.WriteLine(S.Formato());
            }

            SW.Close();

            Console.WriteLine("Se ha guardado correctamente el archivo en la ruta: " + Path);
        }

        public static void CargarSolicitudes()
        {

            string Path = @"/Users/ivannafigueroa/Downloads/TP4nuevo-master/bin/Debug/Solicitud_Inscripcion.txt";
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




                    Solicitud_Inscripcion SolicitudInscripcion = new Solicitud_Inscripcion();

                    SolicitudInscripcion.NroRegistro = Convert.ToInt32(vector[0]);

                    ListaCursosConfirmados.Add(SolicitudInscripcion);

                }
                sr.Close();
            }


        }

        public static bool ValidarNroRegEnSolicitudes(int Registro)
        {
            bool flagADevolver;

            //Si se encuentra el registro del alumno entre las solicitudes ya generadas, se lo considera no habilitado para inscribirse y se devuelve el flag en false.

            if (ListaCursosConfirmados.Find(R => R.NroRegistro == Registro) == null)
            {
                flagADevolver = true;
            }
            else
            {
                Console.WriteLine("El número de registro " + Registro + " ya se generó una solicitud de inscripción. No puede inscribirse nuevamente.");
                flagADevolver = false;
            }

            return flagADevolver;

        }

        public static void MostrarCursos()
        {

            Console.WriteLine("Sus cursos elegidos son:");

            foreach (Solicitud_Inscripcion S in ListaCursosConfirmados)
            {

                Console.WriteLine(S.Formato());
            }

            
        }

    }
}
