namespace PromFutureTestWork.Models;

public class TerminalViewModel
{
    public int? TerminalId { get; set; }
    public List<Table> Rows { get; set; }
    public TerminalSettings Settings { get; set; }
}
