# EventSourcedKitchen

Ejemplo con C# y event sourcing

## Cocinita: Contexto del ejercicio

[MIRO](https://miro.com/app/board/uXjVIWSuopc=/?share_link_id=671970103168)

![EventStorming de la cocina](Assets\img\eventStorming.png)

Tenemos la solución para hacer una cocina. En este momento, el único producto que hacemos el preparar huevos pericos.

En el diagrama podemos ver los eventos descubiertos por nuestros expertos cocineros, los cuales vamos a modelar.

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
  
## 2. Pruebas unitarias del dominio

### 2.1 Alcance de las pruebas (Suftware Under Test - SUT)

Como la lógica del dominio está principalmente en los `handlers` se va a construir un wrapper para poder hacer las pruebas unitarias al rededor de los eventos previos y los eventos que se generan gracias cuando se ejecute el handler.

### 2.2 TestEventStore

Creamos una clase que herede de `IEventStore` con el fin de hacer un mock del almacenamiento con una implementación en memoria.

Internamente creamos dos colecciones de `StoredEvent` uno para las precondiciones y otra para los resultados.

#### EventosPasados

La colección `_eventosPasados` representan todos los eventos que registramos en el test que sucedieron antes de ejecutar el SUT.

#### EventosGenerados

La colección `_eventosGenerados` representan los eventos que se crearon después de la ejecucuión del `handler`.
Servirán para acertar los eventos creados por el handler.

### 2.3 CommandHandlerTest

`CommandHandlerTest` es una clase abstracta que heredarán los tests de dominio para los eventos.

Esta clase accede a `TestEventStore` para inyectar el storeEvent en memoria que es usado para las pruebas unitarias.

Inspierado en [Gherkin](https://cucumber.io/docs/gherkin/reference), se crean tres métodos: `Given`, `When` , `Then`.

La intención es declarar las pruebas en una sintaxis parecida a:

**Dado que (Given)** Los huevos han sido batidos

**Cuando (When)** voy a salar los huevos

**Entonces (Then)** los huevos fueron salados


#### Given

En español "Dado que". Representará el registro de los eventos anteriores que asumimos para la prueba fueron aplicados antes que corra el handler que vamos a correr.

**La lista de eventos debe ser registrada en orden cronológcio.**

#### When

En español "Cuando". Ejecutará el comando pasándolo al `Handler` que haya sido instanciado en la prueba.

#### Then

En español "Entonces". Validará cada uno de los eventos que el handler emitirá en el orden estricto y comparará cada una de sus propiedades.

