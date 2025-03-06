using Kitchen.Dominio;

namespace Kitchen.UnitTests;

public class TestEventStore : IEventStore
{
    private readonly List<StoredEvent> _eventosPasados = new();
    private readonly List<StoredEvent> _eventosGenerados = new();

    /// <summary>
    /// Registrar los eventos ordenados que han sido aplicados antes de la creación de la entidad.
    /// </summary>
    /// <param name="idAgregado">El id de la entidad.</param>
    /// <param name="eventos">Listado de eventos ordenados que serán aplicados a al entidad antes de ejecutar el SUT.</param>
    public void  AgregarEventosAnteriores(Guid idAgregado, params object[] eventos)
    {
        _eventosPasados
            .AddRange(
                eventos.Select((evento, i) => new StoredEvent(idAgregado, i, DateTime.UtcNow, evento))
            );
    }
    
    /// <summary>
    /// Obtiene el cuerpo de los eventos que fueron registrados en el store después de la ejecución del SUT.
    /// </summary>
    /// <param name="idAgregado">El id de la entidad.</param>
    /// <returns>Retorna los eventos registrados en el eventStore posteriores a la ejecución del SUT.</returns>
    public object[] ObtenerEventosGenerados(Guid idAgregado)
    {
        return _eventosGenerados
            .Where(e => e .AggregateId== idAgregado)
            .OrderBy(e => e.SequenceNumber)
            .Select(e => e.EventData)
            .ToArray();
    }

    /// <summary>
    /// Obtiene todos los eventos aplicados para una entidad.
    /// </summary>
    /// <param name="idAgregado">Id de la entidad cuyos eventos vayan a ser consultados</param>
    /// <returns></returns>
    public IEnumerable<StoredEvent> ObtenerEventos(Guid idAgregado)
    {
        var eventos = _eventosPasados.Concat(_eventosGenerados);
        return eventos
            .Where(e => e.AggregateId == idAgregado)
            .ToList();
    }

    /// <summary>
    /// Adicionar el evento a la lista de nuevos eventos.
    /// </summary>
    /// <param name="evento">Evento a aplicar a la entidad</param>
    public void AgregarEvento(StoredEvent evento)
    {
        _eventosGenerados.Add(evento);
    }

    /// <summary>
    /// No se implementa porque es una lista en memoria que no se persiste.
    /// </summary>
    public void GuardarCambios()
    {
    }
}