using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;

namespace NyaaApi_DotNet.Controller.Interface
{
    public interface INyaaSukebeiServices
    {
        Task<APIGatewayProxyResponse> GetSukebeiSearch(string title, int category, int sub_category);
    }
}
