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
using System.Threading;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public static class AsyncProcessManager
{
    public static List<AsyncProcessorDetail> ProcDetails { get; set; }

    public static int StartAsyncProcess(AsyncProcessorDetail input_detail)
    {
        if (ProcDetails == null)
            ProcDetails = new List<AsyncProcessorDetail>();

        bool error = false;

        foreach (AsyncProcessorDetail detail in ProcDetails)
        {
            if (detail.ID == input_detail.ID)
            {
                // task is already being processed
                if (detail.Processing)
                {
                    error = true;
                }
            }
        }

        if (!error)
        {
            // removing any existing instances of this detail in the list
            ProcDetails.RemoveAll(z => z.ID == input_detail.ID);

            // Add the detail to the list
            ProcDetails.Add(input_detail);

            // Start processor thread
            Thread processorThread = new Thread(new ThreadStart(StartProcessorThread));
            processorThread.IsBackground = false;
            processorThread.Start();

            return 0;
        }
        else
            return 1;
    }

    public class AsyncProcessorThread
    {
        // Main processor thread
        public int ID { get; set; }

        public AsyncProcessorThread()
        {
            try
            {
                ID = GetNextIDToProcess();

                ProcDetails.SingleOrDefault(z => z.ID == ID).Begin();

                AsyncProcessorDetail fd = ProcDetails.SingleOrDefault(z => z.ID == ID);

                // Add a case statement for each of the processor classes you create
                switch (fd.ProcessorType)
                {
                    case "ProcessorImportCsv":
                        ProcessorImportCsv ImportDataCSV = new ProcessorImportCsv(fd);
                        break;
                    case "ProcessorImportXls":
                        ProcessorImportXls ImportDataXLS = new ProcessorImportXls(fd);
                        break;
                    case "ProcessorImportJson":
                        ProcessorImportJson ImportDataJSON = new ProcessorImportJson(fd);
                        break;
                    case "ProcessorImportXML":
                        ProcessorImportXml ImportDataXML = new ProcessorImportXml(fd);
                        break;
                    case "ProcessorImportTable":
                        ProcessorImportTable ImportDataTable = new ProcessorImportTable(fd);
                        break;
                    default:
                        ProcDetails.SingleOrDefault(z => z.ID == ID).StatusText = "No ProcessorType found. Exiting..";
                        ProcDetails.SingleOrDefault(z => z.ID == ID).End();
                        break;
                }
            }
            catch (Exception ex) { }
        }
    }

    private static void StartProcessorThread()
    {
        AsyncProcessorThread processor = new AsyncProcessorThread();
    }

    public static List<AsyncProcessorDetail> GetAllProcessDetails()
    {
        List<AsyncProcessorDetail> pds = new List<AsyncProcessorDetail>();
        foreach (AsyncProcessorDetail pd in ProcDetails)
        {
            pds.Add(pd);
        }
        return pds;
    }

    public static void RemoveCompletedProcesses()
    {
        // remove any complete processor details in the list
        ProcDetails.RemoveAll(z => z.Complete == true);
    }
    private static void UpdateCounts(int file_id, int processed, int error_count, int duplicates, int total)
    {
        ProcDetails.SingleOrDefault(z => z.ID == file_id).UpdateCounts(processed, error_count, duplicates, total);
    }

    private static int GetNextIDToProcess()
    {
        return ProcDetails.SingleOrDefault(z => z.Process == true).ID;
    }

    public static AsyncProcessorDetail GetProcessorDetail(int file_id)
    {
        try
        {
            return ProcDetails.SingleOrDefault(z => z.ID == file_id);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static bool Continue(int id)
    {
        return !ProcDetails.SingleOrDefault(z => z.ID == id).Stop;
    }

    public static void StopProcessor(int id)
    {
        ProcDetails.SingleOrDefault(z => z.ID == id).Stop = true;
        ProcDetails.SingleOrDefault(z => z.ID == id).Processing = false;
    }

    public static void FinalizeProcess(int id)
    {
        ProcDetails.SingleOrDefault(z => z.ID == id).End();
    }

}