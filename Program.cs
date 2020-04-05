using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.App;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");

            var reporteador = new Reporteador(engine.GetDiccionarioObjetos());
            var evalList = reporteador.GetListaEvaluaciones();
            var listaAsg = reporteador.GetListaAsignaturas();
            var listaEvalXAsig = reporteador.GetDicEvaluaXAsig();
            var listaPromXAsig = reporteador.GetPromedioPorAsignatura();

            Printer.WriteTitle("Captura de una Evaluación por Consola");
            var newEval = new Evaluación();
            string nombre, notastring;
            float nota;

            WriteLine("Ingrese el nombre de la evaluación");
            Printer.PreioneEnter();
            nombre = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Printer.WriteTitle("El valor del nombre no puede estar vacio");
                WriteLine("Saliendo del programa");
            }
            else
            {
                newEval.Nombre = nombre.ToLower();
                WriteLine("El nombre ha sido capturado correctamente");
            }


            WriteLine("Ingrese la nota de la evaluación");
            Printer.PreioneEnter();
            notastring = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(notastring))
            {
                Printer.WriteTitle("El valor de la nota no puede estar vacio");
                WriteLine("Saliendo del programa");
            }
            else
            {
                try
                {
                    newEval.Nota = float.Parse(notastring);
                    if (newEval.Nota < 0 || newEval.Nota > 5)
                    {
                        throw new ArgumentOutOfRangeException("La nota debe estar entre 1 y 5"); 
                    }
                    WriteLine("La nota ha sido capturado correctamente");
                }
                catch (ArgumentOutOfRangeException arge)
                {
                    Printer.WriteTitle(arge.Message);
                    WriteLine("Saliendo del programa");
                }
                catch(Exception)
                {
                    Printer.WriteTitle("El valor de la nota no es un número valido");
                    WriteLine("Saliendo del programa");
                }
            }

            Console.ReadLine();

        }


        private static void ImpimirCursosEscuela(Escuela escuela)
        {
            Printer.WriteTitle("Cursos de la Escuela");
            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre  }, Id  {curso.UniqueId}");
                }
            }
        }

    }
}
