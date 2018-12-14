using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Vega.Controllers.Resources;
using Vega.Core;
using Vega.Core.Models;

namespace Vega.Controllers {
    [Route ("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller {
        private readonly PhotoSettings photoSettings;
        private IHostingEnvironment _host { get; }
        private IVehicleRepository _repository { get; }
        private IUnitOfWork _uow { get; }
        private IMapper _mapper { get; }
        private IPhotoRepository _photoRepository { get; }
        public PhotosController (IHostingEnvironment host, IVehicleRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<PhotoSettings> options, IPhotoRepository photoRepository) {
            this._photoRepository = photoRepository;
            this.photoSettings = options.Value;
            this._host = host;
            this._repository = repository;
            this._uow = unitOfWork;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {
            var photos = await _photoRepository.GetPhotos(vehicleId);
            return _mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpPost]
        public async Task<IActionResult> Upload (int vehicleId, IFormFile file) {
            var vehicle = await _repository.GetVehicle (vehicleId, includeRelated : false);
            if (vehicle == null) return NotFound ("Not Found");
            if (file == null) return BadRequest ("Null file");
            if (file.Length == 0) return BadRequest ("Empty file");
            if (file.Length > photoSettings.MaxBytes) return BadRequest ("Maximum file size exceeded");
            if (!photoSettings.isSupported (file.FileName)) return BadRequest ("Invalid file type.");

            var uploadsFolderPath = Path.Combine (_host.WebRootPath, "uploads");
            if (!Directory.Exists (uploadsFolderPath))
                Directory.CreateDirectory (uploadsFolderPath);

            var fileName = Guid.NewGuid ().ToString () + Path.GetExtension (file.FileName);
            var filePath = Path.Combine (uploadsFolderPath, fileName);

            using (var stream = new FileStream (filePath, FileMode.Create)) {
                await file.CopyToAsync (stream);
            }
            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add (photo);
            await _uow.CompleteAsync ();
            return Ok (_mapper.Map<Photo, PhotoResource> (photo));
        }
    }
}