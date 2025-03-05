namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record CocinarHuevoPerico(Guid IdHuevoPerico, Guid IdOrden);

public class CocinarHuevoPericoHandler(IEventStore eventStore) : CommandHandler<CocinarHuevoPerico>(eventStore)
{
    public override void Handle(CocinarHuevoPerico comando)
    {
        var stream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        
        stream.Agregar(new HuevosPericosIniciados(comando.IdHuevoPerico, comando.IdOrden));
    }
}