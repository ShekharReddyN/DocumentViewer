using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;

namespace DriveAccessTokenApp
{
    class Program
    {
        public static async Task<string> GetAccessTokenFromJSONKeyAsync(string jsonKeyFilePath, params string[] scopes)
        {
            using (var stream = new FileStream(jsonKeyFilePath, FileMode.Open, FileAccess.Read))
            {
                return await GoogleCredential
                    .FromStream(stream) // Loads key file  
                    .CreateScoped(scopes) // Gathers scopes requested  
                    .UnderlyingCredential // Gets the credentials  
                    .GetAccessTokenForRequestAsync(); // Gets the Access Token  
            }
        }
        public static string GetAccessTokenFromJSONKey(string jsonKeyFilePath, params string[] scopes)
        {
            return GetAccessTokenFromJSONKeyAsync(jsonKeyFilePath, scopes).Result;
        }
        static void Main(string[] args)
        {
            var token = GetAccessTokenFromJSONKey(@"E:\Repos\GoogleDriveViewer\AssignmentPoject-20301828176f.json", "https://www.googleapis.com/auth/drive", "https://www.googleapis.com/auth/drive.file", "https://www.googleapis.com/auth/drive.appdata", "https://www.googleapis.com/auth/drive.metadata", "https://www.googleapis.com/auth/drive.metadata.readonly");
            //var apikey = "AIzaSyD-xr7HUNL-Fb4JjMJ0mgdFiqA-efgmXrM";
            string cr = "user";
            string url = $"https://www.googleapis.com/drive/v3/files?pageSize=100&corpora={cr}&access_token={token}&fields=*";
            string content = new HttpClient().GetStringAsync(url).Result;
            Console.WriteLine("token : " + token );
            Console.WriteLine(content);
            Console.ReadLine();
        }
    }
}
