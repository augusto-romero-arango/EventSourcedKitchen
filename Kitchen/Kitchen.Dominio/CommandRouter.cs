using Kitchen.Dominio.HuevosPericos;

namespace Kitchen.Dominio;

/// <summary>
/// Con el serviceProvider se pueden obtener las dependencias necesarias para los handlers.
/// Como CommandHandler es abstracto, garantizamos que siempre tenga el método Handle.
/// EventStore lo usamos para realizar el UnitOfWork y guardar los cambios.
/// </summary>
/// <param name="eventStore"></param>
/// <param name="serviceProvider"></param>
public class CommandRouter(IEventStore eventStore, IServiceProvider serviceProvider)
{
    public void HandleCommand(object command)
    {
        var commandType = command.GetType();
        var handlerType = typeof(CommandHandler<>).MakeGenericType(commandType);
        var handler = serviceProvider.GetService(handlerType);
        if (handler == null)
        {
            throw new Exception($"No existe el handler para el comando {commandType.Name}");
        }
        var handleMethod = handlerType.GetMethod("Handle");
        handleMethod?.Invoke(handler, [command]);
        
        eventStore.GuardarCambios();
    }
}

/// <summary>
/// Esta sería la implementación más sencilla para el router.
/// En el switch se deberían agregar todos los comandos que se vayan a manejar.
///
/// Pero la lógica dentro de cada case estaría repetida. Siempre será invocar el handler del comando y llamar el método Handle.
/// Adicionalmente, si un handler necesita más dependencias, se le deberían inyectar al router y podrían ser muy variadas.
/// </summary>
/// <param name="eventStore"></param>
[Obsolete("Esta implementación es muy sencilla y no es escalable. Se recomienda usar un contenedor de IoC para manejar las dependencias de los handlers.")]
public class CommandRouterSimple(IEventStore eventStore)
{
    public void HandleCommand(object command)
    {
        switch (command)
        {
            case BatirHuevos batirHuevos:
                var handler = new BatirHuevosHandler(eventStore);
                handler.Handle(batirHuevos);
                break;
            default:
                throw new Exception($"No existe el handler para el comando {command.GetType().Name}");
            
        }
        eventStore.GuardarCambios();
        
    }
}