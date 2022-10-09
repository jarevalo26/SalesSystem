using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Application.Interfaces;
using SalesSystem.Web.Models;

namespace SalesSystem.Web.Controllers
{
    public class TemplateController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBusinessService _businessService;
        private readonly ISalesService _salesService;

        public TemplateController(
            IMapper mapper,
            IBusinessService businessService,
            ISalesService salesService)
        {
            _mapper = mapper;
            _businessService = businessService;
            _salesService = salesService;
        }

        public IActionResult SendPassword(string email, string password)
        {
            ViewData["Correo"] = email;
            ViewData["Clave"] = password;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";
            return View();
        }

        public async Task<IActionResult> SalesPDF(string saleNumber)
        {
            VentaViewModel vmSale= _mapper.Map<VentaViewModel>(await _salesService.Details(saleNumber));
            NegocioViewModel vmBusiness = _mapper.Map<NegocioViewModel>(await _businessService.Get());
            PdfVentaViewModel model = new();
            model.Negocio = vmBusiness;
            model.Venta = vmSale;

            return View(model);
        }

        public IActionResult RestorePassword(string password)
        {
            ViewData["Password"] = password;
            return View();
        }
    }
}
