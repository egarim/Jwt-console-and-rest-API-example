using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApiDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Latest);
       

            //HACK Xpo asp core extensions https://www.devexpress.com/Support/Center/Question/Details/T637597/asp-net-core-dependency-injection-in-xpo

            //HACK https://documentation.devexpress.com/XPO/119377/Getting-Started/Getting-Started-with-NET-Core
            //string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //string connectionString = SQLiteConnectionProvider.GetConnectionString(Path.Combine(appDataPath, "myXpoApp.db"));
            //string connectionString = SQLiteConnectionProvider.GetConnectionString("myXpoApp.db");
            //IDataStore DataStore = SQLiteConnectionProvider.CreateProviderFromConnection(SQLiteConnectionProvider.CreateConnection(@"Data Source=mydb.db"), AutoCreateOption.DatabaseAndSchema);


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "weedware101.database.windows.net,1433";
            builder.UserID = "demodbjose";
            builder.Password = "*.mk3247";
            builder.InitialCatalog = "demodbjose";

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            Dictionary<string, IDataStore> RestDataStores = new Dictionary<string, IDataStore>();
            
            var SqlServerdataStore = DevExpress.Xpo.DB.MSSqlConnectionProvider.CreateProviderFromConnection(connection, AutoCreateOption.SchemaOnly);

            RestDataStores.Add("demodbjose", SqlServerdataStore);


            //services.AddSingleton<IDataStore>(SqlServerdataStore);



            services.AddSingleton<Dictionary<string, IDataStore>>(RestDataStores);

            //var SqlServerdataStore = DevExpress.Xpo.DB.MSSqlConnectionProvider.CreateProviderFromString("Initial Catalog=demodbjose; Password=*.mk3247; User ID=demodbjose; Data Source=weedware101.database.windows.net,1433; Persist Security Info=True;", AutoCreateOption.SchemaAlreadyExists, out objects);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}