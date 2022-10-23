using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using DonorTracking.Data;
using NiQ_Donor_Tracking_System.API.Models;
using Swashbuckle.Swagger.Annotations;

namespace NiQ_Donor_Tracking_System.API.Controllers
{
    [RoutePrefix("api/milkkit")]
    public class MilkKitController : NiqController
    {
        private readonly IMilkKitRepository _milkKitRepository;
        private readonly IMapper _mapper;
        public const string RequestRoute = "RetrieveMilkKit";

        
        
        public MilkKitController(IMilkKitRepository milkKitRepository, IMapper mapper)
        {
            _mapper = mapper;
            _milkKitRepository = milkKitRepository;
        }
        /// <summary>
        /// Get all milk kits
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,"collection of all milk kits", typeof(IEnumerable<MilkKitModel>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<MilkKitModel>>(_milkKitRepository.GetWithLot(), MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured retrieving the milk kits: {ex.Message} ");
            }
        }

        /// <summary>
        /// Get milk kit
        /// </summary>
        /// <param name="barcode">Milk kit barcode</param>
        [HttpGet]
        [Route("{barcode}", Name = RequestRoute)]
        [SwaggerResponse(HttpStatusCode.OK, "requested milk kit", typeof(MilkKitModel))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Milk kit not found", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]

        public IHttpActionResult Get(string barcode)
        {
            try
            {
                MilkKit kit = _milkKitRepository.GetWithLot(barcode);

                return kit == null
                    ? (IHttpActionResult) Content(HttpStatusCode.NotFound, $"Could not find Milk Kit {barcode}"):
                    Ok(_mapper.Map<MilkKitModel>(kit, MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured retrieving the milk kit {barcode}: {ex.Message} ");
            }
        }

        /// <summary>
        /// Create new milk kit
        /// </summary>
        /// <param name="milkKit">Milk kit to be added</param>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created, "Milk kit created", typeof(MilkKitModel))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        public IHttpActionResult Post([FromBody]MilkKitModel milkKit)
        {
            try
            {
                if (!string.IsNullOrEmpty(milkKit.Barcode))
                {
                    MilkKit existing = _milkKitRepository.Get(milkKit.Barcode);

                    if (existing != null)
                    {
                        return BadRequest("Milk Kit already exists");
                    }
                }

                MilkKit added = _mapper.Map<MilkKit>(milkKit);
                added.Active = true;
                added = _milkKitRepository.Add(added);
                added.Barcode = $"MK{added.Id.ToString().PadLeft(7, '0')}";
                added = _milkKitRepository.Update(added);

                return new CreatedNegotiatedContentResult<MilkKitModel>(
                    new Uri(Url.Link(RequestRoute, new { barcode = added.Barcode })),
                    _mapper.Map<MilkKitModel>(added, MapOptions),
                    this);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured creating a new milk kit: {ex.Message} ");
            }
        }

        /// <summary>
        /// Updates existing milk kit
        /// </summary>
        /// <param name="barcode">Milk kit barcode</param>
        /// <param name="milkKit">Milk kit to be updated</param>
        [HttpPut]
        [Route("{barcode}")]
        [SwaggerResponse(HttpStatusCode.OK, "Milk Kit Updated", typeof(MilkKitModel))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Milk kit not found", typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotModified, "Error occured", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        public IHttpActionResult Put(string barcode, [FromBody]MilkKitModel milkKit)
        {
            try
            {
                MilkKit existing = _milkKitRepository.Get(barcode);
                if (existing == null) return Content(HttpStatusCode.NotFound, $"Could not find Milk Kit {barcode}");

                _mapper.Map(milkKit, existing);
                MilkKit result = _milkKitRepository.Update(existing);
                if (result == null) return Content(HttpStatusCode.NotModified, $"Could not update Milk Kit {barcode}");

                return Ok(_mapper.Map<MilkKitModel>(result, MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured updating Milk Kit {barcode}: {ex.Message} ");
            }
        }

        /// <summary>
        /// Set milk kit as inactive
        /// </summary>
        /// <param name="barcode">Milk kit barcode</param>
        [HttpDelete]
        [Route("{barcode}")]
        [SwaggerResponse(HttpStatusCode.OK, "Milk kit deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Milk kit not found", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        public IHttpActionResult Delete(string barcode)
        {
            try
            {
                MilkKit existing = _milkKitRepository.Get(barcode);
                if (existing == null) return Content(HttpStatusCode.NotFound, $"Could not find Milk Kit {barcode}");

                existing.Active = false;
                _milkKitRepository.Update(existing);

                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured deactivating milk kit {barcode}: {ex.Message} ");
            }
        }
    }
}