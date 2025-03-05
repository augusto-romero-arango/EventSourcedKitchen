using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class AdicionarHuevosSaladosTest : CommandHandlerTest<AdicionarHuevosSalados>
{
    protected override CommandHandler<AdicionarHuevosSalados> Handler=> new AdicionarHuevosSaladosHandler(EventStore);

    [Fact]
    public void AdicionarHuevosSalados_CuandoTomatePicadoAdicionado_Y_HuevosSalados_EmiteHuevosSaladosAdicionados()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()),
            new HuevosBatidos(2),
            new CebollaPicada(1),
            new TomatePicado(1),
            new SartenEngrasado(),
            new FogonEncendido(),
            new CebollaPicadaAdicionada(),
            new TomatePicadoAdicionado(),
            new HuevosSalados()
           );
        When(new AdicionarHuevosSalados(IdAgregado));
        Then(new HuevosSaladosAdicionados());

        var huevos = ObtenerEntidad<HuevoPerico>();
        huevos.Estado.Should().Be(EstadoHuevoPerico.CocinandoHuevos);
    }
    
    [Fact]
    public void AdicionarHuevosSalados_CuandoTomatePicadoNoHaSidoAdicionado_Falla()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()),
            new HuevosBatidos(2),
            new CebollaPicada(1),
            new TomatePicado(1),
            new SartenEngrasado(),
            new FogonEncendido(),
            new CebollaPicadaAdicionada(),
            new HuevosSalados());
        When(new AdicionarHuevosSalados(IdAgregado));
        Then(new AdicionHuevosSaladosFallido(AdicionHuevosSaladosFallido.RazonError.TomateNoHaSidoAdicionado));

        var huevos = ObtenerEntidad<HuevoPerico>();
        huevos.Estado.Should().Be(EstadoHuevoPerico.SofriendoCebolla);
    }
    [Fact]
    public void AdicionarHuevosSalados_CuandoTomatePicadoAdicionado_Y_HuevosNoHanSidoSalados_Falla()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()),
            new HuevosBatidos(2),
            new CebollaPicada(1),
            new TomatePicado(1),
            new SartenEngrasado(),
            new FogonEncendido(),
            new CebollaPicadaAdicionada(),
            new TomatePicadoAdicionado()
        );
        When(new AdicionarHuevosSalados(IdAgregado));
        Then(new AdicionHuevosSaladosFallido(AdicionHuevosSaladosFallido.RazonError.LosHuevosNoHanSidoSalados));

        var huevos = ObtenerEntidad<HuevoPerico>();
        huevos.Estado.Should().Be(EstadoHuevoPerico.SofriendoCebollaConTomate);
    }
    [Fact]
    public void AdicionarHuevosSalados_CuandoNoHaAdicionadoTomate_Ni_HuevosNoHanSidoSalados_Falla()
    {
        Given(new HuevosPericosIniciados(IdAgregado, Guid.NewGuid()),
            new HuevosBatidos(2),
            new CebollaPicada(1),
            new TomatePicado(1),
            new SartenEngrasado(),
            new FogonEncendido(),
            new CebollaPicadaAdicionada()
        );
        When(new AdicionarHuevosSalados(IdAgregado));
        Then(new AdicionHuevosSaladosFallido(AdicionHuevosSaladosFallido.RazonError.TomateNoHaSidoAdicionado),
            new AdicionHuevosSaladosFallido(AdicionHuevosSaladosFallido.RazonError.LosHuevosNoHanSidoSalados));

        var huevos = ObtenerEntidad<HuevoPerico>();
        huevos.Estado.Should().Be(EstadoHuevoPerico.SofriendoCebolla);
    }
}



