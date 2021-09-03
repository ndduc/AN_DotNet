using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using NyaaApi_DotNet.Common;
using NyaaApi_DotNet.Controller.Interface;
using NyaaApi_DotNet.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NyaaApi_DotNet.Controller.Implementation
{
    class QADynamo : IQADynamo
    {
        private string hashValue = "QA_V1";
        public async Task<APIGatewayProxyResponse> AddQuestion(IDynamoDBContext DDBContext,
        APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                Anifinders_QA qa = JsonConvert.DeserializeObject<Anifinders_QA>(request?.Body);
                if (qa.Question == null) {
                    throw new Exception();
                }
                qa.HashKey = hashValue;
                qa.RangeKey = getGuid();
                qa.TimeStamp = DateTime.UtcNow;
                qa.Question = qa.Question.ToLower();
                await DDBContext.SaveAsync(qa);
                strResult = qa.RangeKey.ToString();
                /*return Https.apiResponse(HttpStatusCode.OK, qa.RangeKey.ToString());
                return await responseHelperAsync(response);*/
                return Https.apiResponse(HttpStatusCode.OK, strResult);
            }
            catch (Exception err)
            {
                return Https.apiResponse(HttpStatusCode.BadRequest, "Missing required parameter\t\t" + err);
            }

        }

        private string getGuid()
        {
            string id = Guid.NewGuid().ToString(); ;
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            var rest = new QueryRequest
            {
                TableName = DynamoTable.TABLE,
                KeyConditionExpression = "HashKey = :hk AND RangeKey = :rk",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                 {":hk", new AttributeValue { S =  hashValue }},
                 {":rk", new AttributeValue { S =  id }}
                }
            };
            var response = client.QueryAsync(rest);
            if (response.Result.Count == 0)
            {
                return id;
            }
            else
            {
                return getGuid();
            }
        }

        private string strResult;
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
