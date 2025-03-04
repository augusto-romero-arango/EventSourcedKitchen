using FluentAssertions;
using Kitchen.Dominio;

namespace Kitchen.UnitTests;

public abstract class CommandHandlerTest<TComando>
{
    protected Guid IdAgregado = Guid.NewGuid();
    
    protected TestEventStore EventStore = new();
    
    protected abstract CommandHandler<TComando> Handler { get; }

    protected void Given(Guid aggregateId, params object[] eventos)
    {
        EventStore
            .AnterioresEventos
            .AddRange(
                eventos.Select((evento, i) => new StoredEvent(aggregateId, i, DateTime.UtcNow, evento))
                );
    }
    protected void  Given(params object[] eventos)
    {
        Given(IdAgregado, eventos);
    }

    protected void When(TComando comando)
    {
        Handler.Handle(comando);
    }
    
    protected void Then(params object[] eventosEsperados)
    {
        Then(IdAgregado, eventosEsperados);
    }

    protected void Then(Guid aggregateId, params object[] eventosEsperados)
    {
        var eventoCreados = EventStore.NuevosEventos
            .Where(e => e .AggregateId== aggregateId)
            .OrderBy(e => e.SequenceNumber)
            .Select(e => e.EventData)
            .ToArray();
        
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
    
    
   
}