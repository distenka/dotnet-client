﻿namespace dotnet_client
{
    public class ApiKeyProvider : IAuthenticationProvider
    {
        readonly string apiKey;

        public ApiKeyProvider(string apiKey)
        {
            if (String.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            this.apiKey = apiKey;
        }

        public Task<string> AcquireToken()
        {
            return Task.FromResult(apiKey);
        }
    }
}
