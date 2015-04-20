namespace Contra.Tests.Support
{
    public interface Envelope
    {
        object UntypedMessage { get; }
    }

    public interface Envelope<out T> : Envelope
    {
        T Message { get; }
    }

    public class ConcreteEnvelope<T> : Envelope
    {
        public T Message { get; private set; }

        public object UntypedMessage { get; set; }

        public ConcreteEnvelope(T message)
        {
            Message = message;
        }
    }
}