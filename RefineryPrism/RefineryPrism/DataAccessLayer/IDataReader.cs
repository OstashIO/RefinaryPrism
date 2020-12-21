using System.Collections.Generic;
using RefineryPrism.Models;

namespace RefineryPrism.DataAccessLayer
{
    public interface IDataReader
    {
        IEnumerable<Equipment> ReadEquipments(string path);
        IEnumerable<Nomenclature> ReadNomenclatures(string path);
        IEnumerable<Part> ReadParts(string path);
        IEnumerable<WorkTime> ReadWorkTimes(string path);
    }
}