# TDD-JGC

Proyecto de práctica sobre Validación y Pruebas (TDD) en C# (.NET Framework 4.8).

## Objetivo
Implementar un conjunto de modelos (Usuario, Proyecto, Roles y Permisos) con énfasis en:
- Evolución de roles: pasar de `enum Roles` rígido a roles dinámicos (`DynamicRoleRegistry`).
- Asignación de permisos por rol.
- Asociación de un usuario a múltiples proyectos con uno o varios roles por proyecto (clase `App`).
- Validación de reglas de negocio mediante pruebas unitarias (MSTest).

## Estructura de la solución
- `Utils/` Modelos y lógica principal (`User`, `Project`, `Role`, `DynamicRoleRegistry`, `Permissions`).
- `Main/` Ejemplo de capa de aplicación con la clase `App` que relaciona usuario, proyectos y roles.
- `UserTest/` Proyecto de pruebas (MSTest) validando creación de usuarios, proyectos y manejo dinámico de roles.

## Clase App
Gestiona las participaciones del usuario:
- Evita duplicar proyectos y roles (usa `Dictionary<Project, HashSet<Role>>`).
- Permite añadir un rol por instancia o por nombre (`AddProjectRole`).
- Permite añadir múltiples roles (`AddProjectRoles`).
- Obtiene roles de un proyecto (`GetRoles`).
- Obtiene lista de proyectos (`GetProjects`).
- Genera representaciones de texto (`GetProjectRolesDisplay`, `GetAllParticipationsDisplay`).

## Ejemplo rápido de uso
```csharp
var user = new User("ana", "Ana", "López", "ana@example.com", "Secreta123", Language.ES, true, DateTime.Now.AddMonths(1));
var project = new Project("Portal QA", "PQA", "Portal de pruebas", true);
// Añadir rol dinámico extra
project.RoleRegistry.AddRole("Analista QA", new [] { Permissions.ExecuteTest, Permissions.CreateTest });
var app = new App(user);
app.AddProjectRole(project, "Tester");
app.AddProjectRole(project, "Analista QA");
Console.WriteLine(app.GetAllParticipationsDisplay());
// Salida aproximada:
// Portal QA: Analista QA, Tester
```

## Pruebas
Ejecutar desde Visual Studio Test Explorer o:
```
MSTest /testcontainer:UserTest\bin\Debug\UtilTest.dll
```
(Dependiendo de configuración y rutas).

Las pruebas cubren:
- Validación de propiedades y setters en `User` y `Project`.
- Gestión de roles y permisos dinámicos.
- Asignación y modificación de roles en proyectos.

## Requisitos
- .NET Framework 4.8
- MSTest (Microsoft.VisualStudio.TestTools.UnitTesting)

## Próximas mejoras sugeridas
- Persistencia (BD / archivo) de roles dinámicos.
- Validación granular de permisos al ejecutar acciones.
- Tests adicionales para la clase `App` (duplicados, formato de salida, múltiples proyectos).

## Licencia
Ver `LICENSE.txt`.