using Assembly.IBX.WebIO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Assembly.IBX.Discord
{
    internal static class CDN
    {
        const string AVATAR_ASSET_PATH = "/discord/avatar.gif";
        const string BANNER_ASSET_PATH = "/discord/banner.gif";

        /// <summary>
        /// Build a cdn rsource uri from a CDN Resource
        /// </summary>
        /// <param name="cdnResource">The CDN Resource to build the uri from</param>
        /// <returns>A URI for the specified CDN resource</returns>
        public static string BuildResourceURI(CDNResource cdnResource)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("https://");
            sb.Append(Configuration.discordConfiguration.cdn_root);

            switch(cdnResource.resourceType)
            {
                case CDNResource.ResourceType.AVATAR:
                    sb.Append(Configuration.discordConfiguration.cdn_avatar_path);
                    break;
                case CDNResource.ResourceType.BANNER:
                    sb.Append(Configuration.discordConfiguration.cdn_banner_path);
                    break;
                default:
                    break;
            }

            sb.Append('/');
            sb.Append(cdnResource.clientId);
            sb.Append('/');
            sb.Append(cdnResource.filename);
            sb.Append('.');
            sb.Append(IOSystem.GetImageFileExtension(cdnResource.imageFileFormat));

            string resourceURI = sb.ToString();
            return resourceURI;
        }

        internal static async Task<Texture2D> DownloadImageFromURI(string uri, int size = 512)
        {
            UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(uri + $"?size={size}");

            Debug.Log($"Downloading Textrue2D asset from {uri}");

            AsyncOperation operation = uwr.SendWebRequest();

            while(!operation.isDone)
            {
                await Task.Delay(1000 / 30); //30 Hz
            }

            if(uwr.result != UnityWebRequest.Result.Success)
            {
                return null;
            }

            return ((DownloadHandlerTexture)uwr.downloadHandler).texture;
            
        }
    }
}
