using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.Comandos;

public class PicarCebollaTest : CommandHandlerTest<PicarCebolla>
{
    protected override CommandHandler<PicarCebolla> Handler=> new PicarCebollaHandler(EventStore);

    [Fact]
    public void CebollaPicada()
    {
        Given(new HuevosPericosIniciados(IdAgregado));
        When(new PicarCebolla(IdAgregado, 1));
        Then(new CebollaPicada(IdAgregado, 1));
    }
}

