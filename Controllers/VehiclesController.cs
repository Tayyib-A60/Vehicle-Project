using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vega.Controllers.Resources;
using Vega.Models;
using Vega.Core;
using Vega.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace Vega.Controllers {
    [Route ("/api/vehicles")]
    [ApiController]
    public class VehiclesController : Controller {
        private IMapper _mapper { get; }
        private IVehicleRepository _repository { get; }
        private IUnitOfWork _unitOfWork { get; }
        public VehiclesController (IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork) 
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._mapper = mapper;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateVehicle (SaveVehicleResource vehicleResource) {            
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            
            var vehicle = _mapper.Map<SaveVehicleResource, Vehicle> (vehicleResource);
            vehicle.LastUpdate = DateTime.Now;
            _repository.Add (vehicle);
            
            await _unitOfWork.CompleteAsync ();

            vehicle = await _repository.GetVehicle (vehicle.Id);

            var result = _mapper.Map<Vehicle, VehicleResource> (vehicle);
            return Ok (result);
        }

        [HttpPut ("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateVehicle (int id, SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            var vehicle = await _repository.GetVehicle (id);

            if (vehicle == null)
                return NotFound ("Not found");

            _mapper.Map<SaveVehicleResource, Vehicle> (vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;
            await _unitOfWork.CompleteAsync ();

            
            vehicle = await _repository.GetVehicle (vehicle.Id);
            var result = _mapper.Map<Vehicle, VehicleResource> (vehicle);
            return Ok (result);
        }

        [HttpDelete ("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle (int id) {
            var vehicle = await _repository.GetVehicle (id, includeRelated : true);

            if (vehicle == null)
                return NotFound ("Not found");

            _repository.Remove (vehicle);
            await _unitOfWork.CompleteAsync ();

            return Ok (id);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetVehicle (int id) {
            var vehicle = await _repository.GetVehicle (id);

            if (vehicle == null)
                return NotFound ("Not found");

            var vehicleResource = _mapper.Map<Vehicle, VehicleResource> (vehicle);
            return Ok (vehicleResource);
        }

         [HttpGet("test")]
        public IActionResult GetDummy (int abc) 
        {
            return Ok(abc);
        }

        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles ([FromQuery] VehicleQueryResource filterResource) 
        {
            var filter = _mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);
            var queryResult = await _repository.GetVehicles(filter);
            return _mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(queryResult);
        }
    }
}