public class ClimbLog
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Grade { get; set; } = "";
    public DateOnly Date { get; set; }
    public bool Sent { get; set; }
    public int Attempts { get; set; }
    public string? Note { get; set; }
}