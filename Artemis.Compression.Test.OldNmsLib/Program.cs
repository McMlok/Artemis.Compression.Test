using Artemis.Compression.Test.NewNmsLib;
using Artemis.Compression.Test.OldNmsLib;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
  x.AddConsumer<SampleConsumer>();
  x.UsingActiveMq((context, cfg) =>
  {
    cfg.EnableArtemisCompatibility();
    cfg.Host("localhost", c =>
    {
      c.TransportOptions(new Dictionary<string, string> { { "connection.useCompression", "true" } });
    });
    cfg.ReceiveEndpoint("compression-test-to-old", e =>
    {
      e.ConfigureConsumer<SampleConsumer>(context);
    });
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.


app.MapGet("/send", async (ISendEndpointProvider provider) =>
{
  var endpoint = await provider.GetSendEndpoint(new Uri($"queue:compression-test-to-new"));
  await endpoint.Send(new SampleCommand { Name = "Test name" });
  return "Message Sent";
});

app.Run();