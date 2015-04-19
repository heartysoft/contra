namespace Contra.Tests.Support
{
    public class PersonRegisteredEvent : RegisteredEvent
    {
        public string Id { get; private set; }

        public PersonRegisteredEvent(string id)
        {
            Id = id;
        }
    }
}