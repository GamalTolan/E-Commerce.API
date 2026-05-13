using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Service.Abstractions;
using Service.MappingProfiles;
using Shared.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<bool> DeleteBasketAsync(string basketId)
           => await basketRepository.DeleteBasketAsync(basketId);
        

        public async Task<BasketDto> GetBasketAsync(string basketId)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);
            return basket is null ? throw new BasketNotFoundException(basketId) : mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto> UpdateBasketAsync(BasketDto basket)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basket);
            var updatedBasket = await basketRepository.UpdateBasketAsync(customerBasket);
            return updatedBasket is null ? throw new Exception("Can Not Update Basket Now") : mapper.Map<BasketDto>(updatedBasket);
        }
    }
}
