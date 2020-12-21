using System.Collections.Generic;
using RefineryPrism.Models;

namespace RefineryPrism.DataAccessLayer
{
    public interface IDataWriter
    {
        void WriteReport(string path, IEnumerable<WorkPart> parties);
    }
}