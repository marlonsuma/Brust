using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using DonorTracking.Data;
using NiQ_Donor_Tracking_System.API.Models;
using Swashbuckle.Swagger.Annotations;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace NiQ_Donor_Tracking_System.API.Controllers
{
    [RoutePrefix("api/donor")]
    public class DonorController : NiqController
    {
        public const string RequestRoute = "RetrieveDonor";
        public const string ShippingRoute = "RetrieveDonorShippingAddress";
        public const string MailingRoute = "RetrieveDonorMailingAddress";
        private readonly IDonorRepository _donorRepository;
        private readonly IMapper _mapper;
        private readonly IMilkKitRepository _milkKitRepository;
        private readonly IBloodKitRepository _bloodKitRepository;
        private readonly IAddressRepository _addressRepository;

        public DonorController(IDonorRepository donorRepository, 
                               IMilkKitRepository milkKitRepository,
                               IBloodKitRepository bloodKitRepository,
                               IAddressRepository addressRepository,
                               IMapper mapper)
        {
            _addressRepository = addressRepository;
            _bloodKitRepository = bloodKitRepository;
            _milkKitRepository = milkKitRepository;
            _mapper = mapper;
            _donorRepository = donorRepository;
        }

        /// <summary>
        /// Get all donors
        /// </summary>
        //[SwaggerResponse(HttpStatusCode.OK, "Donor list", typeof(IEnumerable<DonorModel>))]
        //[SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<DonorModel>>(_donorRepository.Get(), MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured getting donor list: {ex.Message} ");
            }
        }

        /// <summary>
        /// Get selected donor
        /// </summary>
        /// <param name="donorId">Donor Id</param>
        [HttpGet]
        [Route("{donorId}", Name = RequestRoute)]
        [SwaggerResponse(HttpStatusCode.OK, "Requested donor", typeof(DonorDetailModel))]
        [SwaggerResponse(HttpStatusCode.NotFound, "donor not found", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        public IHttpActionResult Get(string donorId)
        {
            try
            {
                Donor donor = _donorRepository.GetWithKits(donorId);

                return donor == null
                    ? (IHttpActionResult) Content(HttpStatusCode.NotFound, $"Could not find donor {donorId}.")
                    : Ok(_mapper.Map<DonorDetailModel>(donor, MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured getting donor {donorId}: {ex.Message} ");
            }
        }

        /// <summary>
        /// Get donor's milk kits
        /// </summary>
        /// <param name="donorId">Donor Id</param>
        [SwaggerResponse(HttpStatusCode.OK, "Donor's Milk kits", typeof(IEnumerable<MilkKitModel>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error message", typeof(string))]

        [HttpGet]
        [Route("{donorId}/milkkit")]
        public IHttpActionResult GetDonorMilkKits(string donorId)
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<MilkKitModel>>(_milkKitRepository.GetByDonor(donorId), MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured getting donor {donorId} milk kits: {ex.Message} ");
            }
        }

        /// <summary>
        /// Get donor's blood kits
        /// </summary>
        /// <param name="donorId">Donor Id</param>
        [SwaggerResponse(HttpStatusCode.OK, "Donor's blood kits", typeof(IEnumerable<BloodKitModel>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        [HttpGet]
        [Route("{donorId}/bloodkit")]
        public IHttpActionResult GetDonorBloodKits(string donorId)
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<BloodKitModel>>(_bloodKitRepository.GetByDonor(donorId), MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured getting donor {donorId} blood kits: {ex.Message} ");
            }
        }

        /// <summary>
        /// Get donor's shipping address
        /// </summary>
        /// <param name="donorId">Donor Id</param>
        [SwaggerResponse(HttpStatusCode.OK, "Donor's shipping address", typeof(AddressModel))]
        [SwaggerResponse(HttpStatusCode.NotFound, "donor shipping address not found", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        [HttpGet]
        [Route("{donorId}/shippingaddress", Name = ShippingRoute)]
        public IHttpActionResult GetDonorShippingAddress(string donorId)
        {
            try
            {
                var address = _addressRepository.Get(donorId, AddressType.Shipping);

                return address == null ?
                     (IHttpActionResult)Content(HttpStatusCode.NotFound, $"Could not find donor shipping address {donorId}."):
                    Ok(_mapper.Map<AddressModel>(address, MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured in GetDonorShippingAddress: {ex.Message} ");
            }
        }

        /// <summary>
        /// Get donor's mailing address
        /// </summary>
        /// <param name="donorId">Donor Id</param>
        [SwaggerResponse(HttpStatusCode.OK, "Donor's mailing address", typeof(AddressModel))]
        [SwaggerResponse(HttpStatusCode.NotFound, "donor mailing address not found", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        [HttpGet]
        [Route("{donorId}/mailingaddress", Name = MailingRoute)]
        public IHttpActionResult GetDonorMailingAddress(string donorId)
        {
            try
            {
                var address = _addressRepository.Get(donorId, AddressType.Mailing);

                return address == null ?
                    (IHttpActionResult)Content(HttpStatusCode.NotFound, $"Could not find donor mailing address {donorId}.") :
                    Ok(_mapper.Map<AddressModel>(address, MapOptions));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured in GetDonorShippingAddress: {ex.Message} ");
            }
        }

        /// <summary>
        /// Create new donor
        /// </summary>
        /// <param name="donor">Donor information</param>
        [SwaggerResponse(HttpStatusCode.Created, "Donor created", typeof(DonorModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Donor exists", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        public IHttpActionResult Post([FromBody]DonorModel donor)
        {
            if (donor == null) return BadRequest("Invalid donor record");
            try
            {
                var content = System.Web.HttpContext.Current.Request.InputStream;

                byte[] buffer = new byte[16 * 1024];
                var bytes = new Byte[0];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = content.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    bytes = ms.ToArray();
                }
                //var bytes = content.ToArray();
                var jobj = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                JObject _donor = JObject.Parse(jobj);
                var dt = _donor.Value<String>("DateOfBirth");
                // hack
                donor.DateOfBirth = Convert.ToDateTime(dt);

                var existing = _donorRepository.Get(donor.DonorId);

               // donor.DateOfBirth = Convert.ToDateTime("12/23/2001");

                if (existing != null) return BadRequest("Donor already exists");
                Donor added = _mapper.Map<Donor>(donor);
                added.DonorId = donor.DonorId;
                added = _donorRepository.Add(added);

                return new CreatedNegotiatedContentResult<DonorModel>(
                    new Uri(Url.Link(RequestRoute, new {DonorId = added.DonorId})),
                    _mapper.Map<DonorModel>(added, MapOptions),
                    this);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured creating donor: {ex.Message} ");
            }
        }
        
        /// <summary>
        /// Add shipping address for donor
        /// </summary>
        /// <param name="donorId">Donor Id</param>
        /// <param name="shippingAddress">Shipping Address</param>
        [SwaggerResponse(HttpStatusCode.Created, "Shipping address created", typeof(DonorModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Shipping address exists", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        [HttpPost]
        [Route("{donorId}/shippingaddress")]
        public IHttpActionResult PostShippingAddress(string donorId, [FromBody]AddressModel shippingAddress)
        {
            if(shippingAddress == null) return BadRequest("Invalid address record");
            try
            {
                var existing = _addressRepository.Get(donorId, AddressType.Shipping);
                if (existing != null) return BadRequest("Shipping address already exists");
                Address added = _mapper.Map<Address>(shippingAddress);
                added.DonorId = donorId;
                added.AddressType = AddressType.Shipping;
                added = _addressRepository.Add(added);

                return new CreatedNegotiatedContentResult<AddressModel>(
                    new Uri(Url.Link(ShippingRoute, new { DonorId = added.DonorId })),
                    _mapper.Map<AddressModel>(added, MapOptions),
                    this);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured in PostShippingAddress: {ex.Message} ");
            }
        }

        /// <summary>
        /// Add mailing address for donor
        /// </summary>
        /// <param name="donorId">Donor Id</param>
        /// <param name="mailingAddress">Mailing Address</param>
        [SwaggerResponse(HttpStatusCode.Created, "Mailing address created", typeof(DonorModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Mailing address exists", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error occured", typeof(string))]
        [HttpPost]
        [Route("{donorId}/mailingaddress")]
        public IHttpActionResult PostMailingAddress(string donorId, [FromBody]AddressModel mailingAddress)
        {
            if (mailingAddress == null) return BadRequest("Invalid address record");
            try
            {
                var existing = _addressRepository.Get(donorId, AddressType.Mailing);
                if (existing != null) return BadRequest("Mailing address already exists");
                Address added = _mapper.Map<Address>(mailingAddress);
                added.DonorId = donorId;
                added.AddressType = AddressType.Mailing;
                added = _addressRepository.Add(added);

                return new CreatedNegotiatedContentResult<AddressModel>(
                    new Uri(Url.Link(MailingRoute, new { DonorId = added.DonorId })),
                    _mapper.Map<AddressModel>(added, MapOptions),
                    this);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured in PostMailingAddress: {ex.Message} ");
            }
        }

        /// <summary>
        /// Update existing donor
        /// </summary>
        /// <param name="donorId">Donor Id</param>
        /// <param name="donor">Updated donor</param>
        [SwaggerResponse(HttpStatusCode.OK, "Donor updated", typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Donor not found", typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotModified, "Donor update error", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error message", typeof(string))]
        [HttpPut]
        [Route("{donorId}")]
        public IHttpActionResult Put(string donorId, [FromBody]DonorModel donor)
        {
            if (donor == null) return BadRequest("Invalid donor record");
            try
            {
                Donor existing = _donorRepository.Get(donorId);
                if(existing == null) return Content(HttpStatusCode.NotFound, $"Could not find Donor {donorId}");
                _mapper.Map(donor, existing);
                Donor result = _donorRepository.Update(existing);
                if (result == null) return Content(HttpStatusCode.NotModified, $"Could not update donor {donorId}");

                return new OkNegotiatedContentResult<DonorModel>(_mapper.Map<DonorModel>(result, MapOptions),this);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured updating donor {donorId}: {ex.Message} ");
            }
        }

        /// <summary>
        /// Update donor shipping address
        /// </summary>
        /// <param name="donorId">Donor Id</param>
        /// <param name="shippingAddress">Shipping Address</param>
        [SwaggerResponse(HttpStatusCode.OK, "Shipping address updated", typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Shipping address not found", typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotModified, "Shipping address update error", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error message", typeof(string))]
        [HttpPut]
        [Route("{donorId}/shippingaddress")]
        public IHttpActionResult PutShippingAddress(string donorId, [FromBody]AddressModel shippingAddress)
        {
            if (shippingAddress == null) return BadRequest("Invalid address record");
            try
            {
                Address existing = _addressRepository.Get(donorId, AddressType.Shipping);
                if(existing == null) return Content(HttpStatusCode.NotFound, $"Could not find Donor shipping address {donorId}");
                _mapper.Map(shippingAddress, existing);
                Address result = _addressRepository.Update(existing);
                if(result == null) return Content(HttpStatusCode.NotModified, $"Could not update donor shipping address {donorId}");
                return new OkNegotiatedContentResult<AddressModel>(_mapper.Map<AddressModel>(result, MapOptions), this);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured in PutShippingAddress: {ex.Message} ");
            }
        }

        /// <summary>
        /// Update donor mailing address
        /// </summary>
        /// <param name="donorId">Donor Id</param>
        /// <param name="mailingAddress">Mailing Address</param>
        [SwaggerResponse(HttpStatusCode.OK, "Mailing address updated", typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Mailing address not found", typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotModified, "Mailing address update error", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Error message", typeof(string))]
        [HttpPut]
        [Route("{donorId}/mailingaddress")]
        public IHttpActionResult PutMailingAddress(string donorId, [FromBody]AddressModel mailingAddress)
        {
            if (mailingAddress == null) return BadRequest("Invalid address record");
            try
            {
                Address existing = _addressRepository.Get(donorId, AddressType.Mailing);
                if (existing == null) return Content(HttpStatusCode.NotFound, $"Could not find Donor mailing address {donorId}");
                _mapper.Map(mailingAddress, existing);
                Address result = _addressRepository.Update(existing);
                if (result == null) return Content(HttpStatusCode.NotModified, $"Could not update donor mailing address {donorId}");
                return new OkNegotiatedContentResult<AddressModel>(_mapper.Map<AddressModel>(result, MapOptions), this);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"An error occured in PutMailingAddress: {ex.Message} ");
            }
        }
    }
}