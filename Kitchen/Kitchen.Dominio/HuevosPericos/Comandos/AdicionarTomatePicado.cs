namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record AdicionarTomatePicado(Guid IdHuevoPerico);

public class AdicionarTomatePidadoHandler(IEventStore eventStore) : CommandHandler<AdicionarTomatePicado>(eventStore)
{
    public override void Handle(AdicionarTomatePicado comando)
    {
        var stream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        var huevoPerico = stream.CargarEntidad();

        if (huevoPerico.Estado == EstadoHuevoPerico.SofriendoCebolla)
            stream.Agregar(new TomatePicadoAdicionado());
        else
            stream.Agregar(new AdicionTomatePicadoFallido(AdicionTomatePicadoFallido.RazonError.NoSeHaAdicionadoCebolla));
    }
}

public record TomatePicadoAdicionado;

public record AdicionTomatePicadoFallido(AdicionTomatePicadoFallido.RazonError NoSeHaAdicionadoCebolla)
{
    public enum RazonError
    {
        NoSeHaAdicionadoCebolla
    }
}