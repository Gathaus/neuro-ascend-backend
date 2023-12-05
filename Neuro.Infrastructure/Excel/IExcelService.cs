using Neuro.Infrastructure.Excel;

namespace POI.Application.Base.Excel;

public interface IExcelService
{
    ImportResult<T> Import<T>(Stream fileStream) where T : class, new();
    Stream Export<T>(ExportRequest<T> request) where T : class, new();
}
