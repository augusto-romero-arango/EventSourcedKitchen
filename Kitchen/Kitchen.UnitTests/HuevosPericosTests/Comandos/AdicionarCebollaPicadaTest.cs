using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class AdicionarCebollaPicadaTest : CommandHandlerTest<AdicionarCebollaPicada>
{
    protected override CommandHandler<AdicionarCebollaPicada> Handler => new AdicionarCebollaPicadaHandler(EventStore);

    [Fact]
    public void AdicionarCebollaPicada_Cuando_CebollaPicada_SartenEngrasado_y_FogonEncendido()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()),
            new FogonEncendido(),
            new SartenEngrasado(), 
            new CebollaPicada(1));
        When(new AdicionarCebollaPicada(IdAgregado));
        Then(new CebollaPicadaAdicionada());

        var huevoPerico = ObtenerEntidad<HuevoPerico>();
        huevoPerico.Estado.Should().Be(EstadoHuevoPerico.SofriendoCebolla);
    }

    [Fact]
    public void AdicionarCebollaPicada_Cuando_CebollaNoSeHaPicado_EmiteFallo()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()),
            new FogonEncendido(),
            new SartenEngrasado()
            );
        When(new AdicionarCebollaPicada(IdAgregado));
        Then(new AdicionCebollaPicadaFallida(AdicionCebollaPicadaFallida.RazonError.NoSeHaPicadoLaCebolla));
    }
    [Fact]
    public void AdicionarCebollaPicada_Cuando_SartenNoHaSidoEngrasado_EmiteFallo()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()),
            new FogonEncendido(),
            new CebollaPicada(1)
        );
        When(new AdicionarCebollaPicada(IdAgregado));
        Then(new AdicionCebollaPicadaFallida(AdicionCebollaPicadaFallida.RazonError.SartenNoHaSidoEngrasado));
    }
    
    [Fact]
    public void AdicionarCebollaPicada_Cuando_FogonNoHaSidoEncendido_EmiteFallo()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()),
            new SartenEngrasado(), 
            new CebollaPicada(1)
        );
        When(new AdicionarCebollaPicada(IdAgregado));
        Then(new AdicionCebollaPicadaFallida(AdicionCebollaPicadaFallida.RazonError.FogonNoHaSidoEncendido));
    }
    [Fact]
    public void AdicionarCebollaPicada_Cuando_NoSeHaCumplidoNingunPrerrequisito_EmiteTodosLosFallos()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()));
        When(new AdicionarCebollaPicada(IdAgregado));
        Then(new AdicionCebollaPicadaFallida(AdicionCebollaPicadaFallida.RazonError.NoSeHaPicadoLaCebolla),
            new AdicionCebollaPicadaFallida(AdicionCebollaPicadaFallida.RazonError.SartenNoHaSidoEngrasado),
            new AdicionCebollaPicadaFallida(AdicionCebollaPicadaFallida.RazonError.FogonNoHaSidoEncendido));
    }
}



