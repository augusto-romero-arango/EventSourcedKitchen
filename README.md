# EventSourcedKitchen

Ejemplo con C# y event sourcing

## 1. Construir el dominio del problema

### Crear la clase base de los agregados

Debemos crear una clase abstracta que va a ser usada por los agregados donde  vamos a enunciar el método para aplicar comandos al agregado.

Se creó la clase abstracta `AggregateRoot` de donde deben heredar todos los agregados.

Esta solo contiene un método Apply que responde a un objeto, los cuales serán los comandos que le apliquemos al agregado.

### StoredEvent: Objeto para guardar eventos 

Los eventos se representarán con la clase `StoredEvent` que tendrá la inforamción mínima necesaria para poder almacenar las marcas de tiempo y números de sequencia necesarios para resolver el orden de los eventos.

En la propiedad `EventData` guardaremos el cuerpo del evento.

### IEventStore: Interfaz del repositorio

El repositorio tendrá la interfaz `IEventStore`. Esta abstracción nos permitirá cambiar la tecnología de guardado del evento en el futuro.

### EventStream (Cadena de eventos)

La cadena de eventos se encarga de "hidratar" la entidad y guardar los eventos aplicados.

> Hidratar significa aplicar todos los eventos generados para la entidad(agregado) para recuperar el estado actual.

### CommandHandler: La lógica del negocio

`CommandHandler` es la clase abstracta que usaremos para ejecutar la lógica de negocio de los comandos.

Le inyectamos `IEventStore` para que el comando tenga acceso a las entidades que necesite dentro del dominio.

El método `Handle` será el que ejecute la lógica de negocio.

Se debe crear un `CommandHandler` por cada evento que se reciba.

Se podrían ejecutar `eventStore.GuardarCambios()` directamente en el comando, pero con el fin de poder encadenar el guardado de varios comandos dentro de una transacción, se puede (y se recomienda) usar el `CommandRouter`

### CommandRouter

Para evitar que en agregados que invoquen varios comandos, se realice un guardado en la base de datos por cada comando, se puede usar el `CommandRouter`

### Crear la clase que va a ser nuestro agregado

Nuestro agregado será el que adminsitra el evento.

- Creamos una carpeta para el agregado en plural: `HuevosPericos`
- Creamos la clase del agregado en singular: `HuevoPerico`
  