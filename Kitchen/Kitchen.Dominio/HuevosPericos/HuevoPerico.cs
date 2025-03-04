using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.Dominio.HuevosPericos;

public class HuevoPerico: AggregateRoot
{
 
    public void Apply(HuevosPericosIniciados evento)
    {
        Id = evento.IdHuevoPerico;
        IdOrden = evento.IdOrden;
    }
    
    public void Apply(HuevosBatidos evento)
    {
        _ingredientes.Add(Ingredientes.Huevos, evento.CantidadHuevosBatidos);
    }

    public Guid Id { get; private set; }

    public Guid IdOrden { get; private set; }
    private Dictionary<Ingredientes, int> _ingredientes = new();
    public int CantidadIngredientes(Ingredientes ingrediente) => _ingredientes[ingrediente];
}

public record HuevosPericosIniciados(Guid IdHuevoPerico, Guid IdOrden);
public record HuevosBatidos(Guid IdHuevoPerico, int CantidadHuevosBatidos);
public record CebollaPicada(Guid IdHuevoPerico, int CantidadCebolla);
public enum Ingredientes
{
    Huevos,
    Cebolla,
    Tomate,
    Sal
}
