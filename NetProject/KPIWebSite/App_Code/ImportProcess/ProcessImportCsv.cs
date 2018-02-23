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

public class ProcessorImportCsv
{
    #region Variables Privadas
    private AsyncProcessorDetail ProcessorDetail;
    #endregion

    #region Funciones Publicas
    public ProcessorImportCsv(AsyncProcessorDetail processor_detail)
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
            int? colFecha = null;
            int? colValor = null;
            List<importColumns> jsonDimensions = (List<importColumns>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<importColumns>>(ProcessorDetail.Info);
            List<DimensionPrompt> vDimensions = new List<DimensionPrompt>();
            foreach (importColumns elem in jsonDimensions)
            {
                switch (elem.attrid)
                {
                    case -3:
                        colValor = elem.column;
                        break;
                    case -2:
                        colFecha = elem.column;
                        break;
                    case -1:
                        break;
                    default:
                        DimensionPrompt NuevaDimension = new DimensionPrompt();
                        NuevaDimension.tablecolumn = elem.column;
                        NuevaDimension.dimensionid = elem.attrid;
                        NuevaDimension.tablename = elem.name;
                        NuevaDimension.importmode = 0;
                        vDimensions.Add(NuevaDimension);
                        break;
                }
            }
            List<importAttributes> jsonAttributes = (List<importAttributes>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<importAttributes>>(ProcessorDetail.Atributtes);
            foreach (importAttributes elem in jsonAttributes)
            {
                DimensionPrompt NuevaDimension = new DimensionPrompt();
                NuevaDimension.dimensionid = elem.id;
                NuevaDimension.valor = elem.name;
                NuevaDimension.importmode = 1;
                vDimensions.Add(NuevaDimension);
            }

            //Nos aseguramos que existe el campo Fecha y el campo valor
            if ((!colValor.HasValue) || (!colFecha.HasValue))
                AsyncProcessManager.FinalizeProcess(ProcessorDetail.ID);

            //TERCER PASO
            //Comprobamos que el fichero se subió correctamente y que por lo tanto existe en el disco
            //duro para leer
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString() + "/" + ProcessorDetail.FileName);
            if (!System.IO.File.Exists(path))
                AsyncProcessManager.FinalizeProcess(ProcessorDetail.ID);

            //create object for CSVReader and pass the stream
            using (System.IO.FileStream fs = System.IO.File.OpenRead(path))
            {
                csv.csvReader reader = new csv.csvReader(fs, ProcessorDetail.Separator);

                //get the header
                string[] headers = reader.GetCSVLine();
                System.Data.DataTable TablaDeDatos = new System.Data.DataTable();

                string[] data;
                DateTime? dFechaActual = null;
                decimal? dValorActual = null;
                string sSelectA, sSelectB, sComa, sNombre;

                while ((data = reader.GetCSVLine()) != null)
                {
                    //Nueva fila a procesar
                    total++;

                    //Inicializamos la búsqueda
                    sSelectA = string.Empty;
                    sSelectB = string.Empty;
                    sNombre = string.Empty;
                    sComa = string.Empty;

                    //Para cada dimension especificada buscamos aquellos datasets en las que coinciden todas sus dimensiones
                    using (Clases.cKPI_DATASETS objDataset = new Clases.cKPI_DATASETS())
                    {
                        foreach (DimensionPrompt DimensionActual in vDimensions)
                        {
                            if (DimensionActual.importmode == 0)
                            {
                                DimensionActual.valor = data[DimensionActual.tablecolumn].ToString().Replace("'", "");
                                sNombre = sNombre + sComa + DimensionActual.tablename + " = " + data[DimensionActual.tablecolumn].ToString().Replace("'","");
                            }
                            else if (DimensionActual.importmode == 1)
                                sNombre = sNombre + sComa + DimensionActual.valor;
                            sSelectA = sSelectA + DimensionActual.DimensionSelect();
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
                            if (string.IsNullOrEmpty(sNombre))
                                objDataset.nombre = "undefined";
                            else
                                objDataset.nombre = sNombre;
                            objDataset.dimension = "L";
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
                                        objDimensionValues.bInsertarFromImport();
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
                            dFechaActual = ConvertirStringToFecha(data[colFecha.Value]);
                            dValorActual = ConvertirStringToDecimal(data[colValor.Value]);
                            if ((dFechaActual.HasValue) && (dValorActual.HasValue))
                            {
                                switch (objDataset.dimension)
                                {
                                    case "D":
                                        objValor.fecha = dFechaActual;
                                        objValor.valor = dValorActual;
                                        if (objValor.bGuardarLibre(ProcessorDetail.Modo))
                                            processed++;
                                        else
                                            errorCount++;
                                        break;
                                    case "s":
                                        objValor.ejercicio = dFechaActual.Value.Year;
                                        objValor.mes = dFechaActual.Value.Month;
                                        if (dFechaActual.Value.Day < 8)
                                            objValor.semana = 1;
                                        else if (dFechaActual.Value.Day < 15)
                                            objValor.semana = 2;
                                        else if (dFechaActual.Value.Day < 22)
                                            objValor.semana = 3;
                                        else
                                            objValor.semana = 4;
                                        objValor.valor = dValorActual;
                                        if (objValor.bGuardarSemana(ProcessorDetail.Modo))
                                            processed++;
                                        else
                                            errorCount++;
                                        break;
                                    case "Q":
                                        objValor.ejercicio = dFechaActual.Value.Year;
                                        objValor.mes = dFechaActual.Value.Month;
                                        objValor.quincena = dFechaActual.Value.Day < 16 ? 1 : 2;
                                        objValor.valor = dValorActual;
                                        if (objValor.bGuardarQuincena(ProcessorDetail.Modo))
                                            processed++;
                                        else
                                            errorCount++;
                                        break;
                                    case "M":
                                        objValor.ejercicio = dFechaActual.Value.Year;
                                        objValor.mes = dFechaActual.Value.Month;
                                        objValor.valor = dValorActual;
                                        if (objValor.bGuardarMes(ProcessorDetail.Modo))
                                            processed++;
                                        else
                                            errorCount++;
                                        break;
                                    case "T":
                                        objValor.ejercicio = dFechaActual.Value.Year;
                                        if (dFechaActual.Value.Month < 4)
                                            objValor.trimestre = 1;
                                        else if (dFechaActual.Value.Month < 7)
                                            objValor.trimestre = 2;
                                        else if (dFechaActual.Value.Month < 10)
                                            objValor.trimestre = 3;
                                        else
                                            objValor.trimestre = 4;
                                        objValor.valor = dValorActual;
                                        if (objValor.bGuardarTrimestre(ProcessorDetail.Modo))
                                            processed++;
                                        else
                                            errorCount++;
                                        break;
                                    case "S":
                                        objValor.ejercicio = dFechaActual.Value.Year;
                                        objValor.semestre = dFechaActual.Value.Month < 7 ? 1 : 2;
                                        objValor.valor = dValorActual;
                                        if (objValor.bGuardarSemestre(ProcessorDetail.Modo))
                                            processed++;
                                        else
                                            errorCount++;
                                        break;
                                    case "A":
                                        objValor.ejercicio = dFechaActual.Value.Year;
                                        objValor.valor = dValorActual;
                                        if (objValor.bGuardarEjercicio(ProcessorDetail.Modo))
                                            processed++;
                                        else
                                            errorCount++;
                                        break;
                                    case "L":
                                        objValor.fecha = dFechaActual;
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
                                if (!dFechaActual.HasValue)
                                    InsertaDetalleError(total, data[colFecha.Value].ToString(), "El valor especificado no es un campo de FECHA válido");
                                if (!dValorActual.HasValue)
                                    InsertaDetalleError(total, data[colValor.Value].ToString(), "El valor especificado no es un campo de NÚMERO válido");
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
    private DateTime? ConvertirStringToFecha(string Fecha)
    {
        DateTime dResultado;
        string CommaFormat = ProcessorDetail.DatetimeFormat.Replace('.', ',');
        if (DateTime.TryParseExact(Fecha, CommaFormat, System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None,
                                out dResultado))
            return dResultado;
        string DotFormat = ProcessorDetail.DatetimeFormat.Replace(',', '.');
        if (DateTime.TryParseExact(Fecha, DotFormat, System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None,
                                out dResultado))
            return dResultado;
        return null;
    }
    private Decimal? ConvertirStringToDecimal(string Valor)
    {
        Decimal dResultado;
        if (Decimal.TryParse(Valor, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out dResultado))
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
