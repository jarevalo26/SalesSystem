using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;
using SalesSystem.Web.Models;
using SalesSystem.Web.Utilities.Responses;

namespace SalesSystem.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _user;
        private readonly IRoleService _rol;
        private readonly IMapper _maper;

        public UserController(
            IUserService user,
            IRoleService rol,
            IMapper maper)
        {
            _user = user;
            _rol = rol;
            _maper = maper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Roles() {
            List<RolViewModel> rolesModel = _maper.Map<List<RolViewModel>>(await _rol.Roles());
            return StatusCode(StatusCodes.Status200OK, rolesModel);
        }

        [HttpGet]
        public async Task<IActionResult> Usuarios()
        {
            List<UsuarioViewModel> usersModel = _maper.Map<List<UsuarioViewModel>>(await _user.GetAll());
            return StatusCode(StatusCodes.Status200OK, new { data = usersModel });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] IFormFile photo, [FromForm] string model)
        {
            GenericResponse<UsuarioViewModel> gResponse = new GenericResponse<UsuarioViewModel>();
            try
            {
                UsuarioViewModel? userModel = JsonConvert.DeserializeObject<UsuarioViewModel>(model);
                string photoName = "";
                Stream? photoStream = null;
                if (photo != null)
                {
                    string nameInCode = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(photo.FileName);
                    photoName = string.Concat(nameInCode, extension);
                    photoStream = photo.OpenReadStream();
                }

                string urlMailTemplate = $"{this.Request.Scheme}://{this.Request.Host}/Template/SendPassword?email=[correo]&password=[clave]";
                Usuario newUser = await _user.Create(_maper.Map<Usuario>(userModel), photoStream, photoName, urlMailTemplate);
                userModel = _maper.Map<UsuarioViewModel>(newUser);

                gResponse.State = true;
                gResponse.Object = userModel;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] IFormFile photo, [FromForm] string model)
        {
            GenericResponse<UsuarioViewModel> gResponse = new GenericResponse<UsuarioViewModel>();
            try
            {
                UsuarioViewModel? userModel = JsonConvert.DeserializeObject<UsuarioViewModel>(model);
                string photoName = "";
                Stream? photoStream = null;
                if (photo != null)
                {
                    string nameInCode = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(photo.FileName);
                    photoName = string.Concat(nameInCode, extension);
                    photoStream = photo.OpenReadStream();
                }

                Usuario userEdit = await _user.Edit(_maper.Map<Usuario>(userModel), photoStream, photoName);
                userModel = _maper.Map<UsuarioViewModel>(userEdit);

                gResponse.State = true;
                gResponse.Object = userModel;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int userId)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.State = await _user.Delete(userId);
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
