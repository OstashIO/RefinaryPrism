using System.Collections.Generic;
using System.Linq;
using RefineryPrism.Models;

namespace RefineryPrism.Calculation
{
    public class Scheduler
    {
        private EquipmentPull _equipmentPull;

        public Scheduler(IEnumerable<Equipment> equipments, IEnumerable<WorkTime> workTimes)
        {
            _equipmentPull = new EquipmentPull(equipments, workTimes);
        }

        public List<WorkPart> Create(IEnumerable<Part> parts)
        {
            var workParts = new List<WorkPart>();

            var timer = 0;

            // Крутим цикл, пока все партии не обработаются
            while (parts.Count() != workParts.Count || workParts.Count == 0 || workParts.Count(wp => wp.IsProcessed) != 0)
            {
                foreach (var workPart in workParts.Where(wp => wp.IsProcessed))
                {
                    if (workPart.EndTime <= timer)
                    {
                        workPart.IsProcessed = false;
                        workPart.Equipment.Free = true;

                        var part = parts.Single(p => p.Id == workPart.Part.Id);
                        part.InProgress = false;
                        part.Processed = true;
                    }
                }

                var freeMachines = _equipmentPull.GetFreeMachines();

                if (freeMachines.Count != 0)
                {
                    foreach (var freeMachine in freeMachines)
                    {
                        var nextParts = parts.Where(part => !part.Processed && !part.InProgress).ToList();

                        int k = 0;
                        while (freeMachine.Free && nextParts.Count > k)
                        {
                            if (freeMachine.CanHandleNomenclature(nextParts[k].NomenclatureId))
                            {
                                freeMachine.Free = false;

                                workParts.Add(new WorkPart
                                {
                                    Equipment = freeMachine,
                                    Part = nextParts[k],
                                    StartTime = timer,
                                    EndTime = timer + freeMachine.GetWorkTime(nextParts[k].NomenclatureId),
                                    IsProcessed = true
                                });

                                nextParts[k].InProgress = true;
                                nextParts[k].Processed = false;
                            }

                            k++;
                        }
                    }
                }

                // Так как все операции занимают числа кратные 10, то сделал шаг итераии для таймера 10
                timer += 10;
            }

            return workParts;
        }
    }
}
