using System.Net;

namespace BookIt.Application.Exceptions.Common;

public interface IBaseException
{
    public HttpStatusCode StatusCode { get; set; }
}
