using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.Util;

namespace CoreEscuela
{
    public sealed class EscuelaEngine
    {
        public Escuela Escuela { get; set; }

        public EscuelaEngine()
        {

        }

        public void Inicializar()
        {
            Escuela = new Escuela("Platzi Academy", 2012, TiposEscuela.Primaria,
            ciudad: "Bogotá", pais: "Colombia"
            );

            CargarCursos();
            CargarAsignaturas();
            CargarEvaluaciones();
        }

        public void imprimirDiccionario(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dic, bool imprEval = false)
        {
            foreach (var objkey in dic)
            {
                Printer.WriteTitle(objkey.Key.ToString());

                foreach (var val in objkey.Value)
                {
                    switch (objkey.Key)
                    {
                        case LlaveDiccionario.Evaluación:
                            if (imprEval)
                                Console.WriteLine(val);
                            break;
                        case LlaveDiccionario.Escuela:
                            Console.WriteLine("Escuela: " + val);
                            break;
                        case LlaveDiccionario.Alumno:
                            Console.WriteLine("Alumno: " + val.Nombre);
                            break;
                        case LlaveDiccionario.Curso:
                            var curtemp = val as Curso;
                            if(curtemp != null)
                            {
                                int count = curtemp.Alumnos.Count;
                                Console.WriteLine("Curso: " + val.Nombre + " Cantidad Alumnos: " + count);
                            }
                            break;
                        default:
                            Console.WriteLine(val);
                            break;
                    }
                }
            }
        }


        public Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase> > GetDiccionarioObjetos()
        {

            var diccionario = new Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase> >();

            diccionario.Add(LlaveDiccionario.Escuela, new[] { Escuela });
            diccionario.Add(LlaveDiccionario.Curso, Escuela.Cursos.Cast<ObjetoEscuelaBase>());
            
            var listaTmpAsignatura = new List<Asignatura>();
            var listaTmpAlumno = new List<Alumno>();
            var listaTmpEvaluacion = new List<Evaluación>();

            foreach (var cur in Escuela.Cursos)
            {
               listaTmpAsignatura.AddRange(cur.Asignaturas);
               listaTmpAlumno.AddRange(cur.Alumnos);
                
                foreach (var alum in cur.Alumnos)
                {
                    listaTmpEvaluacion.AddRange(alum.Evaluaciones);
                }    
            }
            diccionario.Add(LlaveDiccionario.Asignatura, listaTmpAsignatura);
            diccionario.Add(LlaveDiccionario.Alumno, listaTmpAlumno);
            diccionario.Add(LlaveDiccionario.Evaluación, listaTmpEvaluacion);
            
            return diccionario;
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            bool traeEvaluaciones = true,
            bool traeCursos = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true
            )
        {
            return GetObjetosEscuela(out conteoEvaluaciones, out int dummy, out dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            bool traeEvaluaciones = true,
            bool traeCursos = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true
            )
        {
            return GetObjetosEscuela(out conteoEvaluaciones, out conteoCursos, out int dummy, out dummy);
        }


        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            out int conteoAlumnos,
            bool traeEvaluaciones = true,
            bool traeCursos = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true
            )
        {
            return GetObjetosEscuela(out conteoEvaluaciones, out conteoCursos, out conteoAlumnos, out int dummy);
        }


        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            bool traeEvaluaciones = true,
            bool traeCursos = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true
            )
        {
            return GetObjetosEscuela(out int dummy, out dummy, out dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            out int conteoAlumnos,
            out int conteoAsignaturas,
            bool traeEvaluaciones = true,
            bool traeCursos = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true
            )
        {
            conteoEvaluaciones = conteoCursos = conteoAlumnos = conteoAsignaturas = 0;

            var listaObj = new List<ObjetoEscuelaBase>();
            listaObj.Add(Escuela);

            if (traeCursos)
            listaObj.AddRange(Escuela.Cursos);
            conteoCursos += Escuela.Cursos.Count;

            foreach (var curso in Escuela.Cursos)
            {
                if(traeAsignaturas)
                listaObj.AddRange(curso.Asignaturas);
                conteoAsignaturas += curso.Asignaturas.Count;

                if(traeAlumnos)
                listaObj.AddRange(curso.Alumnos);
                conteoAlumnos += curso.Alumnos.Count;
                
                if (traeEvaluaciones)
                {
                    if (traeEvaluaciones)
                    {
                        foreach (var alumno in curso.Alumnos)
                        {
                            listaObj.AddRange(alumno.Evaluaciones);
                            conteoEvaluaciones += alumno.Evaluaciones.Count;
                        }
                    }
                    
                }
                
            }

            return listaObj.AsReadOnly();
        }

        #region Métodos de Carga
        private void CargarEvaluaciones()
        {
            var rnd = new Random();
            foreach (var curso in Escuela.Cursos)
            {
                foreach (var asignatura in curso.Asignaturas)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var ev = new Evaluación
                            {
                                Asignatura = asignatura,
                                Nombre = $"{asignatura.Nombre} Ev#{i + 1}",
                                Nota = MathF.Round(
                                    5 * (float)rnd.NextDouble()
                                    ,
                                    2),
                                Alumno = alumno
                            };
                            alumno.Evaluaciones.Add(ev);
                        }
                    }
                }
            }

        }



        private void CargarAsignaturas()
        {
            foreach (var curso in Escuela.Cursos)
            {
                var listaAsignaturas = new List<Asignatura>(){
                            new Asignatura{Nombre="Matemáticas"} ,
                            new Asignatura{Nombre="Educación Física"},
                            new Asignatura{Nombre="Castellano"},
                            new Asignatura{Nombre="Ciencias Naturales"}
                };
                curso.Asignaturas = listaAsignaturas;
            }
        }

        private List<Alumno> GenerarAlumnosAlAzar(int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno { Nombre = $"{n1} {n2} {a1}" };

            return listaAlumnos.OrderBy((al) => al.UniqueId).Take(cantidad).ToList();
        }

        private void CargarCursos()
        {
            Escuela.Cursos = new List<Curso>(){
                        new Curso(){ Nombre = "101", Jornada = TiposJornada.Mañana },
                        new Curso() {Nombre = "201", Jornada = TiposJornada.Mañana},
                        new Curso{Nombre = "301", Jornada = TiposJornada.Mañana},
                        new Curso(){ Nombre = "401", Jornada = TiposJornada.Tarde },
                        new Curso() {Nombre = "501", Jornada = TiposJornada.Tarde},
            };

            Random rnd = new Random();
            foreach (var c in Escuela.Cursos)
            {
                int cantRandom = rnd.Next(5, 20);
                c.Alumnos = GenerarAlumnosAlAzar(cantRandom);
            }
        }
    }
    #endregion
}