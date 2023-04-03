## SOLID 

- Single-responsability principle:
    - Splitting functionality into blocks, each addressing a specific concern. 
    - One block of code shouldn't be trying to do many different things
- **Open-closed:** open for extension but close for modification. Ability to extend functionality without having to modify the original implementation.
- **Liskov Substitution principle:** each derived class should be sustitutable by its base or parent class.
- **Interface segregation principle:** a client shouldn't need to implement methods that it doesn't need. 
- **Dependency inversion principle:** entities should depend on abstraction (interfaces and abstract classes) not implementations. 
    - Loose-coupling in applications. 

## DRY - Don't repeat yourself

- Less code repetition
- One implementation point for code in your application
- Open/ Closed principle oonly works when DRY is folowed.

## All-In-One architecture:
- Pros:
    - Easier to deliver.
    - Can be stable and long term solution
- Cons:
    - Hard to enforce SOLID principles
    - Harder to mantain as project grows.
    - Harder to test

## Layered Architecture (web layer, service layer, repository layer)
- Pros:
    - Better enforcing of SOLID principles
    - Easier to mantain larger code base
- Cons:
    - Layers are dependent
    - Still acts as one application
    - Logic is sometimes scattered across layers

## Onion Architecture

- Pros:
    - Better testability as unit tests can be created for separate layers.
    - Promotes loose coupling.
    - Easier to make changes in code base without directly affecting other modules.
- Cons:
    - Learning curve
    - Time consuming

## Clean arquitecture:

- Create a domain project with its entities (HR.LeaveManagement.Domain). For example LeaveAllocation.cs
```c#
using HR.LeaveManagement.Domain.Common;

namespace HR.LeaveManagement.Domain;

public class LeaveAllocation : BaseEntity
{
	public int NumberOfDays { get; set; }
	public LeaveType LeaveType { get; set; }
	public int LeaveTypeId { get; set; }
	public int Period  { get; set; }
}
```
- Create an application project with repository pattern (a generic repository interface and one interface for each table (entity) which implements the generic repository interface).
- IGenericRepository.cs (Crud operations)
```c#
namespace HR.LeaveManagement.Application.Contracts.Persistance;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetAsync(int id);
    Task<T> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
}
```
- ILeaveAllocationRepository.cs
```c#
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Persistance;

public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
{

}
```

## Automapper
- Converts complex data types with ease. 
- Saves time spent on writing manual mapping code.

## CQRS Pattern: command query responsability segregation

- When user makes change to data we invoke a command -> command updates database.
- When user wants to retrieve data we invoke a query that reads from database. 
- An ideal scenario would have two data storages: one read-only database for querying and one transactional database for modifications. 

## Mediator Pattern:
- Manage the relationships between requests and handlers. 

- In the application project add Automapper, Automapper DI and MediatR DI nuget packages.
- Create an ApplicationServiceRegistration class to register both dependencies:
- ApplicationServiceRegistration.cs
```c#
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HR.LeaveManagement.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }

    }
}
```
- Map business entities to Dtos and viceversa:
- MappingProfiles/LeaveTypeProfile
```c#
using System;
using AutoMapper;
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.MappingProfiles
{
	public class LeaveTypeProfile : Profile
	{
		public LeaveTypeProfile()
		{
			CreateMap<LeaveTypeDto, LeaveType>().ReverseMap();
		}
	}
}
```



