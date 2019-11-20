using System;
using Dapper.Contrib.Extensions;

[Table("Event")]
class Event
{
    [Key]
    public int Id { get; set; }
    public int EventLocationId { get; set; }
    public string EventName { get; set; }
    public DateTime EventDate { get; set; }
    public DateTime DateCreated { get; set; }
}