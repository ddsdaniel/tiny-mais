﻿using Infra.HttpClients.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Tiny.Infra.HttpClients.Abstractions.HttpClients
{
    public abstract class TinyHttpClient : RestHttpClient
    {
        protected const string URL_BASE = "https://api.tiny.com.br/api2";

        protected TinyHttpClient(
            HttpClient httpClient,
            ILogger<TinyHttpClient> logger
            ) : base(httpClient, logger)
        {
        }

        protected override async Task<TViewModel> ObterRetorno<TViewModel>(HttpResponseMessage response)
            where TViewModel : class
        {
            if (Invalido) return null;

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var viewModel = JsonSerializer.Deserialize<TViewModel>(json);

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Falha ao obter retorno do Tiny: {ex.Message}. {json}");
                throw;
            }

        }
    }
}
