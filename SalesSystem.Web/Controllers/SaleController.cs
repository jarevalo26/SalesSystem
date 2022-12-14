using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;
using SalesSystem.Web.Models;
using SalesSystem.Web.Utilities.Responses;

namespace SalesSystem.Web.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISalesDocumentTypeService _salesDocumentTypeService;
        private readonly ISalesService _salesService;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;

        public SaleController(
            ISalesDocumentTypeService salesDocumentTypeService,
            ISalesService salesService,
            IMapper mapper,
            IConverter converter)
        {
            _salesDocumentTypeService = salesDocumentTypeService;
            _salesService = salesService;
            _mapper = mapper;
            _converter = converter;
        }

        public IActionResult NewSale()
        {
            return View();
        }

        public IActionResult HistorySale()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SalesDocumentTypeList()
        {
            List<TipoDocumentoVentaViewModel> vmDocTypeList = _mapper.Map<List<TipoDocumentoVentaViewModel>>(await _salesDocumentTypeService.List());
            return StatusCode(StatusCodes.Status200OK, vmDocTypeList);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string search)
        {
            List<ProductoViewModel> vmProductList = _mapper.Map<List<ProductoViewModel>>(await _salesService.GetProducts(search));
            return StatusCode(StatusCodes.Status200OK, vmProductList);
        }

        [HttpPost]
        public async Task<IActionResult> SalesRegister([FromBody] VentaViewModel model)
        {
            GenericResponse<VentaViewModel> gResponse = new();
            try
            {
                model.IdUsuario = 1;
                Venta sale = await _salesService.Register(_mapper.Map<Venta>(model));
                model = _mapper.Map<VentaViewModel>(sale);
                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> SalesHistory(string salesNumber, string initialDate, string finalDate)
        {
            List<VentaViewModel> vmSalesHistory = _mapper.Map<List<VentaViewModel>>(await _salesService.SalesHistory(salesNumber, initialDate, finalDate));
            return StatusCode(StatusCodes.Status200OK, vmSalesHistory);
        }

        [HttpGet]
        public IActionResult ShowPdf(string saleNumber)
        {
            string urlPlantilla = $"{this.Request.Scheme}//{this.Request.Host}/Template/SalesPDF?saleNumber={saleNumber}";
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        Page = urlPlantilla
                    }
                }
            };

            var pdfFile = _converter.Convert(pdf);

            return File(pdfFile, "application/pdf");
        }
    }
}