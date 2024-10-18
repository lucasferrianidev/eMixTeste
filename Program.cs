using Cep.Data;
using EMixApi.Validators;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.EntityFrameworkCore;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<CepValidator>();
//builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();
builder.Services.AddDbContext<CepDbContext>(opts => opts
    .UseSqlServer(configuration.GetConnectionString("CepConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PolicyCors",
         builder =>
         {
             builder.WithOrigins("http://localhost:4200")
             .AllowAnyHeader()
             .AllowAnyMethod();
         });
});


var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true");
        using (conn)
        {
            conn.Open();

            Server server = new Server(new ServerConnection(conn));

            // Verifica se a base de dados existe
            bool databaseExists = false;
            foreach (Database db in server.Databases)
            {
                if (db.Name.Equals("CEP", StringComparison.OrdinalIgnoreCase))
                {
                    databaseExists = true;
                    break;
                }
            }

            //database nao existe
            if (!databaseExists)
            {
                if (!Directory.Exists("C:\\emix"))
                {
                    Directory.CreateDirectory("C:\\emix");
                }

                string queryCreateDataBase;
                SqlConnection myConn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true");

                //queryCreateDataBase = "DROP DATABASE CEP";
                queryCreateDataBase = "CREATE DATABASE CEP ON PRIMARY " +
                    "(NAME = cep_data, " +
                    "FILENAME = 'C:\\emix\\cep.mdf', " +
                    "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)" +
                    "LOG ON (NAME = cep_Log, " +
                    "FILENAME = 'C:\\emix\\cep.ldf', " +
                    "SIZE = 1MB, " +
                    "MAXSIZE = 5MB, " +
                    "FILEGROWTH = 10%)";

                SqlCommand cmdDatabase = new SqlCommand(queryCreateDataBase, myConn);
                try
                {
                    myConn.Open();
                    cmdDatabase.ExecuteNonQuery();

                    string queryCreateTable = @"            
                        USE [CEP]
                        SET ANSI_NULLS ON
                        SET QUOTED_IDENTIFIER ON

                        CREATE TABLE [dbo].[CEP] (
                            [Id]          INT            IDENTITY (1, 1) NOT NULL,
                            [cep]         CHAR (9)       NULL,
                            [logradouro]  NVARCHAR (500) NULL,
                            [complemento] NVARCHAR (500) NULL,
                            [bairro]      NVARCHAR (500) NULL,
                            [localidade]  NVARCHAR (500) NULL,
                            [uf]          CHAR (2)       NULL,
                            [unidade]     BIGINT         NULL,
                            [ibge]        INT            NULL,
                            [gia]         NVARCHAR (500) NULL
                        );";

                    SqlCommand cmdTable = new SqlCommand(queryCreateTable, myConn);
                    cmdTable.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    //TODO: mensagem de texto
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
        }
    }
    catch (Exception ex) { }

    // Call the next delegate/middleware in the pipeline.
    await next(context);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PolicyCors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
