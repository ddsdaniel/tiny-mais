﻿using Infra.HttpClients.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;
using TinyMais.Domain.Abstractions.Models;

namespace TrackCash.Infra.HttpClients.Abstractions.HttpClients
{
    public abstract class TrachCashHttpClient : InfraHttpClient
    {
        private readonly IAppSettings _appSettings;
        protected const string URL_BASE = "https://sistema.trackcash.com.br/api";

        protected TrachCashHttpClient(
            HttpClient httpClient,
            ILogger<TrachCashHttpClient> logger,
            IAppSettings appSettings
            ) : base(httpClient, logger)
        {
            _appSettings = appSettings;
        }

        protected override void Autenticar()
        {
            var usuarioSenha = $"{_appSettings.TrackCash.Credencial.Usuario}:{_appSettings.TrackCash.Credencial.Senha}";

            var credenciaisBytes = Encoding.ASCII.GetBytes(usuarioSenha);

            var credenciaisBase64 = Convert.ToBase64String(credenciaisBytes);

            _httpClient.DefaultRequestHeaders.Add("token", credenciaisBase64);
        }
    }
}
