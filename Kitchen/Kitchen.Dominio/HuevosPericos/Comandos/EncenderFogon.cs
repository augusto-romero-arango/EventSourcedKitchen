namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record EncenderFogon(Guid IdHuevoPerico);

public class EncenderFogonHandler(IEventStore eventStore) : CommandHandler<EncenderFogon>(eventStore)
{
    public override void Handle(EncenderFogon comando)
    {
        var stream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        
        stream.Agregar(new FogonEncendido());
    }
}

public record FogonEncendido();