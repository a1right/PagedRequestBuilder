using EventStuff.Models;
using System;

namespace EventStuff
{
    public static class EventLogBuilder
    {
        public static Event CreateLoginEvent(Guid id)
        {
            var item = Event.New(EventType.Login);

            item.Properties.Add(EventConsts.Properties.GetAuthorProperty(id));

            return item;
        }

        public static Event CreateRoleChangedEvent(Guid id, int roleId)
        {
            var item = Event.New(EventType.RoleChanged);
            item.Properties.Add(EventConsts.Properties.GetNewRoleProperty(roleId));
            item.Properties.Add(EventConsts.Properties.GetAuthorProperty(id));

            return item;
        }
    }

    public static class EventConsts
    {
        public static class Properties
        {
            public static EventExtendedProperty GetAuthorProperty(Guid value) => GetEmptyExtendedProperty(PropertyKeys.Author, value);
            public static EventExtendedProperty GetNewRoleProperty(int value) => GetEmptyExtendedProperty(PropertyKeys.NewRole, value);

            private static EventExtendedProperty GetEmptyExtendedProperty<T>(string key, T value) => new EventExtendedProperty() { Key = key, Value = value };
        }

        public static class PropertyKeys
        {
            public const string Author = "Author";
            public const string NewRole = "NewRole";
        }
    }
}
