using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class CocinarHuevoPericoTest : CommandHandlerTest<CocinarHuevoPerico>
{
    protected override CommandHandler<CocinarHuevoPerico> Handler=> 
        new CocinarHuevoPericoHandler(EventStore);
    
    [Fact]
    public void CocinarHuevoPerico_EmiteHuevosPericosIniciados()
    {
        var idOrden = Guid.NewGuid();
        When(new CocinarHuevoPerico(IdAgregado, idOrden));
        Then(new HuevosPericosIniciados(IdAgregado, idOrden));

        var huevoPerico = ObtenerEntidad<HuevoPerico>();
        huevoPerico.Id.Should().Be(IdAgregado);
        huevoPerico.IdOrden.Should().Be(idOrden);
        huevoPerico.Estado.Should().Be(EstadoHuevoPerico.Iniciado);
    }

    
}