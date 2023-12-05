namespace Neuro.Infrastructure.Excel;

public class ExportRequest<T>
{
    public List<T> Data { get; set; }
    public List<string> Headers { get; set; }
}