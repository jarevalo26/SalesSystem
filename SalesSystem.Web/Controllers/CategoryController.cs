using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;
using SalesSystem.Web.Models;
using SalesSystem.Web.Utilities.Responses;

namespace SalesSystem.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _service;

        public CategoryController(IMapper mapper, ICategoryService service)
        {
            _mapper = mapper;
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<CategoriaViewModel> vmCategories = _mapper.Map<List<CategoriaViewModel>>(await _service.List());
            return StatusCode(StatusCodes.Status200OK, new {data = vmCategories});
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CategoriaViewModel model)
        {
            GenericResponse<CategoriaViewModel> gResponse = new();
            try
            {
                Categoria category = await _service.Create(_mapper.Map<Categoria>(model));
                model = _mapper.Map<CategoriaViewModel>(category);
                gResponse.State = true;
                gResponse.Object = model;
            }
            catch(Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }
            
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] CategoriaViewModel model)
        {
            GenericResponse<CategoriaViewModel> gResponse = new();
            try
            {
                Categoria category = await _service.Edit(_mapper.Map<Categoria>(model));
                model = _mapper.Map<CategoriaViewModel>(category);
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

        [HttpDelete]
        public async Task<IActionResult> Delete(int categoryId)
        {
            GenericResponse<string> gResponse = new();
            try
            {
                gResponse.State = await _service.Delete(categoryId);
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
