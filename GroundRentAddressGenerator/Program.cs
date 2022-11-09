using DataLibrary;
using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using MediatR;

namespace GroundRentAddressGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        //builder.Services.AddMediatR(typeof(MediatREntryPoint).Assembly);
        builder.Services.AddScoped<IDataContext>(s => new DataContext(configuration.GetConnectionString("Default")));
        builder.Services.AddScoped<IAddressDataServiceFactory, AddressDataServiceFactory>();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}