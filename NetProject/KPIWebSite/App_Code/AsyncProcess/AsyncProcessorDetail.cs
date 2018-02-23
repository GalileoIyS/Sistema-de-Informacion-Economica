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

public class AsyncProcessorDetail
{
    // types, identifiers and control properties
    public int ID { get; set; }
    public string ProcessorType { get; set; }
    public bool Process { get; set; }
    public bool Processing { get; set; }
    public bool Stop { get; set; }
    public bool Complete { get; set; }
    public bool Test { get; set; }

    // properties for storing counts
    public int Total { get; set; }
    public int Processed { get; set; }
    public int ErrorCount { get; set; }
    public int Duplicates { get; set; }

    // Status properties
    public int RefreshStatusCount { get; set; }
    public string StatusText { get; set; }

    // properties for dealing with files
    public string FullFilePath { get; set; }
    public string FileName { get; set; }
    
    //Timing Information
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    //Mios
    public int IndicatorId { get; set; }
    public string Info { get; set; }
    public string Filas { get; set; }
    public string Atributtes { get; set; }
    public string Dimension { get; set; }
    public string DatetimeFormat { get; set; }
    public char Separator { get; set; }
    public int Modo { get; set; }
    public int Hoja { get; set; }

    public AsyncProcessorDetail()
    {
    }
    //Proceso asíncrono para la impoprtación de datos desde fichero CSV
    public AsyncProcessorDetail(int id, string full_file_path, string filename, int total, string processor_type, int refresh_status_count, string info, string attributes, int indicatorid, char separator, int modo, string dateFormat)
    {
        Process = true;
        Processing = false;
        Stop = false;
        Complete = false;

        RefreshStatusCount = refresh_status_count;
        ID = id;
        FullFilePath = full_file_path;
        FileName = filename;
        Total = total;
        ProcessorType = processor_type;

        IndicatorId = indicatorid;
        DatetimeFormat = dateFormat;
        Atributtes = attributes;
        Separator = separator;
        Info = info;
        
        Modo = modo;
    }
    //Proceso asíncrono para la impoprtación de datos desde fichero EXCEL
    public AsyncProcessorDetail(int id, string full_file_path, string filename, int total, string processor_type, int refresh_status_count, string info, string attributes, int indicatorid, int hoja, int modo, string dateFormat)
    {
        Process = true;
        Processing = false;
        Stop = false;
        Complete = false;

        RefreshStatusCount = refresh_status_count;
        ID = id;
        FullFilePath = full_file_path;
        FileName = filename;
        Total = total;
        ProcessorType = processor_type;

        IndicatorId = indicatorid;
        DatetimeFormat = dateFormat;
        Atributtes = attributes;
        Info = info;
        Hoja = hoja;
        Modo = modo;
    }
    //Proceso asíncrono para la impoprtación de datos desde fichero JSON y XML
    public AsyncProcessorDetail(int id, string full_file_path, string filename, int total, string processor_type, int refresh_status_count, string info, string attributes, int indicatorid, int modo, string dateFormat)
    {
        Process = true;
        Processing = false;
        Stop = false;
        Complete = false;

        RefreshStatusCount = refresh_status_count;
        ID = id;
        FullFilePath = full_file_path;
        FileName = filename;
        Total = total;
        ProcessorType = processor_type;

        IndicatorId = indicatorid;
        DatetimeFormat = dateFormat;
        Atributtes = attributes;
        Info = info;
        Modo = modo;
    }
    public AsyncProcessorDetail(int id, int total, string processor_type, int refresh_status_count, string columns, string rows, string dimension, int indicatorid, int modo)
    {
        Process = true;
        Processing = false;
        Stop = false;
        Complete = false;

        RefreshStatusCount = refresh_status_count;
        ID = id;
        FullFilePath = string.Empty;
        FileName = string.Empty;
        Total = total;
        ProcessorType = processor_type;

        IndicatorId = indicatorid;
        Dimension = dimension;
        Info = columns;
        Filas = rows;
        Modo = modo;
    }

    public void Begin()
    {
        StartTime = DateTime.Now;
        Process = false;
        Processing = true;
    }

    public void UpdateCounts(int processed, int error_count, int duplicates, int total)
    {
        Processed = processed;
        ErrorCount = error_count;
        Duplicates = duplicates;
        Total = total;
    }

    public void UpdateStatusText(string status_text)
    {
        if (StatusText == null)
            StatusText = "";

        if (StatusText.Length > 0)
            StatusText += "<br />" + DateTime.Now.ToLongTimeString() + ": " + status_text;
        else
            StatusText = DateTime.Now.ToLongTimeString() + ": " + status_text;
    }

    public void End()
    {
        Stop = true;
        Processing = false;
        Complete = true;
        EndTime = DateTime.Now;

        TimeSpan.FromTicks(EndTime.Ticks - StartTime.Ticks).TotalSeconds.ToString();

        UpdateStatusText("Processing completed in "
            + TimeSpan.FromTicks(EndTime.Ticks - StartTime.Ticks).TotalSeconds.ToString()
            + " seconds");
    }
}