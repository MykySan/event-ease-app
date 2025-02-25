namespace EventEaseApp.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Location { get; set; } = string.Empty;
    }
}
