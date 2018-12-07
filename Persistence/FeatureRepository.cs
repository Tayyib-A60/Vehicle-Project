using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vega.Core;
using Vega.Models;

namespace Vega.Persistence
{
    public class FeatureRepository: IFeatureRepository
    {
        private VegaDbContext _context { get; }
        public FeatureRepository (VegaDbContext context) {
            this._context = context;
        }
        public async Task<IEnumerable<Feature>> GetFeatures()
        {
            return await _context.Feature.ToListAsync();
        } 
        public void Add(Feature feature)
        {
            _context.Feature.Add(feature);
        }
    }
}