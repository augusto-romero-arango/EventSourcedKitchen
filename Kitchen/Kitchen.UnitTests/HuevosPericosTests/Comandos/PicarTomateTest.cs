using FluentAssertions;
using Kitchen.Dominio;
using Kitchen.Dominio.HuevosPericos;
using Kitchen.Dominio.HuevosPericos.Comandos;

namespace Kitchen.UnitTests.HuevosPericosTests.Comandos;

public class PicarTomateTest : CommandHandlerTest<PicarTomate>
{
    protected override CommandHandler<PicarTomate> Handler => new PicarTomateHandler(EventStore);

    [Fact]
    public void PicarTomate_DebeEntregarTomatePicado()
    {
        Given(new CocinarHuevoPerico(IdAgregado, Guid.NewGuid()));
        When(new PicarTomate(IdAgregado, 1));
        Then(new TomatePicado(1));

        var huevoPerico = ObtenerEntidad<HuevoPerico>();
        huevoPerico.CantidadIngredientes(Ingredientes.Tomate).Should().Be(1);
    }
}


public record PicarTomate(Guid IdHuevoPerico, int CantidadTomates);
public class PicarTomateHandler(IEventStore eventStore) : CommandHandler<PicarTomate>(eventStore)
{
    public override void Handle(PicarTomate comando)
    {
        var stream = ObtenerStream<HuevoPerico>(comando.IdHuevoPerico);
        stream.Agregar(new TomatePicado(comando.CantidadTomates));
    }
}

