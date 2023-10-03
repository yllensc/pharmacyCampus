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
1. Obtener todos los medicamentos con menos de 50 unidades en stock (genérico)
   * localhost:5223/api/pharmacy/Medicine/underStock{50}
   ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/51d8d924-f229-4c3c-9e89-fbe563f5eaa7)
   ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/0e78022e-4b28-45ec-88c0-ea2ee5832361)
3. Listar los proveedores con su información de contacto en medicamentos.
    * localhost:5223/api/pharmacy/Medicine/GetProvidersInfoWithMedicines
   ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/e14ffb01-3aef-4401-840d-0ad2c02f0821)
   ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/7d5a4876-df8e-46e6-8b43-a6fe5b13b029)
4. Medicamentos comprados al ‘Proveedor A’
    * http://localhost:5223/api/pharmacy/Purchase/medicinesPurchased/{id}
     ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/3553d592-89cb-4abb-8cfe-b560c52c5eb3)
     ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/adf7445b-c539-4387-aa3c-a3de4e339d5a)
5.  Obtener recetas médicas emitidas después del 1 de enero de 2023
    * localhost:5223/api/pharmacy/Sale/recipes
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/c718f39d-70e0-458e-9014-2d5e85b6f11b)
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/a64be094-6248-4b8c-83eb-8d2885c58cdf)
6. Medicamentos que caducan antes del 1 de enero de 2024.(generico)
     * localhost:5223/api/pharmacy/Medicine/ExpiresUnder{2024}
       ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/8ae48e3f-5ced-43d5-a719-c4fdcafb73ce)
       ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/5918344f-e690-4f94-b5b2-85b3d3e2d666)
7. Total de medicamentos vendidos por cada proveedor.
     * localhost:5223/api/pharmacy/Provider/getProvidersWithCantMedicines
       ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/dce75e8c-bed4-4d39-8a6c-b40076650833)
       ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/027c9ce8-c0b7-4e68-8e32-106201a28bde)
8. Cantidad total de dinero recaudado por las ventas de medicamentos.
      * http://localhost:5223/api/pharmacy/Sale/gainSales
        ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/e39283a1-23e5-4d77-aad2-a12d295f9b5d)
        ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/09bab404-3a60-4741-ac90-d9790d08fcf9)
9. 34 Medicamentos que no han sido vendidos en el 2023
      * http://localhost:5223/api/pharmacy/Sale/unsoldMedicines2023
       ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/850fc558-f0a9-4a76-a00d-09ed6dfd734c)
       ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/42f52743-a01d-4243-9249-b78cec5f14da)
10. Obtener el medicamento más caro 
      * localhost:5223/api/pharmacy/Medicine/moreExpensive
        ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/411d776c-1e1c-4c86-ae00-68bc6bf6c333)
        ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/59869a84-1afe-4f48-bbb9-33ac506441dc)
11.Número de medicamentos por proveedor
      * localhost:5223/api/pharmacy/Provider/getProvidersWithCantTotalMedicines
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/6752a042-c23a-46a9-ac32-1c8a1c5d6347)
12. Pacientes que han comprado Paracetamol 
      * http://localhost:5223/api/pharmacy/Sale/patientsByMedicine 
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/d6e9bbe6-593a-4248-886a-da68e4a18a99)
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/54676ba9-edda-4be0-9d56-b04faf0eafdd)
13. Proveedores que no han vendido medicamentos en el último año 
      * http://localhost:5223/api/pharmacy/Purchase/providersWithoutPurchases
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/70d24a00-63a6-4a2b-b1ad-01da4adf187b)
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/490adde0-7fa0-4b99-a68b-9386e7235777)
14. Obtener el total de medicamentos vendidos en marzo (“cualquier mes”) de 2023
      * http://localhost:5223/api/pharmacy/Sale/month?month=9
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/938aa170-6849-44f0-bf5b-7ab0fb563d21)
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/17dda10c-2a93-4058-8001-47476ff3e563)
15. Obtener el medicamento menos vendido en 2023
      * http://localhost:5223/api/pharmacy/Sale/lessSoldMedicine
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/2b22a55d-7a35-4d00-b2a7-fb0c73e4f036)
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/c8b0a3fb-614a-417d-9325-bcc7f9b42b4d)
16. Ganancia total por proveedor en 2023.
    * http://localhost:5223/api/pharmacy/Provider/gain2023
    ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/f5736602-012a-4fc1-9a36-ab98c032119e)
    ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/d2d03318-1b8c-4c40-8a97-8d924ba7ac46)
17. Promedio de medicamentos comprados por venta
    * localhost:5223/api/pharmacy/Sale/average
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/a7943440-677e-4f61-b4b4-d537f8b7d8a0)
     ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/b3b39104-67f4-4ff8-8bd2-d6cff3aa16ee)
19. Obtener todos los medicamentos que expiren en 2024. (genérico)
    * localhost:5223/api/pharmacy/Medicine/ExpiresIn{2024}
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/fe5452a4-5ffa-46e9-83ab-20a75442e753)
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/65090029-6938-4efd-83c3-bd6c5c52731b)
20.Empleados que hayan hecho más de 5 ventas en total. (genérico)
    * localhost:5223/api/pharmacy/Employee/moreThan{2}Sales
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/83f2e1eb-6bcb-4fb0-9cb2-573aba5ac14c)
      ![image](https://github.com/yllensc/pharmacyCampus/assets/131481951/dae77adb-f1fe-4fe9-aee9-b301bccbcdc1)
      








## Construido con 🛠️

* [ASP.NET Core]([http://www.dropwizard.io/1.0.2/docs/](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio)) - El framework web usado
* [MySql]([https://maven.apache.org/](https://dev.mysql.com/doc/workbench/en/wb-mysql-utilities.html)) - Base de datos


## Autores ✒️

* **Margie Bocanegra** - [Marsh1100](https://github.com/Marsh1100)
* **Juan Pablo Lozada** - [Juarika](https://github.com/Juarika)
* **Yllen Santamaría** - [Yllensc](https://github.com/yllensc)
