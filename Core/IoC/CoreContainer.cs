/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Autofac;
using AutoMapper;
using Common;
using Common.Contract.Messaging;
using Core.DB;
using Core.Handler.Car;
using Core.Handler.User;
using Core.Handler.Order;
using Core.LIB;

namespace Core.IoC
{
    public class CoreContainer
    {
        public static void Bind(ContainerBuilder builder)
        {
            // LIB (request handler library)
            builder.RegisterType<ResponseFactory>().As<IResponseFactory>();
            builder.RegisterType<HandlerCaller>().As<IHandlerCaller>();
            builder.RegisterType<RequestHandlerFactory>().As<IRequestHandlerFactory>();


            // CoreService
            builder.RegisterType<CoreService>().As<ICoreService>();


            // DB
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            builder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>();
            builder.RegisterType<LogicFactory>().As<ILogicFactory>();
            builder.RegisterType<MapperFactory>().As<IMapperFactory>();


            // AutoMapper
            builder.Register(c =>
            {
                // Create AutoMapper configuration
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MappingProfile()); // Add your AutoMapper profiles here
                });

                // Create IMapper instance using the configuration
                return configuration.CreateMapper();
            }).As<IMapper>().SingleInstance(); // Register as a singleton since IMapper is usually reused.


            // Integrations


            // All handlers

            // User handlers
            builder.RegisterType<GetUserHandler>().As<RequestHandler<GetUserReq, GetUserResp>>();
            builder.RegisterType<GetAllUserHandler>().As<RequestHandler<GetAllUserReq, GetAllUserResp>>();

            // Car handlers (SIMPLE LOGIC EXAMPLE - uses CarLogic for business rules)
            builder.RegisterType<AddCarHandler>().As<RequestHandler<AddCarReq, AddCarResp>>();
            builder.RegisterType<GetAllCarHandler>().As<RequestHandler<GetAllCarReq, GetAllCarResp>>();
            builder.RegisterType<GetUserCarsHandler>().As<RequestHandler<GetUserCarsReq, GetUserCarsResp>>();

            // Order handlers (COMPLEX LOGIC EXAMPLE - OrderLogic depends on CarLogic)
            builder.RegisterType<CreateOrderHandler>().As<RequestHandler<CreateOrderReq, CreateOrderResp>>();
            builder.RegisterType<CancelOrderHandler>().As<RequestHandler<CancelOrderReq, CancelOrderResp>>();
            builder.RegisterType<GetOrderSummaryHandler>().As<RequestHandler<GetOrderSummaryReq, GetOrderSummaryResp>>();
            builder.RegisterType<GetUserOrdersHandler>().As<RequestHandler<GetUserOrdersReq, GetUserOrdersResp>>();
        }
    }
}
