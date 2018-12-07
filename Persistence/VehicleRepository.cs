using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vega.Models;
using Vega.Core;
using Vega.Core.Models;
using System.Linq;

namespace Vega.Persistence {
    public class VehicleRepository : IVehicleRepository{
        private VegaDbContext _context { get; }
        public VehicleRepository (VegaDbContext context) {
            this._context = context;
        }
        public async Task<Vehicle> GetVehicle (int id, bool includeRelated = true)
        {
            if(!includeRelated)
                return await _context.Vehicles.FindAsync(id);
            return await _context.Vehicles
            .Include(v => v.Features)
            .ThenInclude(vf => vf.Feature)
            .Include(v => v.Model)
            .ThenInclude(vm => vm.Make)
            .SingleOrDefaultAsync(v => v.Id == id);
        }
        public async Task<IEnumerable<Vehicle>> GetVehicles (Filter filter)
        {
            var query =  _context.Vehicles
            .Include(v => v.Model)
            .ThenInclude(vm => vm.Make)
            .Include(v => v.Features)
            .ThenInclude(vf => vf.Feature)
            .AsQueryable();

            if(filter.MakeId.HasValue)
                query = query.Where(v => v.Model.MakeId == filter.MakeId.Value);
            return await query.ToListAsync();
        }
        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }
        public void Remove(Vehicle vehicle)
        {
            _context.Remove(vehicle);
        }
    }
}