using Application.Models.Request;
using Application.Models;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OrderService(IOrderRepository orderRepository,
                            IUserRepository userRepository,
                            IProductRepository productRepository,
                            IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task<OrderDto> CreateOrder(CreateOrderRequest orderRequest)
        {

            var user = await _userRepository.GetById(orderRequest.UserId);
            if (user == null)
            {
                throw new NotFoundException($"The category with id {orderRequest.UserId} that not exist ");
            }
            var orders = await _orderDetailRepository.GetAll();
            if (orders == null)
            {
                throw new NotFoundException();
            }
            var order = CreateOrderRequest.ToEntity(orderRequest, user, orders);
            await _orderRepository.Create(order);
            return OrderDto.CreateDto(order);
        }
        public async Task<OrderDto> GetOrderById(int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order == null)
            {
                throw new NotFoundException($"The order with id {id} that not exist ");
            }
            return OrderDto.CreateDto(order);
        }
        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAll();
            if (orders == null)
            {
                throw new NotFoundException("Orders", "All");
            }
            return OrderDto.CreateListDto(orders);
        }
        public async Task<OrderDto> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order == null)
            {
                throw new NotFoundException($"The order with id {id} that not exist ");
            }
            await _orderRepository.Delete(order);
            return OrderDto.CreateDto(order);
        }


    }
}
