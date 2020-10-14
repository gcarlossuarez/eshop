# Prueba .NET Core

## Contexto

El cliente XXX necesita que su portal Web pueda listar un  catálago de productos. Estos productos deben poderse crear, actualizar y eliminar. Cada producto de contar con un identificador único, descripción, marca, modelo, precio unitario, porcentaje de descuento y estado del producto (Creado, Eliminado).

## Instrucciones

+ Clonar esta solución.
+ Definir en el proyecto catalog.api un recurso para gestionar los productos.
+ El recurso de productos debe contar con los métodos de creación, actualización, eliminación, obtención por productos, y obtención de todos los productos.
+ Crear un nuevo proyecto llamado **eshop.webapp** que contenga un sitio web, que como página inicial muestre el listado de productos creados en el cátalogo. 
+ Cada producto listado debe tener un enlace que permita hacer la actualización de un producto. 
+ Adicionalmente la página inicial debe contar con un botón que redireccione a un formulario y permita crear un nuevo producto.
+ La creación y actualización de productos debe contar con las validaciones del caso.
+ Una vez desarrollada la prueba colgar en tu cuenta de **github** en un repositorio público para poder descargarla y revisarla.

# Consideraciones

+ El código debe estar escrito en inglés.
+ Aplicar buenas prácticas en codificación, como nombres de variables, comentariado de código, entre otros.
+ Hacer uso de SQL Server como base de datos para la persistencia de la información.
+ Deseable no requerido, crear el proyecto **eshop.webapp** con Angular.
+ Deseable no requerido, hacer uso de librerías terceras para temas de validaciones, mapeos, entre otras.
+ Deseable no requerido, publica tu solución en **dockerhub**.