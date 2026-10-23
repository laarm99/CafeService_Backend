namespace NewProject.Application.Common.Email
{
    public class EmailMessage
    {
        public List<string> To { get; set; } = new();
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    public interface IEmailService
    {
        Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
    }

    // Implementacion temporal: solo registra en el log. Se reemplazara por SMTP/Graph.
    public class LoggingEmailService : IEmailService
    {
        private readonly Microsoft.Extensions.Logging.ILogger<LoggingEmailService> _logger;

        public LoggingEmailService(Microsoft.Extensions.Logging.ILogger<LoggingEmailService> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(
                _logger,
                "Correo (simulado) a {Recipients}. Asunto: {Subject}",
                string.Join(", ", message.To),
                message.Subject);
            return Task.CompletedTask;
        }
    }
}
