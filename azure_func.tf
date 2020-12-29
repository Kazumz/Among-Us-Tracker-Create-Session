provider "azurerm" {
  version = "=2.35.0"
  features {}
}

resource "azurerm_resource_group" "au-create-session" {
  name     = "au-create-session-func-rg"
  location = "westus2"
}

resource "random_id" "server" {
  keepers = {
    # Generate a new id each time we switch to a new Azure Resource Group
    rg_id = azurerm_resource_group.au-create-session.name
  }

  byte_length = 8
}

resource "azurerm_storage_account" "au-create-session" {
  name                     = "aucreatesessionstorage"
  resource_group_name      = azurerm_resource_group.au-create-session.name
  location                 = azurerm_resource_group.au-create-session.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "au-create-session" {
  name                = "au-create-asp"
  location            = azurerm_resource_group.au-create-session.location
  resource_group_name = azurerm_resource_group.au-create-session.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_application_insights" "au-create-session" {
  name                = "au-create-session-insights"
  location            = azurerm_resource_group.au-create-session.location
  resource_group_name = azurerm_resource_group.au-create-session.name
  application_type    = "web"
}

resource "azurerm_function_app" "au-create-session" {
  name                      = "au-create-session-func"
  location                  = azurerm_resource_group.au-create-session.location
  resource_group_name       = azurerm_resource_group.au-create-session.name
  app_service_plan_id       = azurerm_app_service_plan.au-create-session.id
  storage_connection_string = azurerm_storage_account.au-create-session.primary_connection_string
  version                   = "~3"
  
  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.au-create-session.instrumentation_key,
	FUNCTIONS_EXTENSION_VERSION: "~3",
	FUNCTIONS_EXTENSION_RUNTIME: "dotnet"
  }
  
  source_control {
	branch             = "master"
	manual_integration = true
	repo_url           = "https://github.com/Kazumz/Among-Us-Tracker-Create-Session"
	rollback_enabled   = false
	use_mercurial      = false
  }
}