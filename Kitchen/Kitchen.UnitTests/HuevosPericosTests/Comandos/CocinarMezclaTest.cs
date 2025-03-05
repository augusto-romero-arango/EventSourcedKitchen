using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class CocinarMezclaTest : CommandHandlerTest<CocinarMezcla>
{
    protected override CommandHandler<CocinarMezcla> Handler => new CocinarMezclaHandler(EventStore);

    [Fact]
    public void CocinarMezcla_CunadoHuevosEstanCocinando_EmiteHuevosPericosCocinados()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()),
            new HuevosBatidos(2),
            new HuevosSalados(), 
            new CebollaPicada(1), 
            new TomatePicado(1), 
            new SartenEngrasado(),
            new FogonEncendido(), 
            new CebollaPicadaAdicionada(),
            new TomatePicadoAdicionado(),
            new HuevosSaladosAdicionados());
        When(new CocinarMezcla(IdAgregado));
        Then(new HuevoPericoCocinado());

        var huevosPericos = ObtenerEntidad<HuevoPerico>();
        huevosPericos.Estado.Should().Be(EstadoHuevoPerico.Finalizado);
    }
}



