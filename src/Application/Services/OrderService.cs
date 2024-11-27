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
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IOrderRepository orderRepository,
                            IUserRepository userRepository,
                            IProductRepository productRepository,
                            IOrderDetailRepository orderDetailRepository,
                            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<OrderDto> CreateOrder(CreateOrderRequest orderRequest)
        {

            var user = await _userRepository.GetById(orderRequest.UserId);
            if (user == null)
            {
                throw new NotFoundException($"User with id {orderRequest.UserId} does not exist.");
            }

            var order = new Order
            {
                User = user,
                DateTime = DateTime.UtcNow,
                StatusOrder = StatusOrder.Pending,
                TotalPrice = 0,
                Details = new List<OrderDetail>()
            };

            await _orderRepository.Create(order);


            return OrderDto.CreateDto(order);
        }
        public async Task<OrderDto> ConfirmOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {orderId} does not exist.");
            }

            if (order.StatusOrder != StatusOrder.Pending)
            {
                throw new InvalidOperationException("Only pending orders can be confirmed.");
            }

            if (order.Details == null || !order.Details.Any())
            {
                throw new InvalidOperationException("Cannot confirm an order without order details.");
            }

            order.StatusOrder = StatusOrder.Completed; //MODIFICAR NOMBRE ENUM

            await _orderRepository.Update(order);

            return OrderDto.CreateDto(order);
        }
        public async Task<OrderDto> CancelOrder(int orderId)
        {
            //orden, detalles y productos en una sola consulta
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {orderId} does not exist.");
            }

            if (order.StatusOrder != StatusOrder.Completed && order.StatusOrder != StatusOrder.Pending)
            {
                throw new InvalidOperationException("Only completed or pending orders can be canceled.");
            }

            order.StatusOrder = StatusOrder.Canceled;
            // Recargar stock 
            foreach (var detail in order.Details)
            {
                var product = detail.Product;
                product.Stock += detail.Quantity;
            }

          
            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _productRepository.UpdateRange(order.Details.Select(d => d.Product).ToList());
                await _orderRepository.Update(order);
            });

            return OrderDto.CreateDto(order);
        }


        public async Task<OrderDto> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                throw new NotFoundException($"The order with id {id} that not exist ");
            }
            return OrderDto.CreateDto(order);
        }
        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
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
