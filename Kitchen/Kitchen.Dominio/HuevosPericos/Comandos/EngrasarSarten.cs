namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record EngrasarSarten(Guid IdHuevoPerico);

public class EngrasarSartenCommandHandler(IEventStore eventStore) : CommandHandler<EngrasarSarten>(eventStore)
{
    public override void Handle(EngrasarSarten comando)
    {
        var stream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        
        stream.Agregar(new SartenEngrasado());
    }
}

public record SartenEngrasado();