// Copyright © 2019 Mavidian Technologies Limited Liability Company. All Rights Reserved.

using Mavidian.DataConveyer.Common;
using Mavidian.DataConveyer.Entities.KeyVal;
using Mavidian.DataConveyer.Orchestrators;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataConveyer_ConvertCsvToFlat
{
   /// <summary>
   /// Represents Data Conveyer functionality specific to translating CSV airplane records
   /// into a flat (fixed-width filed) format.
   /// </summary>
   internal class FileProcessor
   {
      private readonly IOrchestrator Orchestrator;

      internal FileProcessor(string inFile, string outLocation)
      {
         var config = new OrchestratorConfig()
         {
            InputDataKind = KindOfTextData.Delimited,
            InputFields = "PlaneDescription,IataCode,IcaoCode",
            InputFileName = inFile,
            TransformerType = TransformerType.Universal,
            UniversalTransformer = FilterAndReorganizeFields,
            AllowTransformToAlterFields = true,
            OutputDataKind = KindOfTextData.Flat,
            OutputFields = "IataCode|4,Hyphen|2,PlaneDescription|70",
            ExcludeExtraneousFields = true,
            OutputFileName = outLocation + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(inFile) + ".txt"
         };

         Orchestrator = OrchestratorCreator.GetEtlOrchestrator(config);
      }

      /// <summary>
      /// Execute Data Conveyer process.
      /// </summary>
      /// <returns>Task containing the process results.</returns>
      internal async Task<ProcessResult> ProcessFileAsync()
      {
         var result = await Orchestrator.ExecuteAsync();
         Orchestrator.Dispose();

         return result;
      }

      /// <summary>
      /// Universal transformer to translate an input cluster into a set of (0 or 1) output clusters.
      /// </summary>
      /// <param name="inClstr">Cluster received from intake.</param>
      /// <returns>A single resulting cluster to be sent to output, or empty if cluster needs to be filtered out. </returns>
      private IEnumerable<ICluster> FilterAndReorganizeFields(ICluster inClstr)
      {
         //Note: Universal transformer is needed because we need to perform 2 distinct transformations:
         //      (1) Filtering (records with \N in IataCode field need to be removed)
         //      (2) Record layout update (IataCode moved to first position, IcaoCode removed, etc.)
         //      If only (1) was needed, then RecordFilter transformer would suffice.
         //      If only (2) was needed, then Recordbound transformer would suffice.
         var inRec = inClstr[0];
         var iataCode = (string)inRec["IataCode"];
         if (iataCode == "\\N") return Enumerable.Empty<ICluster>();

         var outClstr = inClstr.GetEmptyClone();
         dynamic outRec = inClstr[0];
         outRec.IataCode = iataCode;
         outRec.Hyphen = "-";
         outRec.PlaneDescription = inRec["PlaneDescription"];
         outClstr.AddRecord(outRec);
         return Enumerable.Repeat(outClstr, 1);
      }

   }
}
