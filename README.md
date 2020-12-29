# Among-Us-Tracker-Create-Session Function
An Azure serverless function that contains "business" logic for 'Creating a session'.

- APIM URL: https://kp-global-apim.azure-api.net/au-create-session
- Create Session URL: https://kp-global-apim.azure-api.net/au-create-session/CreateSession

## Aspirational Improvements
- Automatically import this function app in to APIM
    - I am blocked on this due to the following: https://github.com/terraform-providers/terraform-provider-azurerm/issues/4938;
    - But did this instead, as it automatically configures APIM to pass the Function App Key to the backend function: https://docs.microsoft.com/en-us/azure/api-management/import-function-app-as-api

- Key Vault and get Connection String for CosmosDB from Key Vault.
- CI/CD
- Unit Tests: I've opted to ignore tests as these are my own experimental solutions and a bit of weekend fun, but I am an advocate of unit testing and any code I write for others would absolutely contain tests.

## Cosmos DB (Table API)
This function utilises an Azure CosmosDb using Table API to reduce monthly costs and persist session data.

### Key Vault
In a production application, you'd use Key Vault to pluck the connection string for the CosmosDB when the Function starts up, but due to cost saving and time constraints I've opted against utilising it here.
It's quite simple for me to deploy the function and then just paste in the connection string in to the configuration for the function in Azure. Lazy, but quick. In a production application, you'd actively avoid
exposing this connection string and ensure that it's not stored anywhere apart from the vault itself.

In a one-to-one relationship, you could opt to capture the CosmosDB connection string from the Terraform create script output, then send the connection string in to the function creation terraform script as a variable.
However as I've got a many-to-one scenario here, lots of functions and one database, it's better it comes from keyvault and the CosmosDB becomes it's own independent deployment.

## Terraform & CI/CD
I used Terraform to create my infrastructure in Azure. As I do not have a CI/CD setup for this repository, I've opted to run this locally and link the created Function App in Azure to this GitHub repository using the 'Deployment Center' manually. Here Azure will automatically monitor my GitHub repository 'main' branch for changes and copy them to the Function App for me. When I make infrastructure changes, I'll have to manually run Terraform to get these to apply.

In an ideal world I'd setup a GitHub Workflow and:
- Get that to build my code;
- Create my infrastructure using my Terraform script;
- Copy my built Function code to my new Azure Function App.

Truthfully, the documentation for Terraform turned out to be a bit lacklustre. Many examples of deploying C# Functions using Terraform are done using the .zip deployment method and written by different people with differing opinions. The Terraform documentation itself is also quite difficult to read and understand.

Now if I could automate the hooking up of the 'Deployment Centre' in Azure using Terraform... that would be interesting.
