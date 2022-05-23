namespace PromFutureTestWork.Models
{
    public sealed class Table
    {
        public DateTime Date { get; set; }
        public string Command { get; set; }
        public int? Parameter1 { get; set; }
        public int? Parameter2 { get; set; }
        public int? Parameter3 { get; set; }
        public string? Status { get; set; }
    }
}
