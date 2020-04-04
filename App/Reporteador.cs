using CoreEscuela.Entidades;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CoreEscuela.App
{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObcEsc)
        {
            if (dicObcEsc ==  null)
            {
                throw new ArgumentNullException(nameof(dicObcEsc));
            }
            _diccionario = dicObcEsc;
        }

        public IEnumerable<Evaluación> GetListaEvaluaciones()
        {
            if (_diccionario.TryGetValue(LlaveDiccionario.Evaluación, out IEnumerable<ObjetoEscuelaBase> lista))
            {
                return lista.Cast<Evaluación>();
            }
            {
                return new List<Evaluación>();
            }
        }

        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(out var dummy);
        }

        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluación> listaEvaluaciones)
        {
            listaEvaluaciones = GetListaEvaluaciones();

            return (from Evaluación ev in listaEvaluaciones
                   select ev.Asignatura.Nombre).Distinct();
        }

        public Dictionary<string, IEnumerable<Evaluación>> GetDicEvaluaXAsig()
        {
            var dictaRta = new Dictionary<string, IEnumerable<Evaluación>>();

            var listaAsig = GetListaAsignaturas(out var listaEval);

            foreach (var asig in listaAsig)
            {
                var evalsAsig = from eval in listaEval
                               where eval.Asignatura.Nombre == asig
                               select eval;

                dictaRta.Add(asig, evalsAsig);
            }

            return dictaRta;
        }

        public Dictionary<String, IEnumerable<object>> GetPromedioPorAsignatura()
        {
            var rta = new Dictionary<String, IEnumerable<object>>();
            var dicEvalXAsig = GetDicEvaluaXAsig();

            foreach (var asigConEval in dicEvalXAsig)
            {
                var promsAlumn = from eval in asigConEval.Value
                            group eval by new { eval.Alumno.UniqueId, eval.Alumno.Nombre }
                            into grupoEvalsAlumno
                            select new
                            {
                                alumnoid = grupoEvalsAlumno.Key,
                                alumnoNombre = grupoEvalsAlumno.Key.Nombre,
                                promedio = grupoEvalsAlumno.Average(evaluacion => evaluacion.Nota)
                            };
                rta.Add(asigConEval.Key, promsAlumn);
            }
            return rta;
        }

    }
}
