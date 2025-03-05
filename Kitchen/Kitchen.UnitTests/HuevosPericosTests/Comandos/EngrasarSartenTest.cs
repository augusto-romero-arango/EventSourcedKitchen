using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class EngrasarSartenTest : CommandHandlerTest<EngrasarSarten>
{
    protected override CommandHandler<EngrasarSarten> Handler => new EngrasarSartenCommandHandler(EventStore);

    [Fact]
    public void EngrasarSarten_AdicionaAceite()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()));
        When(new EngrasarSarten(IdAgregado));
        Then(new SartenEngrasado());

        var huevoPerico = ObtenerEntidad<HuevoPerico>();
        huevoPerico.CantidadIngredientes(Ingredientes.Aceite).Should().Be(1);
    }
}


