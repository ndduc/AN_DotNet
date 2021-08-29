using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;

namespace NyaaApi_DotNet.Controller.Interface
{
    public interface IJikanManga
    {
        public Task<APIGatewayProxyResponse> GetManga(int mangaId, string path);
    }
}
