using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using PearUp.Constants;
using PearUp.IBusiness;
using PearUp.ILoggingFramework;
using System;
using System.Collections.Generic;
using System.IO;

namespace PearUp.Azure
{
    public class ProfilePhoto
    {
        [FunctionName("ProfilePhoto")]
        public static async void Run([BlobTrigger("profileimages/{name}", Connection = "StorageConnection")]Stream myBlob, string name, TraceWriter log, IDictionary<string, string> metadata)
        {
            try
            {
                log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

                if (!(metadata.TryGetValue("userid", out string userId)
                   && metadata.TryGetValue("path", out string path)
                && metadata.TryGetValue("order", out string order)))
                    throw new Exception(CommonErrorMessages.Request_Is_Not_Valid);

                var userPhotoService = ServiceResolver.CreateInstance<IUserPhotoService>();
                var userResult = await userPhotoService.SetUserPhotoAsync(Convert.ToInt32(userId), path, Convert.ToInt32(order));
                if (!userResult.IsSuccessed)
                    throw new Exception(userResult.GetErrorString());

            }
            catch (Exception ex)
            {
                var pearUpLogger = ServiceResolver.CreateInstance<IPearUpLogger>();
                pearUpLogger.LogError(ex);
            }
        }
    }
}
