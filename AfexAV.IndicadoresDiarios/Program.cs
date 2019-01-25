using HtmlAgilityPack;
using NLog;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AfexAV.IndicadoresDiarios
{
    class Program
    {
        const string UrlBase = "https://si3.bcentral.cl/indicadoressiete/secure/";

        static void Main(string[] args)
        {
            ILogger logger = LogManager.LogFactory.GetCurrentClassLogger();
            logger.Info("Inicio");
            var datos = new List<TipoCambio>();
            try
            {
                logger.Info("No hay tc registrado para el dia {0}", DateTime.Today);

                var uri = new Uri($"{UrlBase}/indicadoresdiarios.aspx");

                var webGet = new HtmlWeb();
                if (webGet.Load(uri) is HtmlDocument document)
                {
                    var dolarNode = document.DocumentNode.CssSelect("#tblDos .filas_indicadores").Take(1);

                    datos.Add(new TipoCambio
                    {
                        Codigo = "USD",
                        Moneda = dolarNode.CssSelect("td.glosa > label").Single().InnerText,
                        Valor = decimal.Parse(dolarNode.CssSelect("td.valor > label").Single().InnerText),
                        Fecha = DateTime.Today
                    });

                    var indicadoresAll = $"{UrlBase}/{document.DocumentNode.CssSelect("#tblDos .filas_indicadores > td.verSerie  > a#hypLnk1_8").Single().Attributes["href"].Value}";
                    logger.Debug("ver todos los indicadores: {0}", indicadoresAll);
                    if (webGet.Load(indicadoresAll) is HtmlDocument document2)
                    {
                        var indiadoreNodes = document2.DocumentNode.CssSelect(".filas_lista_series").ToList();
                        foreach (var node in indiadoreNodes)
                        {
                            var href = node.CssSelect("td.verSerie > a").Single().Attributes["href"].Value;

                            //extraer la el codigo despues de TCN_
                            var code = href.Substring(href.LastIndexOf("TCN_") + 4, 3);//.Split('&').ToDictionary(d => d.Split('=')[0])["gcode"].Split('=')[1].Split('_')[1];

                            var tc = new TipoCambio
                            {
                                Codigo = code,
                                Moneda = node.CssSelect("td.glosa2").Single().InnerText,
                                Valor = decimal.Parse(node.CssSelect("td.valor").Single().InnerText),
                                Fecha = DateTime.Today
                            };
                            datos.Add(tc);
                        }
                    }
                }
              
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error General");
            }

            logger.Info("Fin");
        }
    }
}

