# MongoDB Catalog veritabanını temizle
docker exec -it catalog-mongodb mongosh -u myuser -p mypassword --authenticationDatabase admin --eval "
  use CatalogDb;
  db.Courses.deleteMany({});
  db.Categories.deleteMany({});
  print('MongoDB temizlendi!');
"
