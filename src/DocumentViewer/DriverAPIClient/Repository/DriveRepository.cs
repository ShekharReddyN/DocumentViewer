using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DriverAPIClient.Models;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace DriverAPIClient.Repository
{

    public class DriveRepository : IDriveRepository
    {
        public DriveRepository()
        {

        }
        private Task<string> GetAccessTokenFromJSONKeyAsync()
        {
            var scopes = new List<string>() { "https://www.googleapis.com/auth/drive" };

            return GoogleCredential.FromFile(@"\Assests\AssignmentPoject-20301828176f.json").CreateScoped(scopes) // Gathers scopes requested  
                    .UnderlyingCredential // Gets the credentials  
                    .GetAccessTokenForRequestAsync(); // Gets the Access Token  ;
        }
        private HttpClient GetHttpClient()
        {
            var _client = new HttpClient();
            _client.BaseAddress = new Uri("https://www.googleapis.com/drive/v3");
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessTokenFromJSONKeyAsync().Result);
            return _client;
        }

        public DriveModel GetDirectoryFileList(string directoryName)
        {
            var client = GetHttpClient();
            var directoryJsonObject = client.GetStringAsync($"/files?q={directoryName}");
            var drive = JsonConvert.DeserializeObject<DriveModel>(directoryJsonObject.Result);
            if (drive.files != null)
            {
                //Get the all the files and folders within this directory
                var id = drive.files[0].id;
                var filesJson = client.GetStringAsync($"/files?q={id} in parents").Result;
                return JsonConvert.DeserializeObject<DriveModel>(filesJson);
            }
            return null;
        }

    }
}