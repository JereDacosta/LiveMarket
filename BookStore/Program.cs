using BookStore.Services;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<BookService>();

// Add OpenTelemetry
var otelSettings = builder.Configuration.GetSection("OpenTelemetry");

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: otelSettings["Service:Name"] ?? "unknown",
            serviceVersion: otelSettings["Service:Version"] ?? "unknown",
            serviceInstanceId: Environment.MachineName))
    .WithLogging(loggingBuilder =>
    {
        loggingBuilder.AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otelSettings["Exporter:Endpoint"] ?? "http://otel-collector:4317");
            options.Protocol = OtlpExportProtocol.Grpc;
        });
    })
    .WithTracing(tracerBuilder =>
    {
        tracerBuilder
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(otelSettings["Exporter:Endpoint"] ?? "http://otel-collector:4317");
                options.Protocol = OtlpExportProtocol.Grpc;
            });
    })
    .WithMetrics(metricBuilder =>
    {
        metricBuilder
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation() // For memory, CPU, GC, etc.
            .AddAspNetCoreInstrumentation() // For request metrics like latency
            .AddHttpClientInstrumentation() // For HttpClient request metrics
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(otelSettings["Exporter:Endpoint"] ?? "http://otel-collector:4317");
                options.Protocol = OtlpExportProtocol.Grpc;
            });
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Store V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
