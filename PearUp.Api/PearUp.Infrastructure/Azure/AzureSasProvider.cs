using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace PearUp.Infrastructure.Azure
{
    public class AzureSasProvider : IAzureSasProvider
    {
        private const string policyName = "PearupReadWrite";
        private readonly string connectionString;
        private readonly string containerName;
        private readonly int expiryTimeInMinutes;
        private const string Expiry_Time_Is_Invalid = "Expire Time Is Invalid";
        private const string SasKey_Is_Invalid = "Sas Key Is Invalid";
        private readonly IContainerFactory _containerFactory;

        public AzureSasProvider(IOptions<AzureSettings> options, IContainerFactory containerFactory)
        {
            connectionString = options.Value.ConnectionString;
            containerName = options.Value.ContainerName;
            expiryTimeInMinutes = options.Value.ExpireTimeInMinutes;
            this._containerFactory = containerFactory;
        }

        private SharedAccessBlobPolicy CreatePolicy(BlobContainerPermissions permissions)
        {
            return new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List |
                SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create,
                SharedAccessStartTime = DateTime.UtcNow,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(expiryTimeInMinutes)
            };
        }

        private void AddSharedAccessPolicy(BlobContainerPermissions permissions, SharedAccessBlobPolicy sharedPolicy)
        {
            permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
        }

        private void DeletePolicy(SharedAccessBlobPolicy policy, BlobContainerPermissions permissions)
        {
            if (policy != null)
            {
                permissions.SharedAccessPolicies.Remove(policyName);
            }
        }

        private DateTimeOffset? GetExpiryTime(SharedAccessBlobPolicy policy)
        {
            return policy?.SharedAccessExpiryTime;
        }

        private SharedAccessBlobPolicy GetPolicy(BlobContainerPermissions permissions)
        {
            permissions.SharedAccessPolicies.TryGetValue(policyName, out SharedAccessBlobPolicy policy);
            return policy;
        }

        private bool IsExpired(SharedAccessBlobPolicy policy)
        {
            return policy.SharedAccessExpiryTime < DateTime.UtcNow;
        }

        private string GetSharedAccessSignature(CloudBlobContainer container, string policy)
        {
            return container.GetSharedAccessSignature(null, policy, SharedAccessProtocol.HttpsOnly, null);
        }

        private async Task CreatePolicyAndSetPersmission(BlobContainerPermissions permissions, CloudBlobContainer container)
        {
            var sharedPolicy = CreatePolicy(permissions);
            AddSharedAccessPolicy(permissions, sharedPolicy);
            await SetPermissionsToContainer(permissions, container);
        }

        public async Task<BlobContainerPermissions> GetContainerPermissions(CloudBlobContainer container)
        {
            return await container.GetPermissionsAsync();
        }

        public async Task SetPermissionsToContainer(BlobContainerPermissions permissions, CloudBlobContainer container)
        {
            await container.SetPermissionsAsync(permissions);
        }

        /// <summary>
        /// Will create policy on top of container, and return saskey
        /// </summary>
        /// <returns></returns>
        public async Task<AzureSasKey> GetContainerSAS()
        {
            var container = await _containerFactory.CreateContainerReference();
            var permissions = await GetContainerPermissions(container);
            var policy = GetPolicy(permissions);

            if (policy == null)
            {
                await CreatePolicyAndSetPersmission(permissions, container);
            }
            else if (IsExpired(policy))
            {
                DeletePolicy(policy, permissions);
                await CreatePolicyAndSetPersmission(permissions, container);
            }
            var expiryTime = GetExpiryTime(policy);

            if (!expiryTime.HasValue)
                throw new Exception(Expiry_Time_Is_Invalid);

            string token = GetSharedAccessSignature(container, policyName);
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception(SasKey_Is_Invalid);

            return new AzureSasKey
            {
                ExpiryTimeInUtc = expiryTime.Value,
                SasKey = token
            };
        }


    }
}
