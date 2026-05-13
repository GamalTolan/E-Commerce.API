using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Abstractions;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IPaymentService> _paymentService;
        private readonly Lazy<ICashService> _cashService;



        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository, UserManager<User> userManager,IOptions<JwtOptions> options, IConfiguration configuration,ICashRepository cashRepository) 
        {
            _productService = new Lazy<IProductService>(new ProductService(unitOfWork, mapper));
            _basketService = new Lazy<IBasketService>(new BasketService(basketRepository, mapper));
            _authenticationService = new Lazy<IAuthenticationService>(new AuthenticationService(userManager,mapper,options));
            _orderService = new Lazy<IOrderService>(new OrderService(unitOfWork, mapper, basketRepository));
            _paymentService = new Lazy<IPaymentService>(new PaymentService(unitOfWork, basketRepository,mapper,configuration));
            _cashService = new Lazy<ICashService>(new CashService(cashRepository));
        }
        public IProductService ProductService => _productService.Value;
        public IBasketService BasketService => _basketService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IOrderService OrderService => _orderService.Value;
        public IPaymentService PaymentService => _paymentService.Value;
        public ICashService CashService => _cashService.Value;

    }
}
 