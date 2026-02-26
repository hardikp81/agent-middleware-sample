using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using Newtonsoft.Json;

Console.WriteLine("Press enter once the MCP server is started.");

var mcpClient = await McpClient.CreateAsync(new HttpClientTransport(new HttpClientTransportOptions() { Endpoint = new Uri("http://localhost:5117") }));

var tools = await mcpClient.ListToolsAsync();


var token = new DefaultAzureCredential();
var openAIClient = new AzureOpenAIClient(
        new Uri("https://staging-als-openai.openai.azure.com/"),
        token)
    .GetChatClient("gpt-4.1");


async ValueTask<object?> CustomFunctionCallingMiddleware(
    AIAgent agent,
    FunctionInvocationContext context,
    Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next,
    CancellationToken cancellationToken)
{
    Console.WriteLine();
    Console.WriteLine($"Function Name: {context!.Function.Name}");

    context.Arguments["correlationId"] = Guid.NewGuid().ToString();
    Console.WriteLine($"Function Arguments: {JsonConvert.SerializeObject(context!.Arguments)}");
    Console.WriteLine();

    var result = await next(context, cancellationToken);
    Console.WriteLine($"Function Call Result: {result}");
    Console.WriteLine();

    return result;
}


using IChatClient chatClient = openAIClient
    .AsIChatClient()
    .AsBuilder()
    .UseFunctionInvocation()
    .Build();
//.Use(CustomFunctionCallingMiddleware).Build();



var agent = chatClient
    .AsAIAgent(instructions: "You are a helpful bank assistant.", tools: [.. tools])
    .AsBuilder()
    .Use(CustomFunctionCallingMiddleware)
    .Build();


List<ChatMessage> messages = [];
messages.Add(new(ChatRole.System, "You are a helpful bank assistant."));
while (true)
{
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();

    Console.Write("Enter your questions: ");
    messages.Add(new(ChatRole.User, Console.ReadLine()));
    Console.WriteLine();

    var agentResponse = await agent.RunAsync(messages);
    foreach (var message in agentResponse.Messages)
    {
        Console.Write(message.Text);
        messages.Add(message);
    }
    
}