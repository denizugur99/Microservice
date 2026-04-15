using Microsoft.Extensions.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = DistributedApplication.CreateBuilder(args);

//mongo.db.catalog:
//    image: mongo: latest
//    container_name: mongodb
//    restart: always
//    ports:
//      -"27017:27017"
//    environment:
//MONGO_INITDB_ROOT_USERNAME: ${ MONGO_USERNAME}
//MONGO_INITDB_ROOT_PASSWORD: ${ MONGO_PASSWORD}
//volumes:
//-mongodb_data:/ data / db

var mongoUser=builder.AddParameter("MONGO-USERNAME");
var mongoPassword=builder.AddParameter("MONGO-PASSWORD");

var catalogmongoDB=builder.AddMongoDB("catalogmongoDB",27017,mongoUser,mongoPassword).WithImage("mongo: latest").WithDataVolume("mongodb_data").AddDatabase("CatalogDb");


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
