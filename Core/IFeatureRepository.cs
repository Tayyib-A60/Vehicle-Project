using System.Collections.Generic;
using System.Threading.Tasks;
using Vega.Models;

namespace Vega.Core
{
    public interface IFeatureRepository
    {
         Task<IEnumerable<Feature>> GetFeatures();
         void Add(Feature feature);
    }
}