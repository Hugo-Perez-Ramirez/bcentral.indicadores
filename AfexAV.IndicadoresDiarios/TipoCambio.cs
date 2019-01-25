using System;

namespace AfexAV.IndicadoresDiarios
{
    public class TipoCambio
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Moneda { get; set; }
        public decimal Valor { get; set; }
        public DateTime Fecha { get; set; }

    }
}
