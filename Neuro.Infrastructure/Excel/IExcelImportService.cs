namespace Neuro.Infrastructure.Excel;

public interface IImportService<T> where T : class, new()
{
    ImportResult<T> Import(Stream fileStream);
}
