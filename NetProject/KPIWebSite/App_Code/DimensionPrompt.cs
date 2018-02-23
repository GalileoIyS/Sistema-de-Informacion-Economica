using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Prompt
/// </summary>
public class DimensionPrompt
{
    public int dimensionid;
    public int tablecolumn;
    public int importmode;
    public string tablename;
    public string valor;

    public DimensionPrompt()
    {
        dimensionid = -1;
        tablecolumn = -1;
        importmode = -1;
        tablename = string.Empty;
        valor = string.Empty;
    }

    public string DimensionSelect()
    {
        return " AND EXISTS (SELECT 1 FROM KPI_DIMENSION_VALUES B WHERE A.DATASETID = B.DATASETID AND DIMENSIONID = " + this.dimensionid + " AND UPPERCODIGO = translate('" + this.valor.ToUpper() + "', 'ÁÉÍÓÚÄËÏÖÜÀÈÌÒÙÂÊÎÔÛ', 'AEIOUAEIOUAEIOUAEIOU') ) ";
    }
}
