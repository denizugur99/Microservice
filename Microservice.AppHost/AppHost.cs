using Microsoft.Extensions.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = DistributedApplication.CreateBuilder(args);
#region Keycloak
var keycloakdb = "keycloak-db";
var postgresDbUser = builder.AddParameter("KEYCLOAK-DB-USER");
var postgresDbPassword = builder.AddParameter("KEYCLOAK-DB-PASSWORD");
var postgresDb = builder.AddPostgres("postgres-db-keycloak", postgresDbUser, postgresDbPassword, 5432).WithImage("postgres:16-alpine").WithVolume("microservice_postgres_keycloak_data", "/var/lib/postgresql/data").AddDatabase(keycloakdb);

var keycloakAdmin = builder.AddParameter("KEYCLOAK-ADMIN");
var keycloakAdminPassword = builder.AddParameter("KEYCLOAK-ADMIN-PASSWORD");

var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "latest")
    .WithHttpEndpoint(port: 8080, targetPort: 8080, name: "http")
    .WithEnvironment("KC_HOSTNAME_PORT", "8080")
    .WithEnvironment("KEYCLOAK_ADMIN", keycloakAdmin)
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", keycloakAdminPassword)
    .WithEnvironment("KC_HOSTNAME_STRICT_BACKCHANNEL", "false")
    .WithEnvironment("KC_HTTP_ENABLED", "true")
    .WithEnvironment("KC_HOSTNAME_STRICT", "false")
    .WithEnvironment("KC_HOSTNAME_STRICT_HTTPS", "false")
    .WithEnvironment("KC_HEALTH_ENABLED", "true")
    .WithEnvironment("KC_DB", "postgres")
    .WithEnvironment("KC_DB_URL", "jdbc:postgresql://postgres-db-keycloak:5432/keycloak_db")
    .WithEnvironment("KC_DB_USERNAME", postgresDbUser)
    .WithEnvironment("KC_DB_PASSWORD", postgresDbPassword)
    .WithArgs("start-dev")
    .WaitFor(postgresDb);

var keycloakEndpoint = keycloak.GetEndpoint("http");
#endregion

#region Kafka
var kafka = builder.AddKafka("kafka")
    .WithDataVolume("kafka_data")
    .WithKafkaUI();
#endregion

#region Catalog
var mongoUser =builder.AddParameter("MONGO-USERNAME");
var mongoPassword=builder.AddParameter("MONGO-PASSWORD");
var catalogmongoDB=builder.AddMongoDB("catalogmongoDB",27017,mongoUser,mongoPassword).WithImage("mongo:latest").WithDataVolume("mongodb_data").AddDatabase("CatalogDb");
var catalogApi=builder.AddProject<Projects.Microservice_Catalog_Api>("microservice-catalog-api");
catalogApi.WithReference(catalogmongoDB).WaitFor(catalogmongoDB);
catalogApi.WithReference(kafka).WaitFor(kafka).WithReference(keycloakEndpoint).WaitFor(keycloak);
#endregion

#region Basket
var redisPassword =builder.AddParameter("REDIS-PASSWORD");
var redisBasketDb=builder.AddRedis("redisBasketDb").WithImage("redis:latest").WithDataVolume("redis_data").WithPassword(redisPassword);
var basketApi=  builder.AddProject<Projects.Microservice_Basket_Api>("microservice-basket-api");
basketApi.WithReference(redisBasketDb).WithReference(kafka).WaitFor(kafka).WithReference(keycloakEndpoint).WaitFor(keycloak);
#endregion

#region Discount
var discountMongoDB = builder.AddMongoDB("discountMongoDB", 27018, mongoUser, mongoPassword).WithImage("mongo:latest").WithDataVolume("mongodb_discount_data").AddDatabase("DiscountDb");
var discountApi = builder.AddProject<Projects.Microservice_Discount_API>("microservice-discount-api");
discountApi.WithReference(discountMongoDB).WaitFor(discountMongoDB).WithReference(kafka).WaitFor(kafka).WithReference(keycloakEndpoint).WaitFor(keycloak);
#endregion

#region File
var fileApi = builder.AddProject<Projects.Microservice_File_Api>("microservice-file-api");
fileApi.WithReference(kafka).WaitFor(kafka).WithReference(keycloakEndpoint).WaitFor(keycloak);
#endregion

#region Payment
var paymentApi =builder.AddProject<Projects.Microservice_Payment_Api>("microservice-payment-api");
paymentApi.WithReference(kafka).WaitFor(kafka).WithReference(keycloakEndpoint).WaitFor(keycloak);
#endregion

#region Order
var sqlserverPassword =builder.AddParameter("SQLSERVER-PASSWORD");
var sqlserverOrderDb=builder.AddSqlServer("sqlserver-db-order").WithPassword(sqlserverPassword).WithDataVolume("sqlserver_order_data").AddDatabase("order-db-aspire");
 var orderApi = builder.AddProject<Projects.Microservice_Order_Api>("microservice-order-api");
orderApi.WithReference(sqlserverOrderDb).WaitFor(sqlserverOrderDb);
orderApi.WithReference(kafka).WaitFor(kafka).WithReference(keycloakEndpoint).WaitFor(keycloak);
#endregion

#region Web
var web =builder.AddProject<Projects.MicroserviceWebApp>("microservicewebapp");
web.WithReference(basketApi).WithReference(discountApi).WithReference(fileApi).WithReference(catalogApi).WithReference(paymentApi).WithReference(orderApi).WithReference(keycloakEndpoint).WaitFor(keycloak);
#endregion



builder.AddProject<Projects.microservice_Email>("microservice-email").WithReference(kafka).WaitFor(kafka);



builder.AddProject<Projects.Microservice_Gateway>("microservice-gateway").WithReference(keycloakEndpoint).WaitFor(keycloak);






builder.Build().Run();
