#region Copyright © 2010-2011 Craig P Johnson [craigpj@gmail.com]
/*
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author(s) be held liable for any damages arising from
 * the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 *   1. The origin of this software must not be misrepresented; you must not
 *      claim that you wrote the original software. If you use this software
 *      in a product, an acknowledgment in the product documentation would be
 *      appreciated but is not required.
 * 
 *   2. Altered source versions must be plainly marked as such, and must not
 *      be misrepresented as being the original software.
 * 
 *   3. This notice may not be removed or altered from any source distribution.
 */
#endregion

using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class ProcessorImportTable
{
    #region Variables Privadas
    private AsyncProcessorDetail ProcessorDetail;
    #endregion

    #region Funciones Publicas
    public ProcessorImportTable(AsyncProcessorDetail processor_detail)
    {
        ProcessorDetail = processor_detail;
        Process();
    }
    public void Process()
    {
        // use the following method to update the status on the AsyncProcessorDetail
        // in the AsyncProcessManager
        AsyncProcessManager.GetProcessorDetail(ProcessorDetail.ID).UpdateStatusText("Processing has started");

        try
        {
            int datasets = 0;
            int errorCount = 0;
            int processed = 0;
            int total = 0;

            //PRIMER PASO
            //Nos aseguramos que el usuario que va a subir la información tiene una sesión abierta
            System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
            if (usr == null)
                AsyncProcessManager.FinalizeProcess(ProcessorDetail.ID);

            //SEGUNDO PASO
            //Comprobamos que en los parámetros de importación están definidos el campo Fecha e Indicador así como
            //evaluar los atributos definidos para la importación
            int? colValor = null;
            decimal? dValorActual = null;
            string sSelectA, sSelectB, sComa, sNombre;
            List<importColumns> jsonDimensions = (List<importColumns>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<importColumns>>(ProcessorDetail.Info);
            List<DimensionPrompt> vDimensions = new List<DimensionPrompt>();
            foreach (importColumns elem in jsonDimensions)
            {
                switch (elem.attrid)
                {
                    case -3:
                        colValor = elem.column;
                        break;
                    default:
                        DimensionPrompt NuevaDimension = new DimensionPrompt();
                        NuevaDimension.tablecolumn = elem.column;
                        NuevaDimension.dimensionid = elem.attrid;
                        NuevaDimension.tablename = elem.name;
                        vDimensions.Add(NuevaDimension);
                        break;
                }
            }

            //TERCER PASO
            //Recorremos todas las filas para importar
            Boolean FilaNula = false;
            List<object> jsonRows = (List<object>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(ProcessorDetail.Filas);
            foreach (object row in jsonRows)
            {
                //Nueva fila a procesar
                total++;

                List<object> jsonRow = (List<object>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(row.ToString());
                //Nos aseguramos que la fila tiene todos los datos asignados no nulos
                FilaNula = false;
                for (int i = 0; i <= colValor; i++)
                {
                    if (jsonRow[i] == null)
                        FilaNula = true;
                }
                if (FilaNula)
                    continue;

                //Inicializamos la búsqueda
                sSelectA = " AND A.DIMENSION = '" + ProcessorDetail.Dimension + "'";
                sSelectB = string.Empty;
                sNombre = string.Empty;
                sComa = string.Empty;
                using (Clases.cKPI_DATASETS objDataset = new Clases.cKPI_DATASETS())
                {

                    foreach (DimensionPrompt DimensionActual in vDimensions)
                    {
                        DimensionActual.valor = jsonRow[DimensionActual.tablecolumn].ToString();
                        sSelectA = sSelectA + DimensionActual.DimensionSelect();
                        sNombre = sNombre + sComa + DimensionActual.tablename + " = " + jsonRow[DimensionActual.tablecolumn].ToString();
                        sSelectB = sSelectB + sComa + DimensionActual.dimensionid.ToString();
                        sComa = ", ";
                    }
                    objDataset.indicatorid = ProcessorDetail.IndicatorId;
                    objDataset.userid = Convert.ToInt32(usr.ProviderUserKey);
                    if (!objDataset.bExiste(sSelectA, sSelectB))
                    {
                        datasets++;
                        objDataset.indicatorid = ProcessorDetail.IndicatorId;
                        objDataset.userid = Convert.ToInt32(usr.ProviderUserKey);
                        objDataset.nombre = sNombre;
                        objDataset.dimension = ProcessorDetail.Dimension;
                        objDataset.importid = ProcessorDetail.ID;
                        if (objDataset.bInsertar())
                        {
                            foreach (DimensionPrompt DimensionActual in vDimensions)
                            {
                                using (Clases.cKPI_DIMENSION_VALUES objDimensionValues = new Clases.cKPI_DIMENSION_VALUES())
                                {
                                    objDimensionValues.datasetid = objDataset.datasetid;
                                    objDimensionValues.dimensionid = DimensionActual.dimensionid;
                                    objDimensionValues.codigo = DimensionActual.valor;
                                    objDimensionValues.bInsertar();
                                }
                            }
                        }
                    }
                    using (Clases.cKPI_DATASET_VALUES objValor = new Clases.cKPI_DATASET_VALUES())
                    {
                        objValor.indicatorid = ProcessorDetail.IndicatorId;
                        objValor.datasetid = objDataset.datasetid.Value;
                        objValor.userid = Convert.ToInt32(usr.ProviderUserKey);
                        objValor.importid = ProcessorDetail.ID;
                        dValorActual = ConvertirStringToDecimal(jsonRow[colValor.Value].ToString());
                        if (dValorActual.HasValue)
                        {
                            switch (ProcessorDetail.Dimension)
                            {
                                //case "D":
                                //    objValor.ejercicio = ConvertirStringToInteger(jsonRow[0].ToString());
                                //    objValor.mes = ConvertirStringToInteger(jsonRow[1].ToString());
                                //    objValor.dia = ConvertirStringToInteger(jsonRow[2].ToString());
                                //    objValor.valor = dValorActual;
                                //    if (objValor.bGuardarLibre(ProcessorDetail.Modo))
                                //        processed++;
                                //    else
                                //        errorCount++;
                                //    break;
                                case "s":
                                    objValor.ejercicio = ConvertirStringToInteger(jsonRow[0].ToString());
                                    objValor.mes = ConvertirStringToInteger(jsonRow[1].ToString());
                                    objValor.semana = ConvertirStringToInteger(jsonRow[2].ToString());
                                    objValor.valor = dValorActual;
                                    if (objValor.bGuardarSemana(ProcessorDetail.Modo))
                                        processed++;
                                    else
                                        errorCount++;
                                    break;
                                case "Q":
                                    objValor.ejercicio = ConvertirStringToInteger(jsonRow[0].ToString());
                                    objValor.mes = ConvertirStringToInteger(jsonRow[1].ToString());
                                    objValor.quincena = ConvertirStringToInteger(jsonRow[2].ToString());
                                    objValor.valor = dValorActual;
                                    if (objValor.bGuardarQuincena(ProcessorDetail.Modo))
                                        processed++;
                                    else
                                        errorCount++;
                                    break;
                                case "M":
                                    objValor.ejercicio = ConvertirStringToInteger(jsonRow[0].ToString());
                                    objValor.mes = ConvertirStringToInteger(jsonRow[1].ToString());
                                    objValor.valor = dValorActual;
                                    if (objValor.bGuardarMes(ProcessorDetail.Modo))
                                        processed++;
                                    else
                                        errorCount++;
                                    break;
                                case "T":
                                    objValor.ejercicio = ConvertirStringToInteger(jsonRow[0].ToString());
                                    objValor.trimestre = ConvertirStringToInteger(jsonRow[1].ToString());
                                    objValor.valor = dValorActual;
                                    if (objValor.bGuardarTrimestre(ProcessorDetail.Modo))
                                        processed++;
                                    else
                                        errorCount++;
                                    break;
                                case "S":
                                    objValor.ejercicio = ConvertirStringToInteger(jsonRow[0].ToString());
                                    objValor.semestre = ConvertirStringToInteger(jsonRow[1].ToString());
                                    objValor.valor = dValorActual;
                                    if (objValor.bGuardarSemestre(ProcessorDetail.Modo))
                                        processed++;
                                    else
                                        errorCount++;
                                    break;
                                case "A":
                                    objValor.ejercicio = ConvertirStringToInteger(jsonRow[0].ToString());
                                    objValor.valor = dValorActual;
                                    if (objValor.bGuardarEjercicio(ProcessorDetail.Modo))
                                        processed++;
                                    else
                                        errorCount++;
                                    break;
                                case "L":
                                    objValor.fecha = ConvertirStringToFecha(jsonRow[0].ToString());
                                    objValor.valor = dValorActual;
                                    if (objValor.bGuardarLibre(ProcessorDetail.Modo))
                                        processed++;
                                    else
                                        errorCount++;
                                    break;
                            }
                        }
                        else
                        {
                            errorCount++;
                            if (!dValorActual.HasValue)
                                InsertaDetalleError(total, jsonRow[colValor.Value].ToString(), "El valor especificado no es un campo de NÚMERO válido");
                        }
                    }
                }
                // in loops, call the UpdateStatus method to determine if you should update the counts
                // on the current iteration. This will save processing time so your not updating the
                // counts on every iteration
                if (UpdateStatus(processed, total))
                {
                    // use the following method to update the counts on the AsyncProcessorDetail
                    // in the AsyncProcessManager. This should be included inside any worker loops
                    // you may have or between code blocks to update the user
                    AsyncProcessManager.GetProcessorDetail(ProcessorDetail.ID).UpdateCounts(processed, errorCount, 0, total);
                }
            }
            using (Clases.cKPI_IMPORTS objImport = new Clases.cKPI_IMPORTS())
            {
                objImport.importid = ProcessorDetail.ID;
                objImport.num_datasets = datasets;
                objImport.num_data_ok = processed;
                objImport.num_data_error = errorCount;
                objImport.finalizado = true;
                objImport.bModificar();
            }

            // check to see if the process has been cancelled by calling AsyncProcessManager.Continue
            // this method should be called within an UpdateStatus condition within any loops you have
            // and should be followed by a break;
            if (!AsyncProcessManager.Continue(ProcessorDetail.ID))
                AsyncProcessManager.GetProcessorDetail(ProcessorDetail.ID).UpdateStatusText("Processing cancelled");

            AsyncProcessManager.GetProcessorDetail(ProcessorDetail.ID).UpdateStatusText("Processing is complete");
        }
        catch (Exception ex)
        {
            string error = ex.Message + ex.StackTrace;
        }

        AsyncProcessManager.FinalizeProcess(ProcessorDetail.ID);
    }
    #endregion

    #region Funciones Privadas
    private bool UpdateStatus(int processed, int total)
    {
        // determins if the status should be updated based on the RefreshStatusCount and the current
        // value of the processed counter  
        if (processed > 0)
            if (((processed % ProcessorDetail.RefreshStatusCount) == 0) || (processed == total))
                return true;
            else return false;
        else
            return false;
    }
    private static DateTime? ConvertirStringToFecha(string Fecha)
    {
        //string[] cultureNames = { "en-US", "en-GB", "fr-FR", "fi-FI", "es-ES" };
        System.Globalization.CultureInfo[] culturesInfo = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures);

        foreach (System.Globalization.CultureInfo cultureName in culturesInfo)
        {
            string[] fmts = cultureName.DateTimeFormat.GetAllDateTimePatterns();

            DateTime dResultado;
            if (DateTime.TryParseExact(Fecha, fmts, System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None,
                                    out dResultado))
                return dResultado;
        }
        return null;
    }
    private static Decimal? ConvertirStringToDecimal(string Valor)
    {
        Decimal dResultado;
        if (Decimal.TryParse(Valor, out dResultado))
            return dResultado;
        return null;
    }
    private static int? ConvertirStringToInteger(string Valor)
    {
        int dResultado;
        if (int.TryParse(Valor, out dResultado))
            return dResultado;
        return null;
    }
    private void InsertaDetalleError(int fila, string texto, string error)
    {
        using (Clases.cKPI_IMPORT_DETAILS objDetalle = new Clases.cKPI_IMPORT_DETAILS())
        {
            objDetalle.importid = ProcessorDetail.ID;
            objDetalle.fila = fila;
            objDetalle.cadena = texto;
            objDetalle.mensaje = error;
            objDetalle.bInsertar();
        }
    }
    #endregion
}
