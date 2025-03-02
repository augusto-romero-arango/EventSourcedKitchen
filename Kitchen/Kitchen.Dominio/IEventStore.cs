namespace Kitchen.Dominio;

public interface IEventStore
{
    IEnumerable<StoredEvent> ObtenerEventos(Guid aggregateId);
    void AgregarEvento(StoredEvent evento);
    void GuardarCambios();
}