using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace NyaaApi_DotNet.Controller.Interface
{
    public interface IQADynamo
    {
        public Task<APIGatewayProxyResponse> AddQuestion(IDynamoDBContext DDBContext,
        APIGatewayProxyRequest request, ILambdaContext context);

    }
}
