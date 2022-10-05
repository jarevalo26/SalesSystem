using AspNetCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;
using SalesSystem.Web.Models;
using SalesSystem.Web.Utilities.Responses;

namespace SalesSystem.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _product;

        public ProductController(
            IMapper mapper,
            IProductService product
            )
        {
            _mapper = mapper;
            _product = product;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<ProductoViewModel> vmProductList = _mapper.Map<List<ProductoViewModel>>(await _product.List());
            return StatusCode(StatusCodes.Status200OK, new { data = vmProductList });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] IFormFile image, [FromForm] string model)
        {
            GenericResponse<ProductoViewModel> gResponse = new GenericResponse<ProductoViewModel>();
            try
            {
                ProductoViewModel? vmProduct = JsonConvert.DeserializeObject<ProductoViewModel>(model);
                string? imageName = "";
                Stream? imageStream = null;
                if (image != null)
                {
                    string nameInCode = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(image.FileName);
                    imageName = string.Concat(nameInCode, extension);
                    imageStream = image.OpenReadStream();
                }

                Producto productCreated = await _product.Create(_mapper.Map<Producto>(vmProduct), imageStream, imageName);
                vmProduct = _mapper.Map<ProductoViewModel>(productCreated);
                gResponse.State = true;
                gResponse.Object = vmProduct;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] IFormFile image, [FromForm] string model)
        {
            GenericResponse<ProductoViewModel> gResponse = new GenericResponse<ProductoViewModel>();
            try
            {
                ProductoViewModel? vmProduct = JsonConvert.DeserializeObject<ProductoViewModel>(model);
                Stream? imageStream = null;
                if (image != null)
                {
                    imageStream = image.OpenReadStream();
                }

                Producto productEdited = await _product.Edit(_mapper.Map<Producto>(vmProduct), imageStream);
                vmProduct = _mapper.Map<ProductoViewModel>(productEdited);
                gResponse.State = true;
                gResponse.Object = vmProduct;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int productId)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.State = await _product.Delete(productId);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
}
