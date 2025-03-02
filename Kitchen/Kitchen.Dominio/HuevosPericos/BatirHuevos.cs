namespace Kitchen.Dominio.HuevosPericos;

public record BatirHuevos(Guid IdHuevoPerico, int CantidadHuevos);

public class BatirHuevosHandler(IEventStore eventStore) : CommandHandler<BatirHuevos>(eventStore)
{
    public override void Handle(BatirHuevos comando)
    {
        var huevoPericoStream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        var huevoPerico = huevoPericoStream.ObtenerEntidad();
        
        huevoPericoStream.Agregar(new HuevosBatidos(comando.IdHuevoPerico, comando.CantidadHuevos));
        
        
        eventStore.GuardarCambios();
    }
}
