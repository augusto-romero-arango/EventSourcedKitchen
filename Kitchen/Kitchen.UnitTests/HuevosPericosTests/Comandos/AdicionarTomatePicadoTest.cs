using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class AdicionarTomatePicadoTest : CommandHandlerTest<AdicionarTomatePicado>
{
    protected override CommandHandler<AdicionarTomatePicado> Handler => new AdicionarTomatePidadoHandler(EventStore);


    [Fact]
    public void AdicionarTomatePicado_CuandoCebollaPicadaAdicionada_EmiteTomatePicadoAdicionado()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()), 
            new HuevosBatidos(2),
            new CebollaPicada(1),
            new FogonEncendido(),
            new SartenEngrasado(),
            new CebollaPicadaAdicionada()
            );    
        When(new AdicionarTomatePicado(IdAgregado));
        Then(new TomatePicadoAdicionado());

        var huevoPerico = ObtenerEntidad<HuevoPerico>(IdAgregado);
        huevoPerico.Estado.Should().Be(EstadoHuevoPerico.SofriendoCebollaConTomate);
    }
    
    [Fact]
    public void AdicionarTomatePicado_CuandoCebollaPicadaNoHaSidoAdicionada_Falla()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()), 
            new HuevosBatidos(2),
            new CebollaPicada(1),
            new FogonEncendido(),
            new SartenEngrasado()
        );    
        When(new AdicionarTomatePicado(IdAgregado));
        Then(new AdicionTomatePicadoFallido(AdicionTomatePicadoFallido.RazonError.NoSeHaAdicionadoCebolla));

        var huevoPerico = ObtenerEntidad<HuevoPerico>(IdAgregado);
        huevoPerico.Estado.Should().Be(EstadoHuevoPerico.Iniciado);
    }
    
}



