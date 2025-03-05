using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class BatirHuevosTest : CommandHandlerTest<BatirHuevos>
{
    protected override CommandHandler<BatirHuevos> Handler => new BatirHuevosHandler(EventStore);
    [Fact]
    public void HuevosFueronBatidos()
    {

        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()));
        When(new BatirHuevos(IdAgregado, 3));
        Then(new HuevosBatidos(3));
        
        var huevoPerico = ObtenerEntidad<HuevoPerico>();
        huevoPerico.CantidadIngredientes(Ingredientes.Huevos).Should().Be(3);
    }
    
    [Fact]
    public void BatirHuevos_NoSePuedeSiNoSeHanIniciadoLosHuevos()
    {
        When(new BatirHuevos(IdAgregado, 3));
        Then(new BatirHuevosFallido(BatirHuevosFallido.RazonError.NoSeHanIniciadoLosHuevos));
    }

    [Fact]
    public void BatirHuevos_NoSePuedeRealizarMasDeUnaVez()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()));
        When(new BatirHuevos(IdAgregado, 3));
        When(new BatirHuevos(IdAgregado, 2));
        Then(new HuevosBatidos(3), 
            new BatirHuevosFallido(BatirHuevosFallido.RazonError.NoPuedeBatirHuevosMasDeUnaVez));
        
        var huevoPerico = ObtenerEntidad<HuevoPerico>();
        huevoPerico.CantidadIngredientes(Ingredientes.Huevos).Should().Be(3);
    }

    
    
}

