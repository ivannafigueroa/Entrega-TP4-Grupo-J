using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using static System.Text.Json.JsonSerializer;



namespace Solicitud_Inscripcion
{
    class Seleccion_Cursos
    {
        public static void InscribirAlumno()
        {
            //Definimos todas las variables que utilizaremos
            bool flagValidaciones = false;
            bool FlagGeneral = false;

            string NroRegistro;
            string CarreraIngresada;
            string CodigoMateriaIngresado;
            string CodigoCursoIngresado;
            string CursoAlt;
            string CodigoCursoAltIngresado;
            int CodigoCursoAltValidado;
            int SinCursoAlt = 0;

            int CodigoMateriaValidado;
            int CodigoCursoValidado;
            int ContadorMateriasAgregadas = 0;
            int ContadorMateriasAprobadas = 0;

            Dictionary<int, int> HistorialAlumnos = new Dictionary<int, int>();

            string ContinuarAgregando;
            string UltimasCuatro;


            //Cargamos las listas de carreras, materias y alumnos en el programa (mediante .txt)
            Carrera.CargarCarreras();
            Materia.CargarMaterias();
            AlumnoRegular.CargarAlumnos();
            Solicitud_Inscripcion.CargarSolicitudes();
            HistorialAlumnos.Clear();
            Solicitud_Inscripcion.ListaCursosConfirmados.Clear();


            //Solicitamos el ingreso del número de registro para validar el alumno. Se pedirá hasta que se ingrese un número válido (flagValidaciones = true).
            //Tener en cuenta que este paso no aparece en el caso de uso ya que se valida su identidad cuando el alumno ingresa al campus.

            do
            {
                Console.WriteLine("Ingresar su número de registro antes de empezar:");
                NroRegistro = Console.ReadLine();

                //Primero validamos el tipo de ingreso
                if (AlumnoRegular.ValidarNroRegistro(NroRegistro))
                {
                    //Luego validamos que se encuentre entre los alumnos regulares habilitados
                    if (AlumnoRegular.ValidarNroRegEnLista(Convert.ToInt32(NroRegistro)))
                    {
                        //Luego validamos que se el alumno no haya generado una solicitud previamente
                        if (Solicitud_Inscripcion.ValidarNroRegEnSolicitudes(Convert.ToInt32(NroRegistro)))
                        {
                            flagValidaciones = true;
                        }
                        else
                        {
                            flagValidaciones = false;
                        }
                    }
                    else
                    {
                        flagValidaciones = false;
                    }

                }
                else
                {
                    flagValidaciones = false;
                }

            } while (flagValidaciones == false);

            do
            {
                Console.WriteLine("Se encuentra cursando las últimas 4 materias de la carrera? (S/N)");
                UltimasCuatro = Console.ReadLine();

            } while (UltimasCuatro.ToLower() != "s" && UltimasCuatro.ToLower() != "n");

            //Se empiezan a solicitar los ingresos para la inscripción a cursos
            do
            {
                Curso.ListaCursosDeMaterias.Clear();
                ContinuarAgregando = "";

                //Se muestran las carreras para que el alumno elija en la que se anotará.
                Carrera.MostrarCarreras();

                Console.WriteLine("Ingresar el nombre de la carrera en la que se inscribirá: ");
                CarreraIngresada = Console.ReadLine();

                //Primero se valida el tipo de dato ingresado
                flagValidaciones = Carrera.ValidarCarreraIngresada(CarreraIngresada);

                if (flagValidaciones == true)
                {
                    //Luego se valida que la carrera exista en el listado de carreras cargadas
                    FlagGeneral = Carrera.ValidarCarreraEnLista(CarreraIngresada);

                    if (FlagGeneral == true)
                    {
                        //Una vez validada la carrera, se muestran en pantalla las materias disponibles para inscripción.
                        Console.WriteLine($"Presione [ENTER] para ver las materias disponibles de {CarreraIngresada}");
                        Console.ReadKey();

                        Materia.ObtenerMateriasDisponibles(CarreraIngresada);


                        //Una vez que tenemos la carrera validada, pasamos a validar las materias que el alumno ya aprobó para evaluar que no se inscriba en la misma materia y sus correlativas.
                        //Si el usuario aún no aprobó ninguna materia de la carrera, este paso se saltea.
                        string TieneMatAprobadas;
                        string MateriasAprobadas;
                        string MateriasAprobadas2;
                        string NewMateriaAprobada;
                        int MateriasAprobadas2Validadas;

                        Console.WriteLine("");
                        Console.WriteLine("Como primer paso validaremos las materias ya aprobadas. Para esto, se le solicitará ingresar uno a uno los códigos de las materias ya aprobadas.");

                        do
                        {
                            Console.WriteLine("Ya aprobó materias previamente? S/N");
                            TieneMatAprobadas = Console.ReadLine();

                            if (TieneMatAprobadas.ToLower() == "s")
                            {
                                do
                                {
                                    Console.WriteLine("Ingresar el codigo de una materia que ya hayas aprobado:");
                                    MateriasAprobadas = Console.ReadLine();
                                    flagValidaciones = Materia.ValidarCodigoMateria(MateriasAprobadas);

                                    if (flagValidaciones == true)
                                    {
                                        CodigoMateriaValidado = Convert.ToInt32(MateriasAprobadas);
                                        HistorialAlumnos.Add(CodigoMateriaValidado, Convert.ToInt32(NroRegistro));
                                        //Console.WriteLine(Serialize(HistorialAlumnos.ToList()));

                                        //Se valida que el código ingresado exista en la lista de materias.
                                        flagValidaciones = Materia.ValidarCodigoMateriaenLista(CodigoMateriaValidado);
                                        if (flagValidaciones == true)
                                        {
                                            do
                                            {
                                                Console.WriteLine("Desea agregar otra materia aprobada? (S/N)");
                                                NewMateriaAprobada = Console.ReadLine();

                                            } while (NewMateriaAprobada.ToLower() != "s" && NewMateriaAprobada.ToLower() != "n");

                                            if (NewMateriaAprobada.ToUpper() == "S")
                                            {
                                                do
                                                {
                                                    Console.WriteLine("Ingresar el código de la siguiente materia aprobada: ");
                                                    MateriasAprobadas2 = Console.ReadLine();

                                                    if (MateriasAprobadas != MateriasAprobadas2)
                                                    {
                                                        flagValidaciones = Materia.ValidarCodigoMateria(MateriasAprobadas2);
                                                        if (flagValidaciones == true)
                                                        {
                                                            MateriasAprobadas2Validadas = Convert.ToInt32(MateriasAprobadas2);
                                                            HistorialAlumnos.Add(MateriasAprobadas2Validadas, Convert.ToInt32(NroRegistro));
                                                            //Console.WriteLine(Serialize(HistorialAlumnos.ToList()));

                                                            if (Materia.ValidarCodigoMateriaenLista(MateriasAprobadas2Validadas))
                                                            {
                                                                if (MateriasAprobadas2Validadas == CodigoMateriaValidado)
                                                                {
                                                                    Console.WriteLine("No puede ingresar el mismo código que antes");
                                                                    flagValidaciones = false;
                                                                    continue;
                                                                }
                                                                else
                                                                {
                                                                    //Preguntara al usuario si desea agregar otra materia
                                                                    if (ContadorMateriasAprobadas < 100)
                                                                    {
                                                                        do
                                                                        {
                                                                            Console.WriteLine("Desea agregar otra materia aprobada? (S/N)");
                                                                            ContinuarAgregando = Console.ReadLine();

                                                                            string Path = @"/Users/ivannafigueroa/Downloads/TP4nuevo-master/bin/Debug/MateriasAprobadas.txt";
                                                                            FileInfo FI = new FileInfo(Path);

                                                                            //StreamWriter SW = new StreamWriter(Path);

                                                                            StreamWriter SW;

                                                                            SW = File.AppendText(Path);



                                                                            foreach (KeyValuePair<int, int> H in HistorialAlumnos)
                                                                            {

                                                                                SW.WriteLine(H);
                                                                            }

                                                                            SW.Close();

                                                                            Console.WriteLine("Se ha guardado correctamente el archivo en la ruta: " + Path);

                                                                        } while (ContinuarAgregando.ToLower() != "s" && ContinuarAgregando.ToLower() != "n");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                FlagGeneral = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            flagValidaciones = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Este código ya fue ingresado previamente.");
                                                        FlagGeneral = false;
                                                    }
                                                } while (flagValidaciones == false || FlagGeneral == false);
                                            }
                                        }
                                    }
                                } while (flagValidaciones == false);
                            }
                        }while (TieneMatAprobadas.ToLower() != "s" && TieneMatAprobadas.ToLower() != "n");

                 //Se pide al usuario que ingrese el código de la materia en la que se inscribirá y se valida el ingreso.
                Console.WriteLine("Ingresar el código de la materia a la que desea inscribirse: ");
                CodigoMateriaIngresado = Console.ReadLine();

                flagValidaciones = Materia.ValidarCodigoMateria(CodigoMateriaIngresado);

                if (flagValidaciones == true)
                {
                    CodigoMateriaValidado = Convert.ToInt32(CodigoMateriaIngresado);

             
                    //Se valida que la materia no haya sido elegida aún en la inscripción.
                    if (!Materia.ValidarMateriasElegidas(CodigoMateriaValidado))
                    {
                        flagValidaciones = false;
                        continue;
                    }


                    //Se valida que no se haya aprobado aún
                    if (HistorialAlumnos.ContainsKey(CodigoMateriaValidado))
                    {
                        Console.WriteLine("Esa materia ya fue aprobada. Elija otra.");
                        flagValidaciones = false;
                        continue;
                    }

                    //Se valida que el código ingresado exista en la lista de materias.
                    if (Materia.ValidarCodigoMateriaenLista(CodigoMateriaValidado) == true)
                    {
                                //Se valida si tiene las correlativas necesarias

                                string PathCorrelativas = @"/Users/ivannafigueroa/Downloads/TP4nuevo-master/bin/Debug/MateriasCorrelativas.txt";

                                using (StreamReader sr = new StreamReader(PathCorrelativas))
                                {
                                    String line;

                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        string[] correlativas = line.Split(";");
                                        if (correlativas[1] == CodigoMateriaValidado.ToString())
                                        {
                                            for (int i = 2; i < correlativas.Length; i++)
                                            {
                                                if (correlativas[i] != "0")
                                                {
                                                    bool _existe = HistorialAlumnos.ContainsKey(Convert.ToInt32(correlativas[i]));
                                                    if (_existe == false)
                                                    {
                                                        Console.WriteLine("No tiene la correlativa "+ correlativas[i]+ " necesaria para cursar esa materia. Elija otra.");
                                                        flagValidaciones = false;
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (flagValidaciones == false)
                                {
                                    continue;
                                }

                                //Una vez validada la materia, se muestran en pantalla los cursos disponibles para la misma.
                                Console.WriteLine($"Presione [ENTER] para ver los cursos disponibles de la materia {CodigoMateriaValidado}");
                        Console.ReadKey();

                        //Cargo en la lista de cursos solo los que correspondan a la materia ingresada y los muestro al usuario.
                        Curso.CargarCursosCorrespondientesAMateria(CodigoMateriaValidado);
                        Curso.MostrarCursosDeMateria();

                        do
                        {
                            //Se pide al usuario que ingrese el código del curso en el que se inscribirá y se valida el ingreso.
                            Console.WriteLine("Ingresar el código del curso al que desea inscribirse: ");
                            CodigoCursoIngresado = Console.ReadLine();

                            flagValidaciones = Curso.ValidarCodigoCurso(CodigoCursoIngresado);

                            if (flagValidaciones == true)
                            {
                                CodigoCursoValidado = Convert.ToInt32(CodigoCursoIngresado);

                                //Se valida que el código ingresado exista en la lista de cursos.
                                if (Curso.ValidarCodigoCursoEnLista(CodigoCursoValidado))
                                {
                                    do
                                    {
                                        Console.WriteLine("Desea agregar un curso alternativo? (S/N)");
                                        CursoAlt = Console.ReadLine();

                                    } while (CursoAlt.ToLower() != "s" && CursoAlt.ToLower() != "n");

                                    if (CursoAlt.ToUpper() == "S")
                                    {
                                        do
                                        {
                                            Console.WriteLine("Ingresar el código del curso alternativo al que desea inscribirse: ");
                                            CodigoCursoAltIngresado = Console.ReadLine();

                                            flagValidaciones = Curso.ValidarCodigoCurso(CodigoCursoIngresado);

                                            if (flagValidaciones == true)
                                            {
                                                CodigoCursoAltValidado = Convert.ToInt32(CodigoCursoAltIngresado);

                                                if (Curso.ValidarCodigoCursoEnLista(CodigoCursoAltValidado))
                                                {
                                                    if (CodigoCursoAltValidado == CodigoCursoValidado)
                                                    {
                                                        Console.WriteLine("No puede ingresar el mismo código que el curso principal");
                                                        flagValidaciones = false;
                                                        continue;
                                                    }
                                                    //se toma el codigo que eligio y se fija en MateriasCorrelativas si tiene correlativa o no
                                                    //if los datos de las correlativas son distintos de cero, agarro esos numeros. los guardo. voy a materiasaprobadasporalumno y me fijo (con el nroregistro) si tiene esos codigos aprobados. si los tiene aprobados sigo con el programa. sino, tiro un msj de error.
                                                    //if los datos de las correlativas son iguales a cero, no hago nada porque no tiene correlativas.
                                                    else
                                                    {
                                                        //Una vez que se validaron todos los ingresos se guardan en la solicitud de inscripción
                                                        Solicitud_Inscripcion.AgregarIngresosASolicitudDeInscripcionConCA(Convert.ToInt32(NroRegistro), CarreraIngresada, CodigoMateriaValidado, CodigoCursoValidado, CodigoCursoAltValidado);
                                                        flagValidaciones = true;
                                                        FlagGeneral = true;

                                                        //Al finalizar de agregar una materia, sumo una al contador. El programa dejara agregar hasta 3 materias.
                                                        ContadorMateriasAgregadas++;

                                                        //Preguntara al usuario si desea agregar otra materia, siempre que no haya agregado 3.
                                                        if (ContadorMateriasAgregadas < 4 && UltimasCuatro.ToLower() == "s")
                                                        {
                                                            do
                                                            {
                                                                Console.WriteLine("Desea agregar otra materia a la solicitud? (S/N)");
                                                                ContinuarAgregando = Console.ReadLine();

                                                            } while (ContinuarAgregando.ToLower() != "s" && ContinuarAgregando.ToLower() != "n");
                                                        }
                                                        else if (ContadorMateriasAgregadas < 3 && UltimasCuatro.ToLower() == "n")
                                                        {
                                                            do
                                                            {
                                                                Console.WriteLine("Desea agregar otra materia a la solicitud? (S/N)");
                                                                ContinuarAgregando = Console.ReadLine();

                                                            } while (ContinuarAgregando.ToLower() != "s" && ContinuarAgregando.ToLower() != "n");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    FlagGeneral = false;
                                                }
                                            }
                                            else
                                            {
                                                flagValidaciones = false;
                                            }
                                        } while (flagValidaciones == false || FlagGeneral == false);
                                    }
                                    else
                                    {
                                        //Una vez que se validaron todos los ingresos se guardan en la solicitud de inscripción
                                        Solicitud_Inscripcion.AgregarIngresosASolicitudDeInscripcionSinCA(Convert.ToInt32(NroRegistro), CarreraIngresada, CodigoMateriaValidado, CodigoCursoValidado, SinCursoAlt);
                                        flagValidaciones = true;
                                        FlagGeneral = true;

                                        //Al finalizar de agregar una materia, sumo una al contador. El programa dejara agregar hasta 3 materias.
                                        ContadorMateriasAgregadas++;

                                        //Preguntara al usuario si desea agregar otra materia, siempre que no haya agregado 3.
                                        if (ContadorMateriasAgregadas < 4 && UltimasCuatro.ToLower() == "s")
                                        {
                                            do
                                            {
                                                Console.WriteLine("Desea agregar otra materia a la solicitud? (S/N)");
                                                ContinuarAgregando = Console.ReadLine();

                                            } while (ContinuarAgregando.ToLower() != "s" && ContinuarAgregando.ToLower() != "n");
                                        }
                                        else if (ContadorMateriasAgregadas < 3 && UltimasCuatro.ToLower() == "n")
                                        {
                                            do
                                            {
                                                Console.WriteLine("Desea agregar otra materia a la solicitud? (S/N)");
                                                ContinuarAgregando = Console.ReadLine();

                                            } while (ContinuarAgregando.ToLower() != "s" && ContinuarAgregando.ToLower() != "n");
                                        }

                                    }
                                }
                                else
                                {
                                    FlagGeneral = false;
                                }

                            }
                            else
                            {
                                flagValidaciones = false;
                            }

                        } while (flagValidaciones == false || FlagGeneral == false);
                    }
                    else
                    {
                        FlagGeneral = false;
                    }
                        }
                    }

                }








            } while (flagValidaciones == false || FlagGeneral == false || ContinuarAgregando.ToLower() == "s");

            Solicitud_Inscripcion.MostrarCursos();

            string ConfirmarSolicitud = "";

            do
            {
                Console.WriteLine("Desea confirmar la solicitud? S/N");
                ConfirmarSolicitud = Console.ReadLine();

            } while (ConfirmarSolicitud.ToLower() != "s" && ConfirmarSolicitud.ToLower() != "n");

            if(ConfirmarSolicitud.ToLower() == "s")
            {
                Solicitud_Inscripcion.GuardarSolicitud();
                Console.WriteLine("Se generó el comprobante de inscripción correctamente");
                Console.WriteLine("Presionar [ENTER] tecla para finalizar.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Su solicitud fue cancelada. Presione ENTER para salir.");
                Console.ReadKey();
            }
            

        }

        

    } 
}
