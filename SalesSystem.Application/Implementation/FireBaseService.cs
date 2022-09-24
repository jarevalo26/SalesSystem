using Firebase.Auth;
using Firebase.Storage;
using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.Application.Implementation
{
    public class FireBaseService : IFireBaseService
    {
        private readonly IGenericRepository<Configuracion> _repository;

        public FireBaseService(IGenericRepository<Configuracion> repository)
        {
            _repository = repository;
        }

        public async Task<string?> UploadStorage(Stream streamFile, string destinationFolder, string fileName)
        {
            string? imageUrl = null;
            try
            {
                IQueryable<Configuracion> query = await _repository.GetAll(c => c.Recurso!.Equals("FireBase_Storage"));
                Dictionary<string, string> config = query.ToDictionary(keySelector: c => c.Propiedad!, elementSelector: c => c.Valor!);
                var auth = new FirebaseAuthProvider(new FirebaseConfig(config["api_key"]));
                var signIn = await auth.SignInWithEmailAndPasswordAsync(config["email"], config["clave"]);
                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    config["ruta"], 
                    new FirebaseStorageOptions { 
                        AuthTokenAsyncFactory = () => Task.FromResult(signIn.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(config[destinationFolder])
                    .Child(fileName)
                    .PutAsync(streamFile, cancellation.Token);

                imageUrl = await task!;
                return imageUrl!;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteStorage(string destinationFolder, string fileName)
        {
            try
            {
                IQueryable<Configuracion> query = await _repository.GetAll(c => c.Recurso!.Equals("FireBase_Storage"));
                Dictionary<string, string> config = query.ToDictionary(keySelector: c => c.Propiedad!, elementSelector: c => c.Valor!);
                var auth = new FirebaseAuthProvider(new FirebaseConfig(config["api_key"]));
                var signIn = await auth.SignInWithEmailAndPasswordAsync(config["email"], config["clave"]);
                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    config["ruta"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(signIn.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(config[destinationFolder])
                    .Child(fileName)
                    .DeleteAsync();

                await task;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
