
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using testlol.Models.DTOs;
using testlol.Utills;

namespace testlol.Scripts
{
    class RiotClientManager
    {
        private HttpClient httpClient;
        private bool isConnected = false;
        public delegate void LeagueClosedHandler();
        public event LeagueClosedHandler LeagueClosed; // 클라이언트 닫을 때 
        private bool isLeagueClosed = false;


        // Riot LCU API를 통해 사용자 정보를 가져오는 메서드
        public async Task<UserDTO> GetUserInfo()
        {
            var response = await UsingApiEventJObject("Get", "lol-summoner/v1/current-summoner");

            if (response != null)
            {
                return JsonConvert.DeserializeObject<UserDTO>(response.ToString());
            }
            else
            {
                return null;
            }
        }

        public bool Connect()
        {
            ConnectInit();
            try
            {
                ConnectInit();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{ClientData.ToKen}")));
                httpClient.BaseAddress = new Uri(ClientData.ApiUrl);

                ClientData.clientProcess.EnableRaisingEvents = true;
                if (!isLeagueClosed)
                {
                    ClientData.clientProcess.Exited += ClientProcess_Exited;
                    isLeagueClosed=true;
                }
                

                return true;
            }
            catch
            {
                Console.WriteLine("Connect 오류");

                return false;
            }
        }

        private void  ClientProcess_Exited(object? sender, EventArgs e)
        {
            LeagueClosed();
        }

        // Client 핸들 초기화
        private void ConnectInit()
        {
            

            this.httpClient = null;

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            httpClient = new HttpClient(handler);

            try
            {
                Process[] processes = Process.GetProcessesByName(ClientData.CLIENT_NAME);
                ClientData.clientProcess = processes[0];

                ClientData.LeaguePath = Path.GetDirectoryName(ClientData.clientProcess.MainModule.FileName);
                var lockFilePath = Path.Combine(ClientData.LeaguePath, "lockfile");

                using (var fileStream = new FileStream(lockFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fileStream))
                {
                    var text = reader.ReadToEnd();
                    string[] items = text.Split(':');
                    ClientData.ToKen = items[3];
                    ClientData.Port = ushort.Parse(items[2]);
                    ClientData.ApiUrl = "https://127.0.0.1:" + ClientData.Port.ToString() + "/";
                }
                isConnected = true;
            }
            catch
            {
                isConnected = false;
                Console.WriteLine("connection failed");
            }
        }

        /// <summary>
        /// API 메시지를 JObject로 리턴
        /// </summary>
        /// <param name="method">Get,Post,Put,Delete 타입</param>
        /// <param name="endpoint">API 주소</param>
        /// <param name="data">보낼 데이터</param>
        public async Task<JObject> UsingApiEventJObject(string method, string endpoint, object data = null)
        {
            var response = await UsingApiEventHttpMessage(method, endpoint, data);
            var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
            return responseJson;
        }

        /// <summary>
        /// API 메시지를 HttpResponseMessage로 리턴
        /// </summary>
        /// <param name="method">Get,Post,Put,Delete 타입</param>
        /// <param name="endpoint">API 주소</param>
        /// <param name="data">보낼 데이터</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> UsingApiEventHttpMessage(string method, string endpoint, object data = null)
        {
            var json = data == null ? "" : JsonConvert.SerializeObject(data);
            switch (method)
            {
                case "Get":
                    return await httpClient.GetAsync(endpoint);
                case "Post":
                    return await httpClient.PostAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"));
                case "Put":
                    return await httpClient.PutAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"));
                case "Delete":
                    return await httpClient.DeleteAsync(endpoint);
                default:
                    throw new Exception("Unsupported HTTP method");
            }
        }
    }
}
