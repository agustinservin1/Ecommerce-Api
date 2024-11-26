using Application.Models.Request;
using Application.Models;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<OrderDto> CreateOrder(CreateOrderRequest orderRequest)
        {
            var order = CreateOrderRequest.ToEntity(orderRequest);
            await _productRepository.Create(product);
            return ProductDto.CreateDto(product);
        }


    }
}
