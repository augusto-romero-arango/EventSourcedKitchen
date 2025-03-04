using Kitchen.Dominio;

namespace Kitchen.UnitTests;

public class TestEventStore : IEventStore
{
    public List<StoredEvent> AnterioresEventos = new();
    public List<StoredEvent> NuevosEventos = new();

    public IEnumerable<StoredEvent> ObtenerEventos(Guid aggregateId)
    {
        var eventos = AnterioresEventos.Concat(NuevosEventos);
        return eventos
            .Where(e => e.AggregateId == aggregateId)
            .ToList();
    }

    public void AgregarEvento(StoredEvent evento)
    {
        NuevosEventos.Add(evento);
    }

    public void GuardarCambios()
    {
        throw new NotImplementedException();
    }
}