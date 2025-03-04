namespace Kitchen.Dominio;

public class EventStream<TEntidad>(IEventStore eventStore, Guid aggregateId)
    where TEntidad : AggregateRoot, new()
{
    private int _ultimoNumeroDeSecuencia;
    /// <summary>
    /// Obtiene la entidad en el estado actual, aplicándole todos los eventos registrados hasta el momento
    /// </summary>
    /// <returns></returns>
    public TEntidad CargarEntidad()
    {
        var eventos = eventStore.ObtenerEventos(aggregateId);
        TEntidad entidad = new();

        // Usamos el patrón dynamic para llamar a Apply con el tipo correcto.
        // De esta manera no tomará el constructor vacío de AggregateRoot
        foreach (var evento in eventos)
        {
            entidad.Apply((dynamic)evento.EventData);
            _ultimoNumeroDeSecuencia = evento.SequenceNumber;
        }
        
        return entidad;
    }

    /// <summary>
    /// Adiciona un evento a la secuencia de eventos de la entidad
    /// </summary>
    /// <param name="evento"></param>
    public void Agregar(object evento)
    {
        _ultimoNumeroDeSecuencia++;
        
        StoredEvent storedEvent = new(aggregateId, _ultimoNumeroDeSecuencia, DateTime.UtcNow, evento);
        
        eventStore.AgregarEvento(storedEvent);
        
    }
    
}