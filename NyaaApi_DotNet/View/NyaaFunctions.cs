using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using NyaaApi_DotNet.Common;
using NyaaApi_DotNet.Controller.Implementation;
using NyaaApi_DotNet.Controller.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]


namespace NyaaApi_DotNet.View
{

    class NyaaFunctions
    {
        private INyaaServices nyaa;
        private IJikanAnime jikanAnime;
        private IQADynamo qaDynamo;
        private INyaaSukebeiServices nyaaSukebei;
        private string strResult;
        IDynamoDBContext DDBContext { get; set; }

        public NyaaFunctions()
        {
            this.nyaa = new NyaaServices();
            this.jikanAnime = new JikanAnime();
            this.qaDynamo = new QADynamo();
            this.nyaaSukebei = new NyaaSukebeiServices();
            var tableName = System.Environment.GetEnvironmentVariable(DynamoTable.TABLE);
            if (!string.IsNullOrEmpty(tableName))
            {
                AWSConfigsDynamoDB.Context.TypeMappings[typeof(QADynamo)]
                    = new Amazon.Util.TypeMapping(typeof(QADynamo), tableName);
            }

            var config = new DynamoDBContextConfig { Conversion = Amazon.DynamoDBv2.DynamoDBEntryConversion.V2 };
            this.DDBContext = new DynamoDBContext(new AmazonDynamoDBClient(), config);
        }
        public async Task<APIGatewayProxyResponse> GetNyaaSearch(APIGatewayProxyRequest request)
        {
            return await nyaa.GetNyaaSearchEngAnime(request);
        }

        public async Task<APIGatewayProxyResponse> GetNyaaSearchByEp(APIGatewayProxyRequest request)
        {
            return await nyaa.GetNyaaSearchEngAnime(request);
        }
        public async Task<APIGatewayProxyResponse> GetTopAnime(APIGatewayProxyRequest request)
        {
            int page = -1;
            string subtype = null;
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    foreach (KeyValuePair<string, string> v in request.QueryStringParameters)
                    {
                        string key = v.Key;
                        switch (key)
                        {
                            case JikanAwsParameter.PAGE:
                                page = int.Parse(v.Value);
                                break;
                            case JikanAwsParameter.SUB:
                                subtype = v.Value;
                                break;
                            default:
                                break;
                        }
                    }
                    return await jikanAnime.GetTopAnime(page, subtype);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }
        }


