namespace Kitchen.Dominio.HuevosPericos.Comandos;

public record AdicionarHuevosSalados(Guid IdHuevoPerico);
public class AdicionarHuevosSaladosHandler(IEventStore eventStore) : CommandHandler<AdicionarHuevosSalados>(eventStore)
{
    public override void Handle(AdicionarHuevosSalados comando)
    {
        var stream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        var huevoPerico = stream.CargarEntidad();
        
        if (huevoPerico.Estado == EstadoHuevoPerico.SofriendoCebollaConTomate && huevoPerico.CantidadIngredientes(Ingredientes.Sal)== 1)
            stream.Agregar(new HuevosSaladosAdicionados());
        else 
        {
            if (huevoPerico.Estado != EstadoHuevoPerico.SofriendoCebollaConTomate)
                stream.Agregar(new AdicionHuevosSaladosFallido(AdicionHuevosSaladosFallido.RazonError.TomateNoHaSidoAdicionado));
            if(huevoPerico.CantidadIngredientes(Ingredientes.Sal)== 0)
                stream.Agregar(new AdicionHuevosSaladosFallido(AdicionHuevosSaladosFallido.RazonError.LosHuevosNoHanSidoSalados));
        }
        
    }
}

public record HuevosSaladosAdicionados;

public record AdicionHuevosSaladosFallido(AdicionHuevosSaladosFallido.RazonError Razon)
{
    public enum RazonError
    {
        TomateNoHaSidoAdicionado,
        LosHuevosNoHanSidoSalados
    }
}