using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliVeggie.Microservice.Data.Contracts.Product;
using DeliVeggie.Microservice.Domain.Contracts.Messaging;
using DeliVeggie.Microservice.Domain.Contracts.PriceReduction;
using DeliVeggie.Microservice.Domain.Contracts.Product;
using DeliVeggie.Models.Request;
using DeliVeggie.Models.Response;

namespace DeliVeggie.Microservice.Domain.Product
{
    public class ProductService : IProductService
    {
        #region PRIVATE PROPERTIES
        private readonly IMessagingAdapter _messagingAdapter;
        private readonly IProductRepository _productRepository;
        private readonly IPriceReductionService _priceReductionService;
        #endregion

        #region CONSTRUCTOR
        public ProductService(IMessagingAdapter messagingAdapter, 
                              IProductRepository productRepository,
                              IPriceReductionService priceReductionService)
        {
            _messagingAdapter = messagingAdapter;
            _productRepository = productRepository;
            _priceReductionService = priceReductionService;
        }
        #endregion

        #region PUBLIC METHODS
        public void RespondToProductMessages()
        {
            _messagingAdapter.RespondAsync(HandleRequest);
        }

        public async Task InserProductsIfNotExist()
        {
            var isDocumentExist = await _productRepository.CheckIfDocumentExists();
            if (isDocumentExist)
            {
                return;
            }
            await _productRepository.SaveProductsAsync();
        }

        #endregion

        #region PRIVATE METHODS
        private IResponse HandleRequest(IRequest arg)
        {

            if (arg is Request<ProductDetailsRequest> detailsRequest)
            {
                Console.WriteLine($"Gateway sent a request to retrieve details of product with ID {detailsRequest.Data.Id}.");

                var productDetails = Task.Run(async () =>
                {
                    return await _productRepository.GetProductById(detailsRequest.Data.Id);

                }).GetAwaiter().GetResult();

                ApplyReduction(ref productDetails);

                IResponse response = new Response<ProductDetailsResponse>() { Data = productDetails };
                return response;
            }
            else
            {
                Console.WriteLine($"Gateway sent a request to retrieve all products.");

                var products = Task.Run(async () =>
                {
                    return await _productRepository.GetProductsAsync();

                }).GetAwaiter().GetResult();

                IResponse data = new Response<List<ProductResponse>>() { Data = products.ToList() };
                return data;
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void ApplyReduction(ref ProductDetailsResponse productDetails)
        {
            int currentDay = (int)DateTime.Now.DayOfWeek;

            var reduction = Task.Run(async () =>
            {
                return await _priceReductionService.GetPriceReductionByDayOfWeek(currentDay);

            }).GetAwaiter().GetResult();

            productDetails.PriceWithReduction -= reduction;
        }
        #endregion
    }
}
