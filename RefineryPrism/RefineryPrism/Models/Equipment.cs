using System;
using System.Collections.Generic;
using System.Linq;

namespace RefineryPrism.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Free { get; set; } = true;
        public List<WorkTime> PossibleWorks { get; set; }

        public bool CanHandleNomenclature(int nomenclatureId)
        {
            if (PossibleWorks == null)
            {
                return false;
            }

            if (PossibleWorks.Count == 0)
            {
                return false;
            }

            return PossibleWorks
                .Select(w => w.NomenclatureId)
                .Contains(nomenclatureId);
        }

        public int GetWorkTime(int nomenclatureId)
        {
            var result = PossibleWorks.SingleOrDefault(wt => wt.NomenclatureId == nomenclatureId);

            if (result == null)
            {
                throw new Exception("Ошибка при поиске в работах печей");
            }

            return result.Time;
        }
    }
}
