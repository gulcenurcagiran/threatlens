public class SecurityLog
{
    public string IpAddress { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; } 
    public string EventType { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}