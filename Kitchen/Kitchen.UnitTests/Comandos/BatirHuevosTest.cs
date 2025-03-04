using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.Comandos;

public class BatirHuevosTest : CommandHandlerTest<BatirHuevos>
{
    protected override CommandHandler<BatirHuevos> Handler => new BatirHuevosHandler(EventStore);
    [Fact]
    public void HuevosFueronBatidos()
    {

        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()));
        When(new BatirHuevos(IdAgregado, 3));
        Then(new HuevosBatidos(IdAgregado, 3));
        
        var huevoPerico = ObtenerHuevoPerico();
        huevoPerico.CantidadIngredientes(Ingredientes.Huevos).Should().Be(3);
    }

    private HuevoPerico ObtenerHuevoPerico()
    {
        var stream = new EventStream<HuevoPerico>(EventStore, IdAgregado);
        return stream.CargarEntidad();
    }
    
}