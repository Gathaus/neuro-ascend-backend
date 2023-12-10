using Neuro.Application.Base.Result;

namespace Neuro.Application.Dtos.CustomResponses;

public class TableResponse<TData> : BaseResult
{
    public List<string> Headers { get; set; }
    public List<Header> PropertiesWithHeaders { get; set; }
    public IList<TData> Rows { get; set; }
}

public class PagedTableResponse<TData> : TableResponse<TData>
{
    public int TotalItemCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public string ErrorMessage { get; set; }
}

public class Header
{
    public string name { get; set; }
    public string param { get; set; }
    public string dataType { get; set; }
}