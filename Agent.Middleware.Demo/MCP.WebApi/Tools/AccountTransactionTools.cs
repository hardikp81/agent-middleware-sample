using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCP.WebApi.Tools
{
    public record AccountTransaction(
        string Id,
        decimal Amount,
        string Currency,
        string Description,
        DateTime Date
    );

    [McpServerToolType]
    public class AccountTransactionTools
    {

        private readonly ILogger<AccountTransactionTools> _logger;

        public AccountTransactionTools(ILogger<AccountTransactionTools> logger)
        {
            _logger = logger;
        }

        [McpServerTool(Name = "GetAccountTransactionById", Title = "Get Account transaction by Id")]
        [McpMeta("type", "retrieve")]
        public async Task<AccountTransaction> GetAccountTransactionByIdAsync(
            [Description("Account Id")]string id,
            [Description("Correlation Id")] string correlationId = "")
        {
            _logger.LogInformation("Retrieving account transaction with Id: {Id}, CorrelationId: {CorrelationId}", id, correlationId);

            // Implementation to retrieve an account transaction by its ID
            // This is a placeholder implementation and should be replaced with actual logic to fetch data from a database or service
            return await Task.FromResult(new AccountTransaction(
                id,
                100.00m,
                "USD",
                "Sample Transaction",
                DateTime.UtcNow
            ));
        }

    }
}
