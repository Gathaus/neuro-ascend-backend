using System.Net;
using Neuro.Infrastructure.Logging.Exceptions;



namespace Neuro.Application.Exceptions
{
    public class AuthorizationException : BaseException
    {
        public AuthorizationException(int code, string? exceptionMessage = null, HttpStatusCode? status = null) : base(ExceptionTypesEnum.Authorization, code, exceptionMessage, status)
        {
        }
    }

    public class AuthorizationExceptions 
    {
        public static AuthorizationException UserNotAuthorizedException => new AuthorizationException(0001, "Kullanıcı isSoUser ve isSuperUser değil ya da salespointin hierarchy'inde değil");
        public static AuthorizationException NotPermittedCompany => new AuthorizationException(0002, "İlgili firma için kullanıcının yetkisi yok.");
        public static AuthorizationException NotInSalesHierarchy => new AuthorizationException(0003, "Kullanıcı sales hiyerarşiye dahil değil.");
	}
}
