using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.Dominio.HuevosPericos;

public class HuevoPerico: AggregateRoot
{
    private readonly Dictionary<Ingredientes, int> _ingredientesUtilizados = new();
    public void Apply(HuevosPericosIniciados evento)
    {
        Id = evento.IdHuevoPerico;
        IdOrden = evento.IdOrden;
        Estado = EstadoHuevoPerico.Iniciado;
    }
    
    public void Apply(HuevosBatidos evento)
    {
        _ingredientesUtilizados.Add(Ingredientes.Huevos, evento.CantidadHuevosBatidos);
    }
    public void Apply(CebollaPicada evento)
    {
        _ingredientesUtilizados.Add(Ingredientes.Cebolla, evento.CantidadCebolla);
    }
    public void Apply(TomatePicado evento)
    {
        _ingredientesUtilizados.Add(Ingredientes.Tomate, evento.CantidadTomates);
    }

    public void Apply(SartenEngrasado evento)
    {
        _ingredientesUtilizados.Add(Ingredientes.Aceite, 1);
    }

    public void Apply(FogonEncendido evento)
    {
        FogonEncendido = true;
    }

    public void Apply(CebollaPicadaAdicionada evento)
    {
        Estado = EstadoHuevoPerico.SofriendoCebolla;
    }
    
    public void Apply(HuevosSalados evento)
    {
        _ingredientesUtilizados.Add(Ingredientes.Sal, 1);
    }
    
    public void Apply(TomatePicadoAdicionado evento)
    {
        Estado = EstadoHuevoPerico.SofriendoCebollaConTomate;
    }
    
    public void Apply(HuevosSaladosAdicionados evento)
    {
        Estado = EstadoHuevoPerico.CocinandoHuevos;
    }
    
    public void Apply(HuevoPericoCocinado evento)
    {
        Estado = EstadoHuevoPerico.Finalizado;
    }

    public Guid Id { get; private set; }
    public Guid IdOrden { get; private set; }
    public bool FogonEncendido { get; private set; }
    public EstadoHuevoPerico Estado { get; set; }

    public int CantidadIngredientes(Ingredientes ingrediente) => _ingredientesUtilizados.GetValueOrDefault(ingrediente, 0);
}

public enum EstadoHuevoPerico
{
    Iniciado,
    SofriendoCebolla,
    SofriendoCebollaConTomate,
    CocinandoHuevos,
    Finalizado
}

public record HuevosPericosIniciados(Guid IdHuevoPerico, Guid IdOrden);
public record HuevosBatidos(int CantidadHuevosBatidos);
public record CebollaPicada(int CantidadCebolla);
public record TomatePicado(int CantidadTomates);
public enum Ingredientes
{
    Huevos,
    Cebolla,
    Tomate,
    Sal,
    Aceite
}
