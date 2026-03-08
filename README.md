# agent-middleware-sample
Sample application to demo how Microsoft Agent framework passes the context to the MCP tools using middleware.

# The Setup: Two Small Projects Working Together

## The sample solution contains two projects:

### 1. MCP.WebApi
This is a simple MCP server that exposes one tool:

GetAccountTransactionById(accountId, correlationId)
The tool requires both parameters. But in a real chat scenario, you don’t want the user to type:

“Get the transaction for account 12345 with correlation ID 8f2c….”

Instead of requiring the user to provide the Correlation ID, the system should automatically forward that value to the MCP server.

### 2. Agent.Middleware.Demo (Console Chat App)
This is a lightweight console app that acts as the “agent client.”
The user interacts with an OpenAI‑powered agent, and the agent is configured to call the MCP tool when needed.

Here’s the important part:
The user only provides the account ID.
The system automatically injects the correlation ID using middleware.

You can read the full arcticle [here](https://medium.com/@hardikwrites/how-microsofts-agent-framework-passes-context-to-mcp-tools-using-middleware-78cfc2c8cc0f).

