using Neuro.Infrastructure.Excel;

namespace POI.Application.Base.Excel;

public interface IImportService<T> where T : class, new()
{
    ImportResult<T> Import(Stream fileStream);
}
