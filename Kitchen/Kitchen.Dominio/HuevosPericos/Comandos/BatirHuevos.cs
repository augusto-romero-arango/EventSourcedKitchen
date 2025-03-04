namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record BatirHuevos(Guid IdHuevoPerico, int CantidadHuevos);

public class BatirHuevosHandler(IEventStore eventStore) : CommandHandler<BatirHuevos>(eventStore)
{
    public override void Handle(BatirHuevos comando)
    {
        var huevoPericoStream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        _ = huevoPericoStream.CargarEntidad();
        
        huevoPericoStream.Agregar(new HuevosBatidos(comando.IdHuevoPerico, comando.CantidadHuevos));
        
        
    }
}
