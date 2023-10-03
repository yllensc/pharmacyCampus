# pharmacyCampus 

Backend de una farmacia para gestión administrativa con CSharp a través de estructura de 4 capas y code-first migration

## Comenzando 🚀

Como cliente de una farmacia, requiero un sistema de gestión que me permita gestionar las ventas y compras, interactuar con proveedores, empleados y pacientes, generar informes de ventas y caducidad de medicamentos. Es esencial que este software pase por un proceso de análisis de requerimientos, diseño, implementación, pruebas, y eventual retiro, garantizando en todo momento la adaptabilidad, confiabilidad y eficiencia para las operaciones diarias de la farmacia.

#EndPoints requeridos
1. Obtener todos los medicamentos con menos de 50 unidades en stock.
2. Listar los proveedores con su información de contacto en medicamentos.
3. Medicamentos comprados al ‘Proveedor A’.
4. Obtener recetas médicas emitidas después del 1 de enero de 2023.
5. Total de ventas del medicamento ‘Paracetamol’.
6. Medicamentos que caducan antes del 1 de enero de 2024.
7. Total de medicamentos vendidos por cada proveedor.
8. Cantidad total de dinero recaudado por las ventas de medicamentos.
9. Medicamentos que no han sido vendidos.
10. Obtener el medicamento más caro.
11. Número de medicamentos por proveedor.
12. Pacientes que han comprado Paracetamol.
13. Proveedores que no han vendido medicamentos en el último año.
14. Obtener el total de medicamentos vendidos en marzo de 2023.
15. Obtener el medicamento menos vendido en 2023.
16. Ganancia total por proveedor en 2023 (asumiendo un campo precioCompra en Compras).
17. Promedio de medicamentos comprados por venta.
18. Cantidad de ventas realizadas por cada empleado en 2023.
19. Obtener todos los medicamentos que expiren en 2024.
20. Empleados que hayan hecho más de 5 ventas en total.
21. Medicamentos que no han sido vendidos nunca.
22. Paciente que ha gastado más dinero en 2023.
23. Empleados que no han realizado ninguna venta en 2023.
24. Proveedor que ha suministrado más medicamentos en 2023.
25. Pacientes que compraron el medicamento “Paracetamol” en 2023.
26. Total de medicamentos vendidos por mes en 2023.
27. Empleados con menos de 5 ventas en 2023.
28. Número total de proveedores que suministraron medicamentos en 2023.
29. Proveedores de los medicamentos con menos de 50 unidades en stock.
30. Pacientes que no han comprado ningún medicamento en 2023.
31. Medicamentos que han sido vendidos cada mes del año 2023.
32. Empleado que ha vendido la mayor cantidad de medicamentos distintos en 2023.
33. Total gastado por cada paciente en 2023.
34. Medicamentos que no han sido vendidos en 2023.
35. Proveedores que han suministrado al menos 5 medicamentos diferentes en 2023.
36. Total de medicamentos vendidos en el primer trimestre de 2023.
37. Empleados que no realizaron ventas en abril de 2023.
38. Medicamentos con un precio mayor a 50 y un stock menor a 100.


### Pre-requisitos 📋

.NET 7.0
MySQL

### Instalación 🔧

Migración de la base de datos (code-first migration):
Ejecuta los comandos:
```
1. dotnet ef migrations add ¨[nombreDeLaMigracion] --project ./Persistence --startup-project API --output-dir ./Data/Migrations
2. dotnet ef database update --project ./Infrastructure --startup-project ./API
```

Ejecución de la WebApi (desde la ruta del proyecto):
Ejecuta los comandos:
```
1. cd API
2. dotnet run
```
Al terminar, como es un proyecto local de momento, obtienes la información del localhost:
![image](https://github.com/yllensc/pharmacyCampus/assets/117176562/635e9c41-4ca9-4733-8cc9-af45790c9051)

## Ejecutando las pruebas ⚙️

Endpoints ✌️🤘😎🏥🆗🙌

## Construido con 🛠️

* [ASP.NET Core]([http://www.dropwizard.io/1.0.2/docs/](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio)) - El framework web usado
* [MySql]([https://maven.apache.org/](https://dev.mysql.com/doc/workbench/en/wb-mysql-utilities.html)) - Base de datos


## Autores ✒️

* **Margie Bocanegra** - [Marsh1100](https://github.com/Marsh1100)
* **Juan Pablo Lozada** - [Juarika](https://github.com/Juarika)
* **Yllen Santamaría** - [Yllensc](https://github.com/yllensc)
