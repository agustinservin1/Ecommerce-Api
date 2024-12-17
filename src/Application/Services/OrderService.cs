using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;

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
                Details = []
            };

            await _orderRepository.Create(order);


            return OrderDto.CreateDto(order);
        }
        public async Task<OrderDto> UpdateOrderStatus(int orderId, StatusOrder newStatus)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with id {orderId} does not exist.");
            }
            Console.WriteLine($"Current Status: {order.StatusOrder}, New Status: {newStatus}");
            ValidateOrderStatusTransition(order, newStatus);
            order.StatusOrder = newStatus;

            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                if (newStatus == StatusOrder.Canceled)
                {
                    foreach (var detail in order.Details)
                    {
                        detail.Product.Stock += detail.Quantity;
                    }
                    await _productRepository.UpdateRange(order.Details.Select(d => d.Product).ToList());
                }

                await _orderRepository.Update(order);
                Console.WriteLine($"Updated Status: {order.StatusOrder}");
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

        private void ValidateOrderStatusTransition(Order order, StatusOrder newStatus)
        {
            if (newStatus == StatusOrder.Completed && order.StatusOrder != StatusOrder.Pending)
            {
                throw new InvalidOperationException("Only pending orders can be confirmed.");
            }

            if (newStatus == StatusOrder.Canceled && (order.StatusOrder != StatusOrder.Pending && order.StatusOrder != StatusOrder.Completed))
            {
                throw new InvalidOperationException("Only completed or pending orders can be canceled.");
            }

            if (newStatus == StatusOrder.Completed && (order.Details == null || !order.Details.Any()))
            {
                throw new InvalidOperationException("Cannot confirm an order without order details.");
            }
        }


    }
}
