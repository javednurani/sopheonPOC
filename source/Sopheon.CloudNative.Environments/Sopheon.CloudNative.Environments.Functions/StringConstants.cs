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
   }
}
