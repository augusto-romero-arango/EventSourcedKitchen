namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record SalarHuevos(Guid IdHuevoPerico);

public class SalarHuevosHandler(IEventStore eventStore) : CommandHandler<SalarHuevos>(eventStore)
{
    public override void Handle(SalarHuevos comando)
    {
        var stream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        var huevoPerico = stream.CargarEntidad();
        
        if(huevoPerico.CantidadIngredientes(Ingredientes.Huevos)>0)
            stream.Agregar(new HuevosSalados());
        else
        {
            stream.Agregar(new HuevoSaladoFallido(HuevoSaladoFallido.RazonError.NoSeHanBatidoLosHuevos));
        }
        
        
    }
}

public record HuevosSalados;

public record HuevoSaladoFallido(HuevoSaladoFallido.RazonError razon)
{
    public enum RazonError
    {
        NoSeHanBatidoLosHuevos
    }
}