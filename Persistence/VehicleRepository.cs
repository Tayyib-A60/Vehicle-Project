using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vega.Models;
using Vega.Core;
using Vega.Core.Models;
using System.Linq;
using System.Linq.Expressions;
using System;
using Vega.Extensions;
namespace Vega.Persistence {
     
    public class VehicleRepository : IVehicleRepository {
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
        public async Task<QueryResult<Vehicle>> GetVehicles (VehicleQuery queryObj)
        {
            var result = new QueryResult<Vehicle>();
            var query =  _context.Vehicles
            .Include(v => v.Model)
            .ThenInclude(vm => vm.Make)
            .Include(v => v.Features)
            .ThenInclude(vf => vf.Feature)
            .AsQueryable();

            if(queryObj.MakeId.HasValue)
             query = query.Where(v => v.Model.MakeId == queryObj.MakeId.Value);
            if(queryObj.ModelId.HasValue)
             query = query.Where(v => v.Model.MakeId == queryObj.ModelId.Value);

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName
            };
            result.TotalItems = await query.CountAsync();
            query = query.ApplyOrdering(queryObj, columnsMap);
            query = query.ApplyPaging(queryObj);
            result.Items =  await query.ToListAsync();
            return result;
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