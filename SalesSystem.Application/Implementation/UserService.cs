using Microsoft.EntityFrameworkCore;
using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Usuario> _repository;
        private readonly IFireBaseService _fireBase;
        private readonly IUtilitiesService _utilities;
        private readonly IMailService _mail;

        public UserService(
            IGenericRepository<Usuario> repository,
            IFireBaseService fireBase,
            IUtilitiesService utilities,
            IMailService mail)
        {
            _repository = repository;
            _fireBase = fireBase;
            _utilities = utilities;
            _mail = mail;
        }

        public async Task<List<Usuario>> GetAll()
        {
            IQueryable<Usuario> userList = await _repository.GetAll();
            return userList.Include(rol => rol.IdRolNavigation).ToList();
        }

        public async Task<Usuario> GetById(int UserId)
        {
            IQueryable<Usuario> query = await _repository.GetAll(u => u.IdUsuario == UserId);
            Usuario? user = query.Include(r => r.IdRolNavigation!).FirstOrDefault();
            return user!;
        }

        public async Task<Usuario> GetByCredentials(string email, string password)
        {
            string encryptedPass = _utilities.ConvertToSha256(password);
            Usuario user = await _repository.Get(u => u.Correo!.Equals(email) && u.Clave!.Equals(encryptedPass));
            return user;
        }

        public async Task<Usuario> Create(Usuario entity, Stream? photo = null, string? photoName = null, string? UrlEmailTemplate = null)
        {
            Usuario user = await _repository.Get(u => u.Correo == entity.Correo);
            if (user is not null)
                throw new TaskCanceledException("El correo ya existe.");

            try
            {
                string generatePassword = _utilities.GeneratePassword();
                entity.Clave = _utilities.ConvertToSha256(generatePassword);
                entity.NombreFoto = photoName;
                if (photo != null)
                {
                    string? urlPhoto = await _fireBase.UploadStorage(photo, "carpeta_usuario", photoName!);
                    entity.UrlFoto = urlPhoto;
                }

                Usuario newUser = await _repository.Create(entity);
                if (newUser.IdUsuario == 0)
                    throw new TaskCanceledException("No se pudo crear el usuario.");

                if (UrlEmailTemplate != null || UrlEmailTemplate != "")
                {
                    UrlEmailTemplate = UrlEmailTemplate!
                        .Replace("[correo]", newUser.Correo)
                        .Replace("[clave]", generatePassword);

                    string? htmlEmail = "";

                    using (HttpClient client = new())
                    {
                        HttpResponseMessage response = await client.GetAsync(UrlEmailTemplate);
                        if (response.IsSuccessStatusCode)
                        {
                            StreamReader? reader = null;
                            using Stream dataStream = await response.Content.ReadAsStreamAsync();
                            reader = new StreamReader(dataStream);
                            htmlEmail = reader.ReadToEnd();
                            reader.Close();
                        }

                    }

                    //HttpWebRequest? request = (HttpWebRequest)WebRequest.Create(UrlEmailTemplate);
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    //if (response.StatusCode == HttpStatusCode.OK)
                    //{
                    //    using Stream dataStream = response.GetResponseStream();
                    //    StreamReader? reader = null;
                    //    if (response.CharacterSet == null)
                    //        reader = new StreamReader(dataStream);
                    //    else
                    //        reader = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));

                    //    htmlEmail = reader.ReadToEnd();
                    //    response.Close();
                    //    reader.Close();
                    //}

                    if (!string.IsNullOrEmpty(htmlEmail))
                        await _mail.SendEmail(newUser.Correo!, "Cuenta Creada", htmlEmail);
                }

                IQueryable<Usuario> query = await _repository.GetAll(u => u.IdUsuario == newUser.IdUsuario);
                newUser = query.Include(r => r.IdRolNavigation).First();
                return newUser;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Usuario> Edit(Usuario entity, Stream? photo = null, string? photoName = null)
        {
            Usuario user = await _repository.Get(u => u.Correo == entity.Correo && u.IdUsuario != entity.IdUsuario);
            if (user is not null)
                throw new TaskCanceledException("El correo ya existe.");

            try
            {
                IQueryable<Usuario> queryUser = await _repository.GetAll(u => u.IdUsuario == entity.IdUsuario);
                Usuario userEdit = queryUser.First();
                userEdit.Nombre = entity.Nombre;
                userEdit.Correo = entity.Correo;
                userEdit.Telefono = entity.Telefono;
                userEdit.IdRol = entity.IdRol;
                userEdit.EsActivo = entity.EsActivo;
                if (userEdit.NombreFoto == "")
                    userEdit.NombreFoto = photoName;

                if (photo != null)
                {
                    string? photoUrl = await _fireBase.UploadStorage(photo, "carpeta_usuario", userEdit.NombreFoto!);
                    userEdit.UrlFoto = photoUrl;
                }

                bool response = await _repository.Update(userEdit);
                if (!response)
                    throw new TaskCanceledException("No se pudo modificar el usuario");

                Usuario newUser = queryUser.Include(r => r.IdRolNavigation).First();
                return newUser;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(int userId)
        {
            try
            {
                Usuario user = await _repository.Get(u => u.IdUsuario == userId);
                if (user is null)
                    throw new TaskCanceledException("El usuario no existe");

                string photoName = user.Nombre!;
                bool response = await _repository.Delete(user);
                if (response)
                    await _fireBase.DeleteStorage("carpeta_usuario", photoName);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SaveProfile(Usuario entity)
        {
            try
            {
                Usuario user = await _repository.Get(u => u.IdUsuario == entity.IdUsuario);
                if (user is null)
                    throw new TaskCanceledException("El usuario no existe");

                user.Correo = entity.Correo;
                user.Telefono = entity.Telefono;

                bool respose = await _repository.Update(user);
                return respose;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ChangePassword(int userId, string password, string newPassword)
        {
            try
            {
                Usuario user = await _repository.Get(u => u.IdUsuario == userId);
                if (user is null)
                    throw new TaskCanceledException("El usuario no existe");

                if (user.Clave != _utilities.ConvertToSha256(password))
                    throw new TaskCanceledException("Contraseña ingresada no es correcta");

                user.Clave = _utilities.ConvertToSha256(newPassword);
                bool response = await _repository.Update(user);
                return response;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RestorePassword(string email, string UrlEmailTemplate)
        {
            try
            {
                Usuario user = await _repository.Get(u => u.Correo == email);
                if (user is null)
                    throw new TaskCanceledException("No se encuentra usuario asociado al email.");

                string newPass = _utilities.GeneratePassword();
                user.Clave = _utilities.ConvertToSha256(newPass);

                UrlEmailTemplate = UrlEmailTemplate!.Replace("[clave]", newPass);

                string? htmlEmail = "";

                using (HttpClient client = new())
                {
                    HttpResponseMessage response = await client.GetAsync(UrlEmailTemplate);
                    if (response.IsSuccessStatusCode)
                    {
                        StreamReader? reader = null;
                        using Stream dataStream = await response.Content.ReadAsStreamAsync();
                        reader = new StreamReader(dataStream);
                        htmlEmail = reader.ReadToEnd();
                        reader.Close();
                    }

                }
                bool sendedEmail = false;

                if (string.IsNullOrEmpty(htmlEmail))
                    sendedEmail = await _mail.SendEmail(email, "Contraseña Restablecida", htmlEmail);

                if (!sendedEmail)
                    throw new TaskCanceledException("Tenemos problemas. Por favor intentalo más tarde.");

                bool result = await _repository.Update(user);
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
