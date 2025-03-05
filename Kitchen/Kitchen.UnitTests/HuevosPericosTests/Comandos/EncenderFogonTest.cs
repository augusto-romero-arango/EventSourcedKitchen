using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class EncenderFogonTest : CommandHandlerTest<EncenderFogon>
{
    protected override CommandHandler<EncenderFogon> Handler => new EncenderFogonHandler(EventStore);

    [Fact]
    public void EncenderFogon_DebeMarcarFogonEncendido()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()));
        When(new EncenderFogon(IdAgregado));
        Then(new FogonEncendido());

        var huevoPerico = ObtenerEntidad<HuevoPerico>();
        huevoPerico.FogonEncendido.Should().BeTrue();
    }
}

