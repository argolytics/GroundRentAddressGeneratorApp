using DataLibrary;
using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.HttpClients;
using MediatR;
using System.Net.Http.Headers;

namespace GroundRentAddressGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddScoped<IDataContext>(s => new DataContext(configuration.GetConnectionString("Default")));
        builder.Services.AddScoped<IAddressDataServiceFactory, AddressDataServiceFactory>();

        // Http clients
        builder.Services.AddScoped<GetUploadUri>();
        builder.Services.AddScoped<UploadPdf>();
        builder.Services.AddScoped<ExtractPdf>();
        builder.Services.AddScoped<GetDownloadStatus>();
        builder.Services.AddScoped<DownloadPdf>();
        builder.Services.AddHttpClient("getUploadUri", client =>
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", "eyJhbGciOiJSUzI1NiIsIng1dSI6Imltc19uYTEta2V5LWF0LTEuY2VyIiwia2lkIjoiaW1zX25hMS1rZXktYXQtMSIsIml0dCI6ImF0In0.eyJpZCI6IjE2NjgxMDA1Mzc2MDVfZjBlY2E4ZDUtMmJjZi00MmM3LWE2YzAtZmY0N2ZjNDM1NDYzX3VlMSIsInR5cGUiOiJhY2Nlc3NfdG9rZW4iLCJjbGllbnRfaWQiOiI1ZmE1NDRkOTRiN2Q0MzliODIwYzgyZjhjZWZkMWI4ZCIsInVzZXJfaWQiOiIzREY5MjEwQjYzNUJGREMxMEE0OTVGOTZAdGVjaGFjY3QuYWRvYmUuY29tIiwiYXMiOiJpbXMtbmExIiwiYWFfaWQiOiIzREY5MjEwQjYzNUJGREMxMEE0OTVGOTZAdGVjaGFjY3QuYWRvYmUuY29tIiwiY3RwIjowLCJmZyI6Ilc1M1NYQVZIRlBFNUlYVUtFTVFGWUhZQUxNPT09PT09IiwibW9pIjoiYzllNjM2NTUiLCJleHBpcmVzX2luIjoiODY0MDAwMDAiLCJzY29wZSI6Im9wZW5pZCxEQ0FQSSxBZG9iZUlELGFkZGl0aW9uYWxfaW5mby5vcHRpb25hbEFncmVlbWVudHMiLCJjcmVhdGVkX2F0IjoiMTY2ODEwMDUzNzYwNSJ9.M0a9oZMvYlmzPfwbOv2LR2aTXgHo-qZOwZYL1PmPwljXfdWtduL61U_HUR2vCy8mXeYcH1MR8TdXnPj9xCAxlubfQIhHcUNJxznkAKbt2dotuWfyfDFWHGsEHOlg8hj2vGnCKfqxandFoC7Ghe08ETSFgKCLIAEqTQNbY8YJRM-krhRyMU5fUw12pHGxGgy01jUunKO3lOwMXd8NHKRx5kM2EDA92m9whFKysSNNa8UM_9K36fVpZ91Q1883BaAvKUF-YjB4R0AmEnqBuPbMgSxnYX6eetJcqO8q6FbPJpC_dsYvRRYQunOfF2_mP_OOfpsfX0rQ0_2EK-CKAI__iw");
            client.DefaultRequestHeaders.Add("x-api-key", "5fa544d94b7d439b820c82f8cefd1b8d");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        builder.Services.AddHttpClient("uploadPdf", client =>
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", "eyJhbGciOiJSUzI1NiIsIng1dSI6Imltc19uYTEta2V5LWF0LTEuY2VyIiwia2lkIjoiaW1zX25hMS1rZXktYXQtMSIsIml0dCI6ImF0In0.eyJpZCI6IjE2NjgxMDA1Mzc2MDVfZjBlY2E4ZDUtMmJjZi00MmM3LWE2YzAtZmY0N2ZjNDM1NDYzX3VlMSIsInR5cGUiOiJhY2Nlc3NfdG9rZW4iLCJjbGllbnRfaWQiOiI1ZmE1NDRkOTRiN2Q0MzliODIwYzgyZjhjZWZkMWI4ZCIsInVzZXJfaWQiOiIzREY5MjEwQjYzNUJGREMxMEE0OTVGOTZAdGVjaGFjY3QuYWRvYmUuY29tIiwiYXMiOiJpbXMtbmExIiwiYWFfaWQiOiIzREY5MjEwQjYzNUJGREMxMEE0OTVGOTZAdGVjaGFjY3QuYWRvYmUuY29tIiwiY3RwIjowLCJmZyI6Ilc1M1NYQVZIRlBFNUlYVUtFTVFGWUhZQUxNPT09PT09IiwibW9pIjoiYzllNjM2NTUiLCJleHBpcmVzX2luIjoiODY0MDAwMDAiLCJzY29wZSI6Im9wZW5pZCxEQ0FQSSxBZG9iZUlELGFkZGl0aW9uYWxfaW5mby5vcHRpb25hbEFncmVlbWVudHMiLCJjcmVhdGVkX2F0IjoiMTY2ODEwMDUzNzYwNSJ9.M0a9oZMvYlmzPfwbOv2LR2aTXgHo-qZOwZYL1PmPwljXfdWtduL61U_HUR2vCy8mXeYcH1MR8TdXnPj9xCAxlubfQIhHcUNJxznkAKbt2dotuWfyfDFWHGsEHOlg8hj2vGnCKfqxandFoC7Ghe08ETSFgKCLIAEqTQNbY8YJRM-krhRyMU5fUw12pHGxGgy01jUunKO3lOwMXd8NHKRx5kM2EDA92m9whFKysSNNa8UM_9K36fVpZ91Q1883BaAvKUF-YjB4R0AmEnqBuPbMgSxnYX6eetJcqO8q6FbPJpC_dsYvRRYQunOfF2_mP_OOfpsfX0rQ0_2EK-CKAI__iw");
            client.DefaultRequestHeaders.Add("x-api-key", "5fa544d94b7d439b820c82f8cefd1b8d");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
        });
        builder.Services.AddHttpClient("extractPdf", client =>
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", "eyJhbGciOiJSUzI1NiIsIng1dSI6Imltc19uYTEta2V5LWF0LTEuY2VyIiwia2lkIjoiaW1zX25hMS1rZXktYXQtMSIsIml0dCI6ImF0In0.eyJpZCI6IjE2NjgxMDA1Mzc2MDVfZjBlY2E4ZDUtMmJjZi00MmM3LWE2YzAtZmY0N2ZjNDM1NDYzX3VlMSIsInR5cGUiOiJhY2Nlc3NfdG9rZW4iLCJjbGllbnRfaWQiOiI1ZmE1NDRkOTRiN2Q0MzliODIwYzgyZjhjZWZkMWI4ZCIsInVzZXJfaWQiOiIzREY5MjEwQjYzNUJGREMxMEE0OTVGOTZAdGVjaGFjY3QuYWRvYmUuY29tIiwiYXMiOiJpbXMtbmExIiwiYWFfaWQiOiIzREY5MjEwQjYzNUJGREMxMEE0OTVGOTZAdGVjaGFjY3QuYWRvYmUuY29tIiwiY3RwIjowLCJmZyI6Ilc1M1NYQVZIRlBFNUlYVUtFTVFGWUhZQUxNPT09PT09IiwibW9pIjoiYzllNjM2NTUiLCJleHBpcmVzX2luIjoiODY0MDAwMDAiLCJzY29wZSI6Im9wZW5pZCxEQ0FQSSxBZG9iZUlELGFkZGl0aW9uYWxfaW5mby5vcHRpb25hbEFncmVlbWVudHMiLCJjcmVhdGVkX2F0IjoiMTY2ODEwMDUzNzYwNSJ9.M0a9oZMvYlmzPfwbOv2LR2aTXgHo-qZOwZYL1PmPwljXfdWtduL61U_HUR2vCy8mXeYcH1MR8TdXnPj9xCAxlubfQIhHcUNJxznkAKbt2dotuWfyfDFWHGsEHOlg8hj2vGnCKfqxandFoC7Ghe08ETSFgKCLIAEqTQNbY8YJRM-krhRyMU5fUw12pHGxGgy01jUunKO3lOwMXd8NHKRx5kM2EDA92m9whFKysSNNa8UM_9K36fVpZ91Q1883BaAvKUF-YjB4R0AmEnqBuPbMgSxnYX6eetJcqO8q6FbPJpC_dsYvRRYQunOfF2_mP_OOfpsfX0rQ0_2EK-CKAI__iw");
            client.DefaultRequestHeaders.Add("x-api-key", "5fa544d94b7d439b820c82f8cefd1b8d");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        builder.Services.AddHttpClient("getDownloadStatus", client =>
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", "eyJhbGciOiJSUzI1NiIsIng1dSI6Imltc19uYTEta2V5LWF0LTEuY2VyIiwia2lkIjoiaW1zX25hMS1rZXktYXQtMSIsIml0dCI6ImF0In0.eyJpZCI6IjE2NjgxMDA1Mzc2MDVfZjBlY2E4ZDUtMmJjZi00MmM3LWE2YzAtZmY0N2ZjNDM1NDYzX3VlMSIsInR5cGUiOiJhY2Nlc3NfdG9rZW4iLCJjbGllbnRfaWQiOiI1ZmE1NDRkOTRiN2Q0MzliODIwYzgyZjhjZWZkMWI4ZCIsInVzZXJfaWQiOiIzREY5MjEwQjYzNUJGREMxMEE0OTVGOTZAdGVjaGFjY3QuYWRvYmUuY29tIiwiYXMiOiJpbXMtbmExIiwiYWFfaWQiOiIzREY5MjEwQjYzNUJGREMxMEE0OTVGOTZAdGVjaGFjY3QuYWRvYmUuY29tIiwiY3RwIjowLCJmZyI6Ilc1M1NYQVZIRlBFNUlYVUtFTVFGWUhZQUxNPT09PT09IiwibW9pIjoiYzllNjM2NTUiLCJleHBpcmVzX2luIjoiODY0MDAwMDAiLCJzY29wZSI6Im9wZW5pZCxEQ0FQSSxBZG9iZUlELGFkZGl0aW9uYWxfaW5mby5vcHRpb25hbEFncmVlbWVudHMiLCJjcmVhdGVkX2F0IjoiMTY2ODEwMDUzNzYwNSJ9.M0a9oZMvYlmzPfwbOv2LR2aTXgHo-qZOwZYL1PmPwljXfdWtduL61U_HUR2vCy8mXeYcH1MR8TdXnPj9xCAxlubfQIhHcUNJxznkAKbt2dotuWfyfDFWHGsEHOlg8hj2vGnCKfqxandFoC7Ghe08ETSFgKCLIAEqTQNbY8YJRM-krhRyMU5fUw12pHGxGgy01jUunKO3lOwMXd8NHKRx5kM2EDA92m9whFKysSNNa8UM_9K36fVpZ91Q1883BaAvKUF-YjB4R0AmEnqBuPbMgSxnYX6eetJcqO8q6FbPJpC_dsYvRRYQunOfF2_mP_OOfpsfX0rQ0_2EK-CKAI__iw");
            client.DefaultRequestHeaders.Add("x-api-key", "5fa544d94b7d439b820c82f8cefd1b8d");
        });
        builder.Services.AddHttpClient("downloadPdf", client =>
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", "eyJhbGciOiJSUzI1NiIsIng1dSI6Imltc19uYTEta2V5LWF0LTEuY2VyIiwia2lkIjoiaW1zX25hMS1rZXktYXQtMSIsIml0dCI6ImF0In0.eyJpZCI6IjE2NjgxMDA1Mzc2MDVfZjBlY2E4ZDUtMmJjZi00MmM3LWE2YzAtZmY0N2ZjNDM1NDYzX3VlMSIsInR5cGUiOiJhY2Nlc3NfdG9rZW4iLCJjbGllbnRfaWQiOiI1ZmE1NDRkOTRiN2Q0MzliODIwYzgyZjhjZWZkMWI4ZCIsInVzZXJfaWQiOiIzREY5MjEwQjYzNUJGREMxMEE0OTVGOTZAdGVjaGFjY3QuYWRvYmUuY29tIiwiYXMiOiJpbXMtbmExIiwiYWFfaWQiOiIzREY5MjEwQjYzNUJGREMxMEE0OTVGOTZAdGVjaGFjY3QuYWRvYmUuY29tIiwiY3RwIjowLCJmZyI6Ilc1M1NYQVZIRlBFNUlYVUtFTVFGWUhZQUxNPT09PT09IiwibW9pIjoiYzllNjM2NTUiLCJleHBpcmVzX2luIjoiODY0MDAwMDAiLCJzY29wZSI6Im9wZW5pZCxEQ0FQSSxBZG9iZUlELGFkZGl0aW9uYWxfaW5mby5vcHRpb25hbEFncmVlbWVudHMiLCJjcmVhdGVkX2F0IjoiMTY2ODEwMDUzNzYwNSJ9.M0a9oZMvYlmzPfwbOv2LR2aTXgHo-qZOwZYL1PmPwljXfdWtduL61U_HUR2vCy8mXeYcH1MR8TdXnPj9xCAxlubfQIhHcUNJxznkAKbt2dotuWfyfDFWHGsEHOlg8hj2vGnCKfqxandFoC7Ghe08ETSFgKCLIAEqTQNbY8YJRM-krhRyMU5fUw12pHGxGgy01jUunKO3lOwMXd8NHKRx5kM2EDA92m9whFKysSNNa8UM_9K36fVpZ91Q1883BaAvKUF-YjB4R0AmEnqBuPbMgSxnYX6eetJcqO8q6FbPJpC_dsYvRRYQunOfF2_mP_OOfpsfX0rQ0_2EK-CKAI__iw");
            client.DefaultRequestHeaders.Add("x-api-key", "5fa544d94b7d439b820c82f8cefd1b8d");
        });
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