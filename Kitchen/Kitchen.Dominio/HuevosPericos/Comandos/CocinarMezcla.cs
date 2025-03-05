namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record CocinarMezcla(Guid IdHuevoPerico);

public class CocinarMezclaHandler(IEventStore eventStore) : CommandHandler<CocinarMezcla>(eventStore)
{
    public override void Handle(CocinarMezcla comando)
    {
        var stream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        var huevoPerico = stream.CargarEntidad();

        if (huevoPerico.Estado == EstadoHuevoPerico.CocinandoHuevos)
            stream.Agregar(new HuevoPericoCocinado());
    }
}

public record HuevoPericoCocinado;