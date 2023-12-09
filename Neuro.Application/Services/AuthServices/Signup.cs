using Neuro.Application.Base.Result;
using Neuro.Application.Base.Service;
using Neuro.Domain.UnitOfWork;

namespace Neuro.Application.Services.AuthServices;

public class Signup : IBusinessService<Signup.Request, Signup.Response>
{
    private readonly IUnitOfWork _unitOfWork;

    #region constructor

    public Signup(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Request & Response

    public class Request
    {
    }

    public class Response
    {
    }

    #endregion

    public async Task<Result<Response>> ExecuteAsync(Request request)
    {
        //TODO write business code here
        return null;
    }
}