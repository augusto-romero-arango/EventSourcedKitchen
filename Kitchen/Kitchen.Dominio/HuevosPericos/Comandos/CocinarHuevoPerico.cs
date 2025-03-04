namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record CocinarHuevoPerico(Guid IdHuevoPerico, Guid IdOrden);

public class CocinarHuevoPericoHandler(IEventStore eventStore) : CommandHandler<CocinarHuevoPerico>(eventStore)
{
    public override void Handle(CocinarHuevoPerico comando)
    {
        var huevoPericoStream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        _ = huevoPericoStream.CargarEntidad();
        
        huevoPericoStream.Agregar(new HuevosPericosIniciados(comando.IdHuevoPerico, comando.IdOrden));
    }
}