using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;
using SalesSystem.Web.Models;
using SalesSystem.Web.Utilities.Responses;

namespace SalesSystem.Web.Controllers
{
    public class BusinessController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBusinessService _business;

        public BusinessController(IMapper mapper, IBusinessService business)
        {
            _mapper = mapper;
            _business = business;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            GenericResponse<NegocioViewModel> gResponse = new();
            try
            {
                NegocioViewModel vmBusiness = _mapper.Map<NegocioViewModel>(await _business.Get());
                gResponse.State = true;
                gResponse.Object = vmBusiness;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPost]
        public async Task<IActionResult> SaveChanges([FromForm]IFormFile logo, [FromForm]string model)
        {
            GenericResponse<NegocioViewModel> gResponse = new();
            try
            {
                NegocioViewModel? vmBusiness = JsonConvert.DeserializeObject<NegocioViewModel>(model);
                string? logoName = "";
                Stream? stream = null;

                if (logo != null)
                {
                    string name = Guid.NewGuid().ToString("N");
                    string ext = Path.GetExtension(logo.FileName);
                    logoName = string.Concat(name, ext);
                    stream = logo.OpenReadStream();
                }

                Negocio editBusiness = await _business.SaveChanges(_mapper.Map<Negocio>(vmBusiness), stream, logoName);
                vmBusiness = _mapper.Map<NegocioViewModel>(editBusiness);

                gResponse.State = true;
                gResponse.Object = vmBusiness;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
