namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record AdicionarCebollaPicada(Guid IdHuevoPerico);

public class AdicionarCebollaPicadaHandler(IEventStore eventStore) : CommandHandler<AdicionarCebollaPicada>(eventStore)
{
    public override void Handle(AdicionarCebollaPicada comando)
    {
        var stream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        var huevoPerico = stream.CargarEntidad();
        
        if(huevoPerico.FogonEncendido 
           && huevoPerico.CantidadIngredientes(Ingredientes.Aceite) > 0
           && huevoPerico.CantidadIngredientes(Ingredientes.Cebolla) > 0)
            stream.Agregar(new CebollaPicadaAdicionada());
        
        if(huevoPerico.CantidadIngredientes(Ingredientes.Cebolla) == 0)
            stream.Agregar(new AdicionCebollaPicadaFallida(AdicionCebollaPicadaFallida.RazonError.NoSeHaPicadoLaCebolla));
        
        if (huevoPerico.CantidadIngredientes(Ingredientes.Aceite) == 0)
            stream.Agregar(new AdicionCebollaPicadaFallida(AdicionCebollaPicadaFallida.RazonError.SartenNoHaSidoEngrasado));
        
        if(huevoPerico.FogonEncendido == false)
            stream.Agregar(new AdicionCebollaPicadaFallida(AdicionCebollaPicadaFallida.RazonError.FogonNoHaSidoEncendido));
    }
}

public record CebollaPicadaAdicionada();

public record AdicionCebollaPicadaFallida(AdicionCebollaPicadaFallida.RazonError razonError)
{
    public enum RazonError
    {
        NoSeHaPicadoLaCebolla,
        SartenNoHaSidoEngrasado,
        FogonNoHaSidoEncendido
    }
  
}