        public async Task<APIGatewayProxyResponse> GetTopHentai(APIGatewayProxyRequest request)
        {
            int page = -1;
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    foreach (KeyValuePair<string, string> v in request.QueryStringParameters)
                    {
                        string key = v.Key;
                        switch (key)
                        {
                            case JikanAwsParameter.PAGE:
                                page = int.Parse(v.Value);
                                break;
                            default:
                                break;
                        }
                    }
                    return await jikanAnime.GetTopHentai(page);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }
        }

        public async Task<APIGatewayProxyResponse> SearchAnimeSeasonal(APIGatewayProxyRequest request)
        {
            string season = null;
            int year = -1;
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    foreach (KeyValuePair<string, string> v in request.QueryStringParameters)
                    {
                        string key = v.Key;
                        switch (key)
                        {
                            case JikanAwsParameter.SEASON:
                                season = v.Value;
                                break;
                            case JikanAwsParameter.YEAR:
                                year = int.Parse(v.Value);
                                break;
                            default:
                                break;
                        }
                    }
                    return await jikanAnime.SearchAnimeSeasonal(season, year);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }

        }


        public async Task<APIGatewayProxyResponse> SearchAnimeSeasonalWrapper(APIGatewayProxyRequest request)
        {
            string season = null;
            int year = -1;
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    foreach (KeyValuePair<string, string> v in request.QueryStringParameters)
                    {
                        string key = v.Key;
                        switch (key)
                        {
                            case JikanAwsParameter.SEASON:
                                season = v.Value;
                                break;
                            case JikanAwsParameter.YEAR:
                                year = int.Parse(v.Value);
                                break;
                            default:
                                break;
                        }
                    }
                    return await jikanAnime.SearchAnimeSeasonalWrapper(season, year);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }

        }

        public async Task<APIGatewayProxyResponse> SearchAnimeTmp(APIGatewayProxyRequest request)
        {
            string title = null;
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    foreach (KeyValuePair<string, string> v in request.QueryStringParameters)
                    {
                        string key = v.Key;
                        switch (key)
                        {
                            case JikanAwsParameter.TITLE:
                                title = v.Value;
                                break;
                            default:
                                break;
                        }
                    }
                    return await jikanAnime.SearchAnimeTmp(title);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }
        }

        public async Task<APIGatewayProxyResponse> GetAnimeEpisode(APIGatewayProxyRequest request)
        {
            int id = -1;
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    foreach (KeyValuePair<string, string> v in request.QueryStringParameters)
                    {
                        string key = v.Key;
                        switch (key)
                        {
                            case JikanAwsParameter.ANIMEID:
                                id = int.Parse(v.Value);
                                break;
                            default:
                                break;
                        }
                    }
                    return await jikanAnime.GetAnimeEpisode(id);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }
        }

        public async Task<APIGatewayProxyResponse> GetAnimeDetail(APIGatewayProxyRequest request)
        {
            int id = -1;
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    foreach (KeyValuePair<string, string> v in request.QueryStringParameters)
                    {
                        string key = v.Key;
                        switch (key)
                        {
                            case JikanAwsParameter.ANIMEID:
                                id = int.Parse(v.Value);
                                break;
                            default:
                                break;
                        }
                    }
                    return await jikanAnime.GetAnimeDetail(id);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }
        }


        public async Task<APIGatewayProxyResponse> AddQuestion(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return await qaDynamo.AddQuestion(DDBContext, request, context);
        }



        public async Task<APIGatewayProxyResponse> GetSearchNyaaSukebeiAnime(APIGatewayProxyRequest request)
        {
            string title = "";
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    title = QueryTitle(request);
                    return await GetSukebeiSearch(title, 1, 1);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }
        }


        public async Task<APIGatewayProxyResponse> GetSearchNyaaSukebeiEroge(APIGatewayProxyRequest request)
        {
            ///return await nyaaSukebei.GetNyaaSukebei(request);
            string title = "";
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    title = QueryTitle(request);
                    return await GetSukebeiSearch(title, 1, 3);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }
        }

        public async Task<APIGatewayProxyResponse> GetSearchNyaaSukebeiDoujinshi(APIGatewayProxyRequest request)
        {

            ///return await nyaaSukebei.GetNyaaSukebei(request);
            string title = "";
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    title = QueryTitle(request);
                    return await GetSukebeiSearch(title, 1, 2);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }
        }

        public async Task<APIGatewayProxyResponse> Test(APIGatewayProxyRequest request)
        {
            Vndbs a = new Vndbs();
            a.test();
            return Https.apiResponse(HttpStatusCode.OK, "test");
        }

        public async Task<APIGatewayProxyResponse> GetSearchNyaaSukebeiJAV(APIGatewayProxyRequest request)
        {
            ///return await nyaaSukebei.GetNyaaSukebei(request);
            string title = "";
            if (request.QueryStringParameters.Keys.Count < 1)
            {
                strResult = "Error: {Keys.count < 1} Please Provide Correct Parameter Value";
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            else
            {
                try
                {
                    title = QueryTitle(request);
                    return await GetSukebeiSearch(title, 2, 2);
                }
                catch (Exception e)
                {
                    strResult = "Error Exception Found: " + e.ToString();
                    return Https.apiResponse(HttpStatusCode.OK, strResult);
                }
            }
        }

        private string QueryTitle(APIGatewayProxyRequest request)
        {
            string title = "";
            foreach (KeyValuePair<string, string> v in request.QueryStringParameters)
            {
                string key = v.Key;
                switch (key)
                {
                    case JikanAwsParameter.TITLE:
                        title = v.Value;
                        break;
                    default:
                        break;
                }
            }

            return title;
        }

        private async Task<APIGatewayProxyResponse> GetSukebeiSearch(string q, int cate, int sub_cate)
        {
            try
            {
                return await nyaaSukebei.GetSukebeiSearch(q, cate, sub_cate);
            }
            catch (Exception e)
            {
                strResult = "Error Exception Found: " + e.ToString();
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }

        }
    }

}
