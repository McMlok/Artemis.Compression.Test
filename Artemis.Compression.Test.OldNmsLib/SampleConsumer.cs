using Artemis.Compression.Test.NewNmsLib;
using MassTransit;

namespace Artemis.Compression.Test.OldNmsLib
{
  internal class SampleConsumer : IConsumer<SampleCommand>
  {
    private readonly ILogger<SampleConsumer> logger;

    public SampleConsumer(ILogger<SampleConsumer> logger)
    {
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task Consume(ConsumeContext<SampleCommand> context)
    {
      logger.LogInformation($"Message consumed {context.Message.Name}");
      return Task.CompletedTask;
    }
  }
}