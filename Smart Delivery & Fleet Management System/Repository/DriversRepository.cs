using Smart_Delivery___Fleet_Management_System.Interface;
using Smart_Delivery___Fleet_Management_System.Models;
using Smart_Delivery___Fleet_Management_System.Data;
using Microsoft.EntityFrameworkCore;
using Smart_Delivery___Fleet_Management_System.Enums;
namespace Smart_Delivery___Fleet_Management_System.Repository
{
    public class DriversRepository : IDriversRepository
    {
        private readonly DeliveryDbContext _context;

        public DriversRepository(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
        }

        public async Task DeleteAsync(Driver driver)
        {
            _context.Drivers.Remove(driver);
            await Task.CompletedTask;
        }

        public async Task<int> GetActiveOrdersCountAsync(int driverId)
        {
            return await _context.Orders
                .CountAsync(o =>
                    o.AssignedDriverId == driverId &&
                    o.Status != OrderStatus.Delivered &&
                    o.Status != OrderStatus.Failed);
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetAvailableWithLocationAsync()
        {
            return await _context.Drivers
                .Where(d=>d.Status== DriverStatus.Available).ToListAsync();
        }

        public async Task<Driver?> GetByIdAsync(int id)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Driver?> GetByUserIdAsync(int userId)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            await Task.CompletedTask;
        }

        public async Task UpdateDriverLocationAsync(int driverId, double lat, double lng)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null) return;

            driver.CurrentLat = lat;
            driver.CurrentLng = lng;

            var location = new DriverLocation
            {
                DriverId = driverId,
                Lat = lat,
                Lng = lng,
                Timestamp = DateTime.UtcNow
            };

            await _context.DriverLocations.AddAsync(location);
        }

    }
}
