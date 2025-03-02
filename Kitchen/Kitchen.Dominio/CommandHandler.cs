namespace Kitchen.Dominio;

public abstract class CommandHandler<TCommand>(IEventStore eventStore)
{
    protected EventStream<TEntidad> ObtenerStream<TEntidad>(Guid aggregateId)
        where TEntidad : AggregateRoot, new()
    {
        return new EventStream<TEntidad>(eventStore, aggregateId);
    }
    /// <summary>
    /// Ejecuta la lógica de negocio correspondiente al comando
    /// </summary>
    /// <param name="comando"></param>
    public abstract void Handle(TCommand comando);
}