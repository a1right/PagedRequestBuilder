using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventStuff.Models
{
    public class Event
    {
        public long Id { get; set; }
        public EventType Type { get; set; }
        public DateTime DateTime { get; set; }
        [Column(TypeName = "jsonb")]
        public List<EventExtendedProperty> Properties { get; set; } = new();

        public static Event New(EventType type) => new Event()
        {
            Type = type,
            DateTime = DateTime.UtcNow
        };
    }

    public enum EventType
    {
        Login,
        SignUp,
        InvitedSignUp,
        RoleChanged,
    }

    public class EventExtendedProperty
    {
        public string Key { get; set; }
        public dynamic Value { get; set; }
    }

    public class EventFilter
    {
        public EventType? Type { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public List<EventExtendedProperty>? Properties { get; set; }
    }
}
