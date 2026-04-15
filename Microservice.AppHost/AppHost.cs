var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Microservice_Basket_Api>("microservice-basket-api");

builder.AddProject<Projects.Microservice_Catalog_Api>("microservice-catalog-api");

builder.AddProject<Projects.Microservice_Discount_API>("microservice-discount-api");

builder.AddProject<Projects.microservice_Email>("microservice-email");

builder.AddProject<Projects.Microservice_File_Api>("microservice-file-api");

builder.AddProject<Projects.Microservice_Gateway>("microservice-gateway");

builder.AddProject<Projects.Microservice_Order_Api>("microservice-order-api");

builder.AddProject<Projects.Microservice_Payment_Api>("microservice-payment-api");

builder.AddProject<Projects.MicroserviceWebApp>("microservicewebapp");

builder.Build().Run();
