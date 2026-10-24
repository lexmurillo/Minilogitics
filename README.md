# MiniLogistics

Sistema básico de logística desarrollado con .NET, compuesto por una aplicación de escritorio y tres microservicios independientes.

El proyecto busca comprender la comunicación entre servicios, la persistencia de datos por microservicio y la ejecución de la infraestructura mediante Docker Compose.

## Tecnologías

- .NET 10
- C# y WPF
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Docker y Docker Compose
- HTTP entre microservicios

## Arquitectura

```text
MiniLogistics.Wpf
       |
       | HTTP
       |
  +----+-------------+
  |         |        |
  v         v        v
Packages  Shipping  Tracking
  .Api      .Api      .Api
  |         |         |
  v         v         v
Packages  Shipping  Tracking
  Db        Db        Db
```

Cada microservicio es responsable de su propia base de datos. La aplicación WPF consume las APIs y no se conecta directamente a SQL Server.

## Microservicios

### Packages.Api

Responsable de administrar los paquetes y su información.

Entidad principal: `Package`.

Datos previstos:
- Id
- Description
- Weight
- Origin
- Destination
- CreatedAt

### Shipping.Api

Responsable de administrar los envíos y sus estados.

Entidad principal: `Shipment`.

Estados definidos:
- `Created`
- `InTransit`
- `Delivered`

Shipping.Api valida la existencia del paquete mediante una solicitud HTTP a Packages.Api, sin acceder directamente a su base de datos.

### Tracking.Api

Responsable de registrar y consultar el historial de movimientos de los paquetes.

Entidad principal: `TrackingEvent`.

El servicio está diseñado para recibir eventos de seguimiento desde Shipping.Api mediante HTTP.

## Aplicación de escritorio

`MiniLogistics.Wpf` es el punto central desde el cual el usuario interactuará con el sistema.

La aplicación consume los microservicios mediante HTTP. La administración de paquetes, envíos y seguimiento se desarrolla de acuerdo con las fases del proyecto.

## Infraestructura con Docker

El proyecto incluye `docker-compose.yml` para ejecutar localmente los siguientes servicios:

- SQL Server
- Packages.Api
- Shipping.Api
- Tracking.Api

Para iniciar la infraestructura:

```bash
docker compose up -d
```

Para consultar el estado de los contenedores:

```bash
docker compose ps
```

Para consultar los registros:

```bash
docker compose logs -f
```

Para detener los contenedores sin eliminar los datos persistidos:

```bash
docker compose down
```

La aplicación WPF se ejecuta localmente, fuera de Docker.

## Estructura prevista

```text
MiniLogistics/
├── MiniLogistics.sln
├── src/
│   ├── Desktop/
│   │   └── MiniLogistics.Wpf/
│   └── Services/
│       ├── Packages/
│       │   └── Packages.Api/
│       ├── Shipping/
│       │   └── Shipping.Api/
│       └── Tracking/
│           └── Tracking.Api/
├── tests/
│   ├── Packages.Tests/
│   ├── Shipping.Tests/
│   └── Tracking.Tests/
└── docker-compose.yml
```

## Restricciones del proyecto

- Cada microservicio es dueño de sus datos.
- Ningún servicio consulta directamente las tablas de otro microservicio.
- WPF no accede directamente a las bases de datos.
- La comunicación inicial entre servicios utiliza HTTP.
- No se utiliza RabbitMQ en esta versión.
- No se incorpora MediatR, CQRS ni Kubernetes.
- Se prioriza una implementación pequeña y funcional, sin agregar patrones innecesarios.

## Objetivo de aprendizaje

Comprender de forma práctica:

- Comunicación WPF → API.
- Persistencia API → base de datos.
- Comunicación API → API.
- Separación de datos por microservicio.
- Uso de `HttpClient` e `IHttpClientFactory`.
- Orquestación local mediante Docker Compose.

## Próxima iteración

Una vez completada esta versión, se plantea evaluar la sustitución de la comunicación HTTP entre Shipping.Api y Tracking.Api por un intermediario de mensajes, utilizando eventos como `ShipmentStatusChanged`.

**Estado:** proyecto en desarrollo. Las funcionalidades se incorporarán progresivamente según las fases definidas.