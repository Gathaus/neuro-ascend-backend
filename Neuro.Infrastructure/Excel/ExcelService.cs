using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Neuro.Infrastructure.Excel;
using OfficeOpenXml;
using POI.Application.Base.Excel;

public class ExcelService : IExcelService
{
    public ExcelService()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }
    public ImportResult<T> Import<T>(Stream fileStream) where T : class, new()
    {
        var result = new ImportResult<T>();

        var properties = typeof(T).GetProperties();

        using (var package = new ExcelPackage(fileStream))
        {
            var worksheet = package.Workbook.Worksheets[0];
            
            // Headers check
            for (int col = 1; col <= properties.Length; col++)
            {
                var expectedHeader = properties[col - 1].Name;
                var actualHeader = worksheet.Cells[1, col].Value?.ToString();

                if (expectedHeader != actualHeader)
                {
                    var error = new ImportError 
                    {
                        RowNumber = 1,
                        ColumnName = actualHeader,
                        ErrorMessage = $"Expected header '{expectedHeader}' but found '{actualHeader}'."
                    };
                    result.Errors.Add(error);
                    return result; // Başlıklar doğru değilse işlemi hemen durduruyoruz
                }
            }
            
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var item = new T();
                foreach (var property in properties)
                {
                    object cellValue = null; 

                    try
                    {
                        cellValue = worksheet.Cells[row, properties.ToList().IndexOf(property) + 1].Value;

                        if (property.PropertyType.IsEnum)
                        {
                            int intValue = Convert.ToInt32(cellValue);
                            var enumValue = Enum.ToObject(property.PropertyType, intValue);
                            property.SetValue(item, enumValue);
                        }
                        else
                        {
                            var value = Convert.ChangeType(cellValue, property.PropertyType);
                            property.SetValue(item, value);
                        }
                    }
                    catch (Exception ex)
                    {
                        var error = new ImportError 
                        {
                            RowNumber = row,
                            ColumnName = property.Name,
                            ErrorMessage = $"Error converting value '{cellValue}' to type '{property.PropertyType.Name}'. Original error: {ex.Message}"
                        };
                        result.Errors.Add(error);
                    }
                }
                result.Data.Add(item);
            }
        }

        return result;
    }

    public Stream Export<T>(ExportRequest<T> request) where T : class, new()
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Data");

            for (int i = 0; i < request.Headers.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = request.Headers[i];
            }

            int row = 2;
            foreach (var item in request.Data)
            {
                for (int col = 0; col < request.Headers.Count; col++)
                {
                    var property = typeof(T).GetProperty(request.Headers[col]);
                    if (property != null)
                    {
                        worksheet.Cells[row, col + 1].Value = property.GetValue(item);
                    }
                }
                row++;
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }
    }
}
