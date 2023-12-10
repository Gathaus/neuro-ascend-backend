namespace Neuro.Infrastructure.Excel;

public class ImportResult<T> where T : class, new()
{
    public List<T> Data { get; set; } = new List<T>();
    public List<ImportError> Errors { get; set; } = new List<ImportError>();
}
