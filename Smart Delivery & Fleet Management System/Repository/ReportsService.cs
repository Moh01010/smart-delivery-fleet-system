using Microsoft.EntityFrameworkCore;
using Smart_Delivery___Fleet_Management_System.Data;
using Smart_Delivery___Fleet_Management_System.DTOs.Reports;
using Smart_Delivery___Fleet_Management_System.Enums;
using Smart_Delivery___Fleet_Management_System.Interface;

namespace Smart_Delivery___Fleet_Management_System.Repository
{
    public class ReportsService : IReportsService
    {
        private readonly DeliveryDbContext _context;

        public ReportsService(DeliveryDbContext context)
        {
            _context = context;
        }
        public async Task<AverageDeliveryTimeDto> GetAverageDeliveryTimeAsync()
        {
            var deliveredOrders = await _context.Orders
            .Where(o => o.Status == OrderStatus.Delivered)
            .ToListAsync();

            double totalMinutes = 0;
            int count = 0;

            foreach (var order in deliveredOrders)
            {
                var created = order.CreatedAt;

                var delivered = await _context.OrderStatusHistories
                    .Where(h => h.OrderId == order.Id && h.Status == OrderStatus.Delivered)
                    .Select(h => h.Timestamp)
                    .FirstOrDefaultAsync();

                totalMinutes += (delivered - created).TotalMinutes;
                count++;
            }

            return new AverageDeliveryTimeDto
            {
                AverageMinutes = count == 0 ? 0 : totalMinutes / count
            };
        }

        public async Task<IEnumerable<DriverPerformanceDto>> GetDriversPerformanceAsync()
        {
            return await _context.OrderStatusHistories
            .Where(h => h.Status == OrderStatus.Delivered && h.UpdatedByDriverId != null)
            .GroupBy(h => h.UpdatedByDriverId)
            .Select(g => new DriverPerformanceDto
            {
                DriverId = g.Key.Value,
                DriverName = _context.Drivers
                    .Where(d => d.Id == g.Key.Value)
                    .Select(d => d.User.Name)
                    .FirstOrDefault(),
                DeliveredOrders = g.Count()
            })
            .ToListAsync();
        }

        public async Task<FailureRateDto> GetFailureRateAsync()
        {
            var total = await _context.Orders.CountAsync();
            var failed = await _context.Orders
                .CountAsync(o => o.Status == OrderStatus.Failed);

            return new FailureRateDto
            {
                TotalOrders = total,
                FailedOrders = failed,
                FailurePercentage = total == 0 ? 0 : (double)failed / total * 100
            };
        }
    }
}
