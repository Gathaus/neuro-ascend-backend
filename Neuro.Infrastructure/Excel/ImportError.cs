namespace POI.Application.Base.Excel;

public class ImportError
{
    public int RowNumber { get; set; }
    public string ColumnName { get; set; }
    public string ErrorMessage { get; set; }
    
    public override string ToString()
    {
        return $"Row: {RowNumber}, Column: {ColumnName}, Error: {ErrorMessage}";
    }
}