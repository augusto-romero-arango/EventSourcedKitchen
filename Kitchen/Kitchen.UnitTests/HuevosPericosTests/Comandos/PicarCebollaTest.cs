using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class PicarCebollaTest : CommandHandlerTest<PicarCebolla>
{
    protected override CommandHandler<PicarCebolla> Handler=> new PicarCebollaHandler(EventStore);

    [Fact]
    public void CebollaPicada()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()));
        When(new PicarCebolla(IdAgregado, 1));
        Then(new CebollaPicada(1));

        var huevoPerico = ObtenerEntidad<HuevoPerico>();
        Assert.Equal(1, huevoPerico.CantidadIngredientes(Ingredientes.Cebolla));
    }
}

