using CoreEscuela.Entidades;
using System;
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
            _diccionario[LlaveDiccionario.Evaluación];
        }
    }
}
