using System.Threading.Tasks;
using Vega.Core;

namespace Vega.Persistence
{
    public class UnitOfWork : IUnitOfWork {
        private VegaDbContext _context { get; }
        public UnitOfWork (VegaDbContext context) {
            this._context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}