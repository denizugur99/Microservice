using Microsoft.Extensions.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = DistributedApplication.CreateBuilder(args);
#region Catalog
var mongoUser =builder.AddParameter("MONGO-USERNAME");
var mongoPassword=builder.AddParameter("MONGO-PASSWORD");
var catalogmongoDB=builder.AddMongoDB("catalogmongoDB",27017,mongoUser,mongoPassword).WithImage("mongo: latest").WithDataVolume("mongodb_data").AddDatabase("CatalogDb");
var catalogApi=builder.AddProject<Projects.Microservice_Catalog_Api>("microservice-catalog-api");
catalogApi.WithReference(catalogmongoDB).WaitFor(catalogmongoDB);
#endregion

#region Basket
var redisPassword =builder.AddParameter("REDIS-PASSWORD");
var redisBasketDb=builder.AddRedis("redisBasketDb").WithImage("redis: latest").WithDataVolume("redis_data").WithPassword(redisPassword);
var basketApi=  builder.AddProject<Projects.Microservice_Basket_Api>("microservice-basket-api");
basketApi.WithReference(redisBasketDb);
#endregion

#region Discount
var mongoDiscountUser = builder.AddParameter("MONGO-USERNAME");
var mongoDiscountPassword = builder.AddParameter("MONGO-PASSWORD");
var discountMongoDB = builder.AddMongoDB("discountMongoDB", 27018, mongoDiscountUser, mongoDiscountPassword).WithImage("mongo: latest").WithDataVolume("mongodb_discount_data").AddDatabase("DiscountDb");
var discountApi = builder.AddProject<Projects.Microservice_Discount_API>("microservice-discount-api");
discountApi.WithReference(discountMongoDB).WaitFor(discountMongoDB);
#endregion

#region File
var fileApi = builder.AddProject<Projects.Microservice_File_Api>("microservice-file-api");
#endregion

#region Payment
var paymentApi =builder.AddProject<Projects.Microservice_Payment_Api>("microservice-payment-api");
#endregion

var sqlserverPassword=builder.AddParameter("SQLSERVER-PASSWORD");
var sqlserverOrderDb=builder.AddSqlServer("sqlserver-db-order").WithPassword(sqlserverPassword).WithDataVolume("sqlserver_order_data").AddDatabase("order-db-aspire");
 var orderApi = builder.AddProject<Projects.Microservice_Order_Api>("microservice-order-api");
orderApi.WithReference(sqlserverOrderDb).WaitFor(sqlserverOrderDb);


builder.AddProject<Projects.microservice_Email>("microservice-email");



builder.AddProject<Projects.Microservice_Gateway>("microservice-gateway");





builder.AddProject<Projects.MicroserviceWebApp>("microservicewebapp");

builder.Build().Run();
