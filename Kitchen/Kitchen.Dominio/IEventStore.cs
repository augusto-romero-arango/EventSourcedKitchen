namespace Kitchen.Dominio;

public interface IEventStore
{
    IEnumerable<StoredEvent> ObtenerEventos(Guid idAgregado);
    void AgregarEvento(StoredEvent evento);
    void GuardarCambios();
}