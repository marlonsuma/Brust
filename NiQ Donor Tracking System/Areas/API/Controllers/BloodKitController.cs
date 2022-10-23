using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using DonorTracking.Data;
using NiQ_Donor_Tracking_System.API.Models;

namespace NiQ_Donor_Tracking_System.API.Controllers
{
    [RoutePrefix("api/bloodkit")]
    public class BloodKitController : NiqController
    {
        private readonly IBloodKitRepository _bloodKitRepository;
        private readonly IMapper _mapper;
        private readonly IDinGenerator _dinGenerator;
        public const string RequestRoute = "RetrieveBloodKit";

        public BloodKitController(IBloodKitRepository bloodKitRepository, IDinGenerator dinGenerator, IMapper mapper)
        {
            _dinGenerator = dinGenerator;
            _mapper = mapper;
            _bloodKitRepository = bloodKitRepository;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<BloodKitModel>>(_bloodKitRepository.Get(), MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured retrieving the blood kits: {ex.Message} ");
            }
        }

        [HttpGet]
        [Route("{din}", Name = RequestRoute)]
        public IHttpActionResult Get(string din)
        {
            try
            {
                BloodKit kit = _bloodKitRepository.Get(din);

                return kit == null ? 
                    (IHttpActionResult) Content(HttpStatusCode.NotFound, $"Could not find Blood Kit {din}.") :
                    Ok(_mapper.Map<BloodKitModel>(kit, MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured retrieving the blood kit {din}: {ex.Message} ");
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]BloodKitModel bloodKit)
        {
            try
            {
                if (!string.IsNullOrEmpty(bloodKit.Din))
                {
                    BloodKit existing = _bloodKitRepository.Get(bloodKit.Din);

                    if (existing != null)
                    {
                        return BadRequest("Blood Kit already exists");
                    }
                }
                BloodKit added = _mapper.Map<BloodKit>(bloodKit);
                added.Din = _dinGenerator.Generate();
                added.OrderDate = DateTime.Now;
                added.Active = true;
                added = _bloodKitRepository.Add(added);

                if (added == null)
                {
                    return BadRequest("Blood Kit could not be created.");
                }

                return new CreatedNegotiatedContentResult<BloodKitModel>(
                    new Uri(Url.Link(RequestRoute, new {din = added.Din})),
                    _mapper.Map<BloodKitModel>(added, MapOptions),
                    this);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured creating a new Blood Kit: {ex.Message} ");
            }
        }

        [HttpPut]
        [Route("{din}")]
        public IHttpActionResult Put(string din, [FromBody]BloodKitModel bloodKit)
        {
            try
            {
                BloodKit existing = _bloodKitRepository.Get(din);
                if (existing == null) return Content(HttpStatusCode.NotFound, $"Could not find Blood Kit {din}");
                
                _mapper.Map(bloodKit, existing);
                BloodKit result = _bloodKitRepository.Update(existing);
                if (result == null) return Content(HttpStatusCode.NotModified, $"Could not update Blood Kit {din}");
                
                return Ok(_mapper.Map<BloodKitModel>(result, MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured updating Blood Kit {din}: {ex.Message} ");
            }
        }
        
        [HttpDelete]
        [Route("{din}")]
        public IHttpActionResult Delete(string din)
        {
            try
            {
                BloodKit existing = _bloodKitRepository.Get(din);
                if (existing == null) return Content(HttpStatusCode.NotFound, $"Could not find Blood Kit {din}");
                
                existing.Active = false;
                _bloodKitRepository.Update(existing);

                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured deactivating blood kit {din}: {ex.Message} ");
            }
        }
    }
}