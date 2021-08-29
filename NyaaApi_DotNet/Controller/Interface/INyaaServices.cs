using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;


namespace NyaaApi_DotNet.Controller.Interface
{
    public interface INyaaServices
    {
        Task<APIGatewayProxyResponse> GetNyaaSearchEngAnime(APIGatewayProxyRequest request);

    }
}
