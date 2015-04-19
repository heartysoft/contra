namespace Contra.Tests.Support
{
    public class CriminalRegisteredEvent : PersonRegisteredEvent
    {
        public CriminalRegisteredEvent(string id) : base(id)
        {
        }
    }
}