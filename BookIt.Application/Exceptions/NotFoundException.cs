using BookIt.Application.Exceptions.Common;
using System.Net;

namespace BookIt.Application.Exceptions;

public class NotFoundException : Exception, IBaseException
{
    public NotFoundException(string message = "Tapilmadi") : base(message)
    {

    }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;
}
