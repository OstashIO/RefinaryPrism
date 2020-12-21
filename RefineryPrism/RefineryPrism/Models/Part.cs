namespace RefineryPrism.Models
{
    public class Part
    {
        public int Id { get; set; }
        public int NomenclatureId { get; set; }
        public bool InProgress { get; set; } = false;
        public bool Processed { get; set; } = false;
    }
}
