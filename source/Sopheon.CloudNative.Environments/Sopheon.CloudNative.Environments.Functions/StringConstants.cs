namespace Sopheon.CloudNative.Environments.Functions
{
   public static class StringConstants
   {
      #region OpenAPI

      public const string CONTENT_TYPE_APP_JSON = "application/json";
      public const string CONTENT_TYPE_TEXT_PLAIN = "text/plain";

      public const string RESPONSE_SUMMARY_200 = "200 OK response";
      public const string RESPONSE_SUMMARY_201 = "201 Created response";
      public const string RESPONSE_SUMMARY_204 = "204 No Content response";
      public const string RESPONSE_SUMMARY_400 = "400 Bad Request response";
      public const string RESPONSE_SUMMARY_404 = "404 Not Found response";
      public const string RESPONSE_SUMMARY_500 = "500 Internal Server Error response";

      public const string RESPONSE_DESCRIPTION_200 = "200 OK response, with Dto in response body";
      public const string RESPONSE_DESCRIPTION_201 = "201 Created response, with Dto in response body";
      public const string RESPONSE_DESCRIPTION_204 = "204 No Content response, with no response body";
      public const string RESPONSE_DESCRIPTION_400 = "400 Bad Request response, with error message in response body";
      public const string RESPONSE_DESCRIPTION_404 = "404 Not Found response, with error message in response body";
      public const string RESPONSE_DESCRIPTION_500 = "500 Internal Server Error response, with error message in response body";

      #endregion // OpenAPI

      public const string RESPONSE_REQUEST_BODY_INVALID = "Request body was invalid.";
      public const string RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID = "The EnvironmentKey must be a valid Guid";
      public const string RESPONSE_GENERIC_ERROR = "Something went wrong. Please try again later.";
      public const string RESPONSE_REQUEST_PATH_PARAMETER_INVALID = "A required path parameter was invalid or missing. Check the request path/route.";
      public const string BLOB_FILE_NOT_FOUND = "No file was found in blob storage at path: ";
      public const string ELASTICPOOL_DATABASE_BUFFER_BLOB_PATH = "armtemplates/ElasticPoolWithBuffer/ElasticPool_Database_Buffer.json";

      // buffered database indicator using Azure Tags
      public const string CUSTOMER_PROVISIONED_DATABASE_TAG_NAME = "CustomerProvisionedDatabase"; // Tag Name/key for Azure Resouce (Azure SQL database)
      public const string CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_INITIAL = "NotAssigned"; // databases with this Tag Value are part of buffer
      public const string CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_ASSIGNED = "Assigned";

      // replacement tokens
      public static readonly string SERVER_NAME_TOKEN = "^SqlServerName^";
      public static readonly string ADMINISTRATOR_LOGIN_ENIGMA_TOKEN = "^SqlAdminEngima^";

      public static readonly string HTTP_CLIENT_NAME_AZURE_REST_API = "AzureRestApiHttpClient";
      public static readonly string HTTP_CLIENT_NAME_ENVIRONMENT_FUNCTIONS = "EnvironmentFunctionsHttpClient"; // used to call Sopheon.CloudNative.Environments.Functions HttpTrigger endpoints
   }
}
