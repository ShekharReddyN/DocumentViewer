using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using DocumentViewer.Models;
using Newtonsoft.Json;

namespace DocumentViewer.Repository
{
    public class DriveRepository : IDriveRepository
    {
        private readonly HttpClient httpClient;
        public DriveRepository()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://www.googleapis.com/drive/v3/");

        }

        public bool DeleteFile(string token, string id)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = httpClient.DeleteAsync($"files/{id}").Result;
            return response.StatusCode == System.Net.HttpStatusCode.NoContent ? true : false;



        }

        public DriveModel GetDirectoryFileList(string directoryName, string accessToken)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var directoryJsonObject = httpClient.GetStringAsync($"files?q={directoryName}");
            var drive = JsonConvert.DeserializeObject<DriveModel>(directoryJsonObject.Result);
            if (drive.files != null)
            {
                //Get the all the files and folders within this directory
                var id = drive.files[0].id;
                var filesJson = httpClient.GetStringAsync($"files?q={id} in parents").Result;
                return JsonConvert.DeserializeObject<DriveModel>(filesJson);
            }
            return null;
        }

        public StreamContent GetFile(string id,string acsToken)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + acsToken);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"files/{id}?alt=media");
            var response =  httpClient.SendAsync(httpRequestMessage).Result;
            return response.Content as StreamContent;

        }

        public DriveModel GetFileList(string acToken,string rootId = "")
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + acToken);
            if (string.IsNullOrEmpty(rootId))
            {
                var rootFolder = httpClient.GetStringAsync($"files/root?fields=id");
                rootId = JsonConvert.DeserializeObject<RootFile>(rootFolder.Result).Id;
            }

            var directoryJsonObject = httpClient.GetStringAsync($"files?q='{rootId}' in parents&pageSize=400&fields=files,incompleteSearch,kind,nextPageToken");
            var drive = JsonConvert.DeserializeObject<DriveModel>(directoryJsonObject.Result);

            //var directoryJsonObject = httpClient.GetStringAsync($"files?pageSize=4&fields=files,incompleteSearch,kind,nextPageToken");
            //var test = httpClient.GetStringAsync($"files?pageSize=4&fields=files,incompleteSearch,kind,nextPageToken");
            return drive;
        }

        public bool UploadFile(HttpPostedFileBase file, string v)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + v);
            //httpClient.DefaultRequestHeaders.Add("Content-Type", "multipart/related");

            httpClient.BaseAddress = new Uri("https://www.googleapis.com/upload/drive/v3/");
            var _file = new DriveFile()
            {
                name = Path.GetFileName(file.FileName),
                mimeType = file.ContentType,
                parents = new List<string>() { "1crgdmvhinXCTDH-wkRmjFOIxsEhECEQj" }
            };

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "files?uploadType=multipart");

            //    httpRequestMessage.Headers.Add("Content-Type", "multipart/related");
            var ft = new MultipartFormDataContent();

            var stContent = new StreamContent(file.InputStream);
            stContent.Headers.Add("Content-Type", file.ContentType);
            var json = new StringContent(JsonConvert.SerializeObject(_file), Encoding.UTF8, "application/json");
            //json.Headers.Add("Content-Type", "Application/json");
            ft.Add(json);
            ft.Add(stContent);

            httpRequestMessage.Content = ft;

            //var multi = new MultipartContent();
            //multi.Add()

            //var fileUploadTask = httpClient.PostAsync("files?uploadType=multipart", ft);

            var fileUploadTask = httpClient.SendAsync(httpRequestMessage);


            var fileUploadResponse = fileUploadTask.Result;
            return fileUploadResponse.IsSuccessStatusCode;
        }
    }
}