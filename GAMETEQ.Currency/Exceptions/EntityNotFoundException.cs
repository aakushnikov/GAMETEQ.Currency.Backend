namespace GAMETEQ.Currency.Exceptions;

public sealed class EntityNotFoundException<T> : Exception
{
    public EntityNotFoundException()
        : base($"{typeof(T).Name} not found")
    {
    }
}