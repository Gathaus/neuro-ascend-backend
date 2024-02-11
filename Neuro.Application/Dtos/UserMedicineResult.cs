namespace Neuro.Application.Dtos;

public class UserMedicineResult
{
    public List<MedicineInfo> Medicines { get; set; } = new List<MedicineInfo>();
    public List<MedicineInfo> NextMedicines { get; set; } = new List<MedicineInfo>();
    public List<MedicineInfo> ForgottenMedicines { get; set; } = new List<MedicineInfo>();

}