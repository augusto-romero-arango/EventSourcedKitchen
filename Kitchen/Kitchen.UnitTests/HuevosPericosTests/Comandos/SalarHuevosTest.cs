using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class SalarHuevosTest : CommandHandlerTest<SalarHuevos>
{
    protected override CommandHandler<SalarHuevos> Handler => new SalarHuevosHandler(EventStore);

    [Fact]
    public void SalarHuevos_CuandoHuevosHanSidoBatidos_EmiteHuevosSalados()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()),
            new HuevosBatidos(2));
        When(new SalarHuevos(IdAgregado));
        Then(new HuevosSalados());

        var huevoPerico = ObtenerEntidad<HuevoPerico>(IdAgregado);
        huevoPerico.CantidadIngredientes(Ingredientes.Sal).Should().Be(1);
    }

    [Fact]
    public void SalarHuevos_CuandoNoHaBatidoHuevos_Falla()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()));
        When(new SalarHuevos(IdAgregado));
        Then(new HuevoSaladoFallido(HuevoSaladoFallido.RazonError.NoSeHanBatidoLosHuevos));
        
        var huevoPerico = ObtenerEntidad<HuevoPerico>(IdAgregado);
        huevoPerico.CantidadIngredientes(Ingredientes.Sal).Should().Be(0);
    }
    
    
}





