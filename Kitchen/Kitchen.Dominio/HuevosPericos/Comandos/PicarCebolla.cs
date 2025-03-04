namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record PicarCebolla(Guid IdHuevoPerico, int CantidadCebolla);
public class PicarCebollaHandler(IEventStore eventStore) : CommandHandler<PicarCebolla>(eventStore)
{
    public override void Handle(PicarCebolla comando)
    {
        var huevoPericoStream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        _ = huevoPericoStream.CargarEntidad();
        huevoPericoStream.Agregar(new CebollaPicada(comando.IdHuevoPerico,comando.CantidadCebolla));
    }
}


