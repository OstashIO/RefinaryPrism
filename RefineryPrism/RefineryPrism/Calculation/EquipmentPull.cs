using System.Collections.Generic;
using System.Linq;
using RefineryPrism.Models;

namespace RefineryPrism.Calculation
{
    public class EquipmentPull
    {
        private readonly List<Equipment> _equipments;
        private readonly List<WorkTime> _workTimes;

        public EquipmentPull(IEnumerable<Equipment> equipments, IEnumerable<WorkTime> workTimes)
        {
            _equipments = new List<Equipment>(equipments);
            _workTimes = new List<WorkTime>(workTimes);

            foreach (var equipment in _equipments)
            {
                var nomenclatureIds = _workTimes.Where(wt => wt.EquipmentId == equipment.Id);

                equipment.PossibleWorks = new List<WorkTime>(nomenclatureIds);
            }
        }

        public List<Equipment> GetFreeMachines()
        {
            return _equipments.Where(e => e.Free).ToList();
        }
    }
}