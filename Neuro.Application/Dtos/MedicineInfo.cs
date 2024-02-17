namespace Neuro.Application.Dtos;

public class MedicineInfo
{
    public int Id { get; set; }
    public string MedicationName { get; set; }
    public TimeSpan Time { get; set; }
    public int PillNumber { get; set; }
    public string Usage { get; set; }
}