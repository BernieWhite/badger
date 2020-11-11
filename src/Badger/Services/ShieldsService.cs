// Copyright (c) Bernie White.
// Licensed under the MIT License.

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Badger.Models;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Badger.Services
{
    public interface IBadgeService
    {
        Task<Stream> GetAsync(string service, string id);

        Task CreateAsync(string service, string id, BadgeStatus status, string label);
    }

    internal sealed class ShieldsService : IBadgeService
    {
        private const string ENDPOINT_URL = "https://img.shields.io/badge/";

        private readonly HttpClient _ShieldsClient;
        private readonly BlobContainerClient _StorageClient;
        private readonly string _Shard;

        public ShieldsService(BlobServiceClient blobServiceClient)
        {
            _ShieldsClient = new HttpClient();
            _StorageClient = blobServiceClient.GetBlobContainerClient("badges");
            _StorageClient.CreateIfNotExists();
            _Shard = "0";
        }

        async Task<Stream> IBadgeService.GetAsync(string service, string id)
        {
            var blob = GetBlob(GetBlobName(_Shard, service, id));
            if (!await blob.ExistsAsync())
                return null;

            return await GetBlob(GetBlobName(_Shard, service, id)).OpenReadAsync();
        }

        async Task IBadgeService.CreateAsync(string service, string id, BadgeStatus status, string label)
        {
            var badgeContent = await CreateBadge(service, status, label);
            await GetBlob(GetBlobName(_Shard, service, id)).UploadAsync(badgeContent, overwrite: true);
        }

        private async Task<Stream> CreateBadge(string service, BadgeStatus status, string label)
        {
            var color = "lightgrey";
            switch (status)
            {
                case BadgeStatus.Success:
                    color = "brightgreen";
                    break;

                case BadgeStatus.Pending:
                    color = "yellow";
                    break;

                case BadgeStatus.Error:
                    color = "red";
                    break;
            }

            var response = await _ShieldsClient.GetAsync(GetBadgeUrl(ENDPOINT_URL, service, label, color));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        private BlobClient GetBlob(string blobName)
        {
            return _StorageClient.GetBlobClient(blobName);
        }

        private static string GetBlobName(string shard, string service, string id)
        {
            var normalService = service.Replace('/', '_');
            var normalId = id.Replace('/', '_');

            return string.Concat("badges/", shard, "/s/", normalService, "/", normalId);
        }

        private static string GetBadgeUrl(string endpoint, string service, string label, string color)
        {
            return string.Concat(endpoint, service, "-", label, "-", color);
        }
    }
}
