namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record PicarCebolla(Guid IdHuevoPerico, int CantidadCebolla);
public class PicarCebollaHandler(IEventStore eventStore) : CommandHandler<PicarCebolla>(eventStore)
{
    public override void Handle(PicarCebolla comando)
    {
        var stream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        _ = stream.CargarEntidad();
        stream.Agregar(new CebollaPicada(comando.CantidadCebolla));
    }
}


