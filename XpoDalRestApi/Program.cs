using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Linq;
using Xpo.RestDataStore;

namespace Xpo.RestDataStoreClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadKey();
            XPDictionary dictionary = new ReflectionDictionary();
            dictionary.GetDataStoreSchema(typeof(Program).Assembly);

            ThreadSafeDataLayer UpdateSchemaDal = new ThreadSafeDataLayer(dictionary, new RestApiDataStore("http://localhost:55829/api/DataStore/", DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema));

            UnitOfWork UpdateSchemaUoW = new UnitOfWork(UpdateSchemaDal);

            UpdateSchemaUoW.UpdateSchema();
            UpdateSchemaUoW.CreateObjectTypeRecords();

            SimpleDataLayer Dal = new SimpleDataLayer(dictionary, new RestApiDataStore("http://localhost:55829/api/DataStore/", DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema));

            XpoDefault.ObjectLayer = new RestObjectLayer(Dal, "Jose");
            UnitOfWork UoW = new UnitOfWork();
            //UnitOfWork UoW = new UnitOfWork(Dal);

            UoW.UpdateSchema();
            UoW.CreateObjectTypeRecords();

            if (!UoW.Query<Product>().Any())
            {
                Category BestFoodInTheWorld = new Category(UoW) { Code = "001", Name = "Best food in the world" };

                Category HealtyFood = new Category(UoW) { Code = "002", Name = "Healty Food" };

                Product Hamburger = new Product(UoW);
                Hamburger.Name = "Rocco's hamburger";
                Hamburger.Description = "is a cheeseburger with cheese inside the meat instead of on top, resulting in a melted core of cheese.";
                Hamburger.Code = "001";
                Hamburger.Category = BestFoodInTheWorld;
                Product Pizza = new Product(UoW);
                Pizza.Name = "Pizza";
                Pizza.Description = "Pizza Margherita is a typical Neapolitan pizza, made with San Marzano tomatoes, mozzarella fior di latte, fresh basil, salt and extra-virgin olive oil";
                Pizza.Code = "002";
                Pizza.Category = BestFoodInTheWorld;
                Product Tacos = new Product(UoW);
                Tacos.Name = "Tacos";
                Tacos.Description = "Carne Asada Tacos. Carne asada tacos are delicious, flank steak tacos with a few simple ingredients and tons of flavor";
                Tacos.Code = "003";
                Tacos.Category = BestFoodInTheWorld;

                Product Salad = new Product(UoW) { Name = "Salad", Description = "Just a salad", Code = "004", Category = HealtyFood };
            }
            UoW.CommitChanges();
            Console.ReadKey();
        }
    }
}