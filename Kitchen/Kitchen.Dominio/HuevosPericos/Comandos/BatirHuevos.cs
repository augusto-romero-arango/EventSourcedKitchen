namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record BatirHuevos(Guid IdHuevoPerico, int CantidadHuevos);

public class BatirHuevosHandler(IEventStore eventStore) : CommandHandler<BatirHuevos>(eventStore)
{
    public override void Handle(BatirHuevos comando)
    {
        var huevoPericoStream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        var huevoPerico = huevoPericoStream.CargarEntidad();

        var error = RetornarErrorSiExiste(huevoPerico);
        if (error != null)
        {
            huevoPericoStream.Agregar(error);
            return;
        }
        
        huevoPericoStream.Agregar(new HuevosBatidos(comando.CantidadHuevos));    
    }

    private static BatirHuevosFallido? RetornarErrorSiExiste(HuevoPerico huevoPerico)
    {
        if (huevoPerico.IdOrden == Guid.Empty)
            return new BatirHuevosFallido(BatirHuevosFallido.RazonError.NoSeHanIniciadoLosHuevos);
        
        if (huevoPerico.CantidadIngredientes(Ingredientes.Huevos) > 0)
            return new BatirHuevosFallido(BatirHuevosFallido.RazonError.NoPuedeBatirHuevosMasDeUnaVez);

        return null;
    }
}

public record BatirHuevosFallido(BatirHuevosFallido.RazonError Razon)
{
    public enum RazonError
    {
        NoSeHanIniciadoLosHuevos,
        NoPuedeBatirHuevosMasDeUnaVez
    }
}
