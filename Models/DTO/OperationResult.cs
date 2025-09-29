public class OperationResult
{
    public bool Success { get; set; }
    public int AffectedRows { get; set; } = 0;
    public string Message { get; set; } = string.Empty;
}