using FluentAssertions;
using Kitchen.Dominio;

namespace Kitchen.UnitTests;

public abstract class CommandHandlerTest<TComando>
{
    /// <summary>
    /// Instanciar el handler que se va a probar.
    /// </summary>
    protected abstract CommandHandler<TComando> Handler { get; }
    
    /// <summary>
    /// Es un id del agregado que vams a testear, se puede usar para pruebas que solo necesiten un agregado.
    /// En caso que esté probando más de un agregado, use las sobrecargas  que piden id IdAgregado.
    /// </summary>
    protected Guid IdAgregado = Guid.NewGuid();
    
    protected TestEventStore EventStore = new();

    /// <summary>
    /// Registrar los eventos que debeieron haber sucedido antes de ejecutar el comando.
    /// Si la prueba solo necesita un agregado, use la sobrecarga sin el IdAgregado.
    /// </summary>
    /// <param name="idAgregado">El IdAgregado del stream que vamos a afectar.</param>
    /// <param name="eventos">Eventos instanciados con los datos de pruebas que se deben aplicar antes del comando.</param>
    protected void Given(Guid idAgregado, params object[] eventos)
    {
        EventStore.AgregarEventosAnteriores(idAgregado, eventos);
    }
    
    /// <summary>
    /// Registrar los eventos que debeieron haber sucedido antes de ejecutar el comando.
    /// </summary>
    /// <param name="eventos">Eventos instanciados con los datos de pruebas que se deben aplicar antes del comando.</param>
    protected void  Given(params object[] eventos)
    {
        Given(IdAgregado, eventos);
    }
    
    

    /// <summary>
    /// Ejecuta el handler del comando instanciado en la prueba.
    /// </summary>
    /// <param name="comando">Registre el comando a ejecutar en la prueba.</param>
    protected void When(TComando comando)
    {
        Handler.Handle(comando);
    }
    
    /// <summary>
    /// Ejecuta la aserción de los eventos ordenados que se debarían haber emitido en la ejecución del comando.
    /// Si la prueba solo necesita un agregado, use la sobrecarga sin el IdAgregado.
    /// </summary>
    /// <param name="aggregateId">El IdAgregado del stream que vamos a afectar.</param>
    /// <param name="eventosEsperados">Conjunto de eventos ordenados que se deberían haber emitido al momento de ejecutar el comando.</param>
    protected void Then(Guid aggregateId, params object[] eventosEsperados)
    {
        var eventoCreados = EventStore.ObtenerEventosGenerados(aggregateId);
        
        eventoCreados.Length.Should().Be(eventosEsperados.Length);
        for (var i = 0; i < eventoCreados.Length; i++)
        {
            eventoCreados[i].Should().BeOfType(eventosEsperados[i].GetType());
            try
            {
                eventoCreados[i].Should().BeEquivalentTo(eventosEsperados[i]);
            }
            catch (InvalidOperationException e)
            {
                // Empty event with matching type is OK. This means that the event class
                // has no properties. If the types match in this situation, the correct
                // event has been appended. So we should ignore this exception.
                if (!e.Message.StartsWith("No members were found for comparison."))
                    throw;
            }
        }
    }
    
    /// <summary>
    /// Ejecuta la aserción de los eventos ordenados que se debarían haber emitido en la ejecución del comando.
    /// </summary>
    /// <param name="eventosEsperados">Conjunto de eventos ordenados que se deberían haber emitido al momento de ejecutar el comando.</param>
    protected void Then(params object[] eventosEsperados)
    {
        Then(IdAgregado, eventosEsperados);
    }

    /// <summary>
    /// Obtenga la entidad qeu se va a probar a la que ya se le hayan aplicado todos los eventos hasta el momento.
    /// </summary>
    /// <typeparam name="TEntidad">Entidad que herede de AgreggateRoot.</typeparam>
    /// <returns>Entidad con el estado posterior a la aplciación de los eventos registrados.</returns>
    protected TEntidad ObtenerEntidad<TEntidad>()
        where TEntidad : AggregateRoot, new()
    {
        return ObtenerEntidad<TEntidad>(IdAgregado);
    } 
    
    /// <summary>
    /// Obtenga la entidad qeu se va a probar a la que ya se le hayan aplicado todos los eventos hasta el momento.
    /// </summary>
    /// <typeparam name="TEntidad">Entidad que herede de AgreggateRoot.</typeparam>
    /// <param name="idAgregado">El id del agregado que se desea recuperar.</param>
    /// <returns>Entidad con el estado posterior a la aplciación de los eventos registrados.</returns>
    protected TEntidad ObtenerEntidad<TEntidad>(Guid idAgregado) 
        where TEntidad : AggregateRoot, new()
    {
        var stream = new EventStream<TEntidad>(EventStore, idAgregado);
        return stream.CargarEntidad();
    }
    
    
   
}