namespace Kitchen.Dominio.HuevosPericos;

public class HuevoPerico: AggregateRoot
{
    public void Apply(CocinarHuevoPerico evento)
    {
        IdOrden = evento.IdOrden;
    }
    
    public void Apply(BatirHuevos evento)
    {
        CantidadHuevos = evento.CantidadHuevos;
    }

    public Guid IdOrden { get; private set; }
    public int CantidadHuevos { get; private set; }
}

public record HuevosBatidos(Guid IdHuevoPerico, int CantidadHuevosBatidos);
