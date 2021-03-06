﻿using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Linq;
using Xpo.RestDataStore;

namespace Xpo.RestDataStoreClient
{
    internal class Program
    {
        public const string RestApiDataStoreUrl = "http://10.0.0.29/WebApiDemo/api/DataStore";

        public const string RestApiLoginUrl = "http://10.0.0.29/WebApiDemo/api/Login";
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadKey();

             
            
            
            RestApiDataStoreClient Client = new RestApiDataStoreClient(RestApiLoginUrl, RestApiDataStoreUrl, typeof(Program).Assembly);
         
            Client.Login("Joche", "123","","");
           // Client.UpdateSchema();
            //UnitOfWork UoW = Client.CreateNewUnitOfWork();
            ////UnitOfWork UoW = new UnitOfWork(Dal);

            //if (!UoW.Query<Product>().Any())
            //{
            //    Category BestFoodInTheWorld = new Category(UoW) { Code = "001", Name = "Best food in the world" };

            //    Category HealtyFood = new Category(UoW) { Code = "002", Name = "Healty Food" };

            //    Product Hamburger = new Product(UoW);
            //    Hamburger.Name = "Rocco's hamburger";
            //    Hamburger.Description = "is a cheeseburger with cheese inside the meat instead of on top, resulting in a melted core of cheese.";
            //    Hamburger.Code = "001";
            //    Hamburger.Category = BestFoodInTheWorld;
            //    Product Pizza = new Product(UoW);
            //    Pizza.Name = "Pizza";
            //    Pizza.Description = "Pizza Margherita is a typical Neapolitan pizza, made with San Marzano tomatoes, mozzarella fior di latte, fresh basil, salt and extra-virgin olive oil";
            //    Pizza.Code = "002";
            //    Pizza.Category = BestFoodInTheWorld;
            //    Product Tacos = new Product(UoW);
            //    Tacos.Name = "Tacos";
            //    Tacos.Description = "Carne Asada Tacos. Carne asada tacos are delicious, flank steak tacos with a few simple ingredients and tons of flavor";
            //    Tacos.Code = "003";
            //    Tacos.Category = BestFoodInTheWorld;

            //    Product Salad = new Product(UoW) { Name = "Salad", Description = "Just a salad", Code = "004", Category = HealtyFood };
            //}
            //UoW.CommitChanges();
            Console.ReadKey();
        }
    }
}