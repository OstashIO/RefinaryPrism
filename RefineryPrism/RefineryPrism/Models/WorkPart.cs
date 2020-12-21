namespace RefineryPrism.Models
{
    public class WorkPart
    {
        public Part Part { get; set; }

        public Equipment Equipment { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public bool IsProcessed { get; set; } = true;
    }
}
