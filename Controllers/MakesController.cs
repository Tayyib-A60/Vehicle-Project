using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vega.Models;
using Vega.Persistence;
using Vega.Controllers.Resources;

namespace Vega.Controllers {
    public class MakesController : Controller {
        private VegaDbContext _context { get; }
        
        private IMapper _mapper { get; }
        public MakesController (VegaDbContext context, IMapper mapper) {
            this._mapper = mapper;
            this._context = context;
        }

        [HttpGet ("/api/makes")]
        public async Task<IEnumerable<MakeResource>> GetMakes () {
            var makes = await _context.Makes.Include (m => m.Models).ToListAsync ();
            return _mapper.Map<List<Make>, List<MakeResource>>(makes);
        }
    }
}