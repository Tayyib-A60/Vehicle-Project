using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vega.Controllers.Resources;
using Microsoft.AspNetCore.Authorization;
using Vega.Core;
using Vega.Models;
using Vega.Persistence;

namespace Vega.Controllers {
    [Route ("/api/features")]
    [ApiController]
    public class FeaturesController : Controller {
        private VegaDbContext _context { get; }
        private IMapper _mapper { get; }
        private IFeatureRepository _repository { get; }
        private IUnitOfWork _unitOfWork { get; }
        public FeaturesController (IFeatureRepository repository, IMapper mapper, IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<FeatureResource>> GetFeature () {
            var features = await _repository.GetFeatures ();
            return _mapper.Map<IEnumerable<Feature>, IEnumerable<FeatureResource>> (features);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeature (FeatureResource featureResource) {
            var feature = _mapper.Map<FeatureResource, Feature> (featureResource);
            _repository.Add (feature);
            await _unitOfWork.CompleteAsync();
            var result = _mapper.Map<Feature,FeatureResource>(feature);
            return Ok(result);
        }
    }
}