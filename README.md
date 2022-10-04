# Redarbor-WebApi

### Funcionalidades:
- Obtener todos empleados con paginación. 
- Obtener todos empleados sin paginación. 
- Obtener empleado por id con paginación. 
- Obtener empleado por id sin paginación. 
- Agregar un nuevo empleado. 
- Actualizar un empleado existenete.
- Eliminar un empleado por id.

### Características tecnicas APP
- BackEnd: FrameWork .Net Core 3.1
  - API Rest
  - Patron de diseño: GenericRepository - CQRS
  - Arquitectura: Por capas MVC
  - ORM: Micro Orm Dapper(v2.0.123)
  - Db SQL Server
  - Librerías: AutoMapper(v12.0.0), FluentValidation(v11.2.2), Swagger(v6.4.0), SqlCLient(v4.8.3), Newtonsoft(v13.0.1), MediatR(v11.0.0)
  
**Estas instrucciones te permitirán obtener una copia del proyecto en funcionamiento en tu máquina local para propósitos de desarrollo y pruebas.**
**Pre-requisitos**

* .Net Core 5.0
* Sql Server 2016 o superior
* Sql Management studio 2016 o superior
* Visual studio 2019

- Opcionales
    1. SourceTree (Cliente para manejo de git)

**Compilación**
1. Desrcargar o clonar el proyecto
2. Abrir la carpeta donde se encuentra ubicado el proyecto
  - Pasos DB:
    1. Abrir Sql Management
    2. Conectar a localhost con su respectivos usuario y contraseña
    3. Abrir la carpeta Scripts_DB
    4. Ejecutar Scritp 01 en Sql Management
   - Pasos Backend:
        1. Abrir la carpeta WebApi
        2. Abrir la solución en Visual Studio 2019 (**Preferiblemente**)    
        3. En la pestaña *Solution Explorer (Explorador de la solución)* haga click derecho sobre la solución y seleccione la opción *Clean (Limpiar)*
        4. En la pestaña *Solution Explorer (Explorador de la solución)* haga click derecho sobre la solución y seleccione la opción *Build Solution (Compilar)*
        5. Abrir el archivo appsettings.json en la sección "ConnectionStrings" y modificar las llaves "ConexionDB" según el tipo de logeo que realiza en Sql Management.
       **NOTA:** Si el login en Sql Management lo tiene con autentiucación de windows dejar la configuración como se encuentra
        6. Haga click en el botón Play(IIS Express) o oprima la tecla F5
        7. Espere que se compile la solución y se abra la ventana de Swagger
  
**Autor**

* Yoe Andres Cardenas - Desarrollador full stack
