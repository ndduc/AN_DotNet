using Amazon.Lambda.APIGatewayEvents;
using NyaaApi_DotNet.API;
using NyaaApi_DotNet.Common;
using NyaaApi_DotNet.Controller.Interface;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace NyaaApi_DotNet.Controller.Implementation
{
    class NyaaSukebeiServices : INyaaSukebeiServices
    {

        readonly string URL = SukeibeApi.SUKEIBE;
        private string strResult;
        private StringBuilder endpoint;

        public async Task<APIGatewayProxyResponse> GetSukebeiSearch(string title, int category, int sub_category)
        {
            endpoint = new StringBuilder();
            if (title.Length <= 1)
            {
                strResult = "Missing Parameters";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {


                endpoint.Append(URL).Append(SukeibeApi.QUEST).
                    Append(SukeibeApiParameter.TITLE).Append(title).
                    Append(SukeibeApi.AND).Append(SukeibeApiParameter.CATE).Append(category).
                    Append(SukeibeApi.AND).Append(SukeibeApiParameter.SUBCATE).Append(sub_category);


                using var client = new HttpClient();
                client.BaseAddress = new Uri(endpoint.ToString());
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                return await responseHelperAsync(response);
            }
        }

        private async Task<APIGatewayProxyResponse> responseHelperAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                strResult = await response.Content.ReadAsStringAsync();
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                strResult = "No Search Result Found";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
        }

    }
}
