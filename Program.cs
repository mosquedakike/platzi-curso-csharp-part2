﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            //Printer.Beep(10000, cantidad: 10);
            ImpimirCursosEscuela(engine.Escuela);


            Dictionary<int, string> diccionario = new Dictionary<int, string>();
            diccionario.Add(10, "kike");
            diccionario.Add(23, "aprender a programar es dificil pero muy emocionante");

            foreach (var keyValPair in diccionario)
            {
                Console.WriteLine($"key: {keyValPair.Key} Valor: {keyValPair.Value}");
            }

            Printer.WriteTitle("Acceso a diccionario");
            WriteLine(diccionario[23]);
            Printer.WriteTitle("Otro diccionario");
            var dic = new Dictionary<string, string>();
            dic.Add("C#","Es un lenguaje muy chevere");
            Console.WriteLine(dic["C#"]);
            dic["ASP"] = "Es la proxima tencologia que tengo que aprender";
            Console.WriteLine(dic["ASP"]);

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
