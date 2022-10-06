using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Producto> _repository;
        private readonly IFireBaseService _firebase;

        public ProductService(
            IGenericRepository<Producto> repository,
            IFireBaseService firebase)
        {
            _repository = repository;
            _firebase = firebase;
        }

        public async Task<List<Producto>> List()
        {
            IQueryable<Producto> query = await _repository.GetAll();
            return query.Include(c => c.IdCategoriaNavigation).ToList();
        }

        public async Task<Producto> Create(Producto entity, Stream? image = null, string imageName = "")
        {
            Producto product = await _repository.Get(p => p.CodigoBarra == entity.CodigoBarra);
            if (product != null)
                throw new TaskCanceledException("El codigo de barras ya existe.");

            try
            {
                entity.NombreImagen = imageName;
                if(image != null)
                {
                    string? imageUrl = await _firebase.UploadStorage(image, "carpeta_producto", imageName);
                    entity.UrlImagen = imageUrl;
                }

                Producto newProduct = await _repository.Create(entity);
                if (newProduct.IdProducto == 0)
                    throw new TaskCanceledException("No se pudo crear el producto.");

                IQueryable<Producto> query = await _repository.GetAll(p => p.IdProducto == newProduct.IdProducto);
                newProduct = query.Include(c => c.IdCategoriaNavigation).First();
                return newProduct;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Producto> Edit(Producto entity, Stream? image = null, string imageName = "")
        {
            Producto product = await _repository.Get(p => p.CodigoBarra == entity.CodigoBarra && p.IdProducto != entity.IdProducto);
            if (product !=  null)
                throw new TaskCanceledException("El codigo de barras ya existe.");

            try
            {
                IQueryable<Producto> query = await _repository.GetAll(p => p.IdProducto == entity.IdProducto);
                Producto productEdited = query.First();
                productEdited.Marca = entity.Marca;
                productEdited.Descripcion = entity.Descripcion;
                productEdited.IdCategoria = entity.IdCategoria;
                productEdited.Stock = entity.Stock;
                productEdited.Precio = entity.Precio;
                productEdited.EsActivo = entity.EsActivo;

                if (productEdited.NombreImagen == "")
                {
                    productEdited.NombreImagen = imageName;
                }

                if (image != null)
                {
                    string? imageUrl = await _firebase.UploadStorage(image, "carpeta_producto", productEdited.NombreImagen!);
                    productEdited.UrlImagen = imageUrl;
                }

                bool response = await _repository.Update(productEdited);
                if (!response)
                    throw new TaskCanceledException("No se pudo editar el producto.");

                Producto newProduct = query.Include(c => c.IdCategoriaNavigation).First();
                return newProduct;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(int productId)
        {
            try
            {
                Producto product = await _repository.Get(p => p.IdProducto == productId);
                if (product == null)
                    throw new TaskCanceledException("El producto no existe");

                string? imageName = product.NombreImagen;
                bool response = await _repository.Delete(product);
                if (response)
                    await _firebase.DeleteStorage("carpeta_producto", imageName!);

                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}
