using Neuro.Domain.Entities;

public class TimeOfDay : BaseEntity<int>
{
    private DateTime time;

    public DateTime Time
    {
        get { return time; }
        set
        {
            // Gelen DateTime değerini UTC'ye dönüştür
            var utcTime = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();

            // Sadece saat, dakika ve saniye bilgisini saklayın, tarih kısmını dikkate almayın
            time = new DateTime(1, 1, 1, utcTime.Hour, utcTime.Minute, utcTime.Second, DateTimeKind.Utc);
        }
    }

    public int UserMedicineId { get; set; }
    public UserMedicine UserMedicine { get; set; }
}