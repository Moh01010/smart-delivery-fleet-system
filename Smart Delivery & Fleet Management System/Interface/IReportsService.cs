using Smart_Delivery___Fleet_Management_System.DTOs.Reports;

namespace Smart_Delivery___Fleet_Management_System.Interface
{
    public interface IReportsService
    {
        Task<IEnumerable<DriverPerformanceDto>> GetDriversPerformanceAsync();
        Task<AverageDeliveryTimeDto> GetAverageDeliveryTimeAsync();
        Task<FailureRateDto> GetFailureRateAsync();
    }
}
