using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services
{
    public class OrderDetailService : IOrderDetailService
    {
            private readonly IOrderDetailRepository _orderDetailRepository;
            private readonly IProductRepository _productRepository;
            private readonly IOrderRepository _orderRepository;
            private readonly IUnitOfWork _unitOfWork;

            public OrderDetailService(IOrderDetailRepository orderDetailRepository,
                                      IProductRepository productRepository,
                                      IOrderRepository orderRepository,
                                      IUnitOfWork unitOfWork
                )
            {
                _orderDetailRepository = orderDetailRepository;
                _productRepository = productRepository;
                _orderRepository = orderRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<IEnumerable<OrderDetailDto>> GetAllOrderDetails()
            {
                var orderDetails = await _orderDetailRepository.GetAllOrderDetailsAsync();
                return OrderDetailDto.CreateListDto(orderDetails);
            }

            public async Task<OrderDetailDto> GetOrderDetailById(int id)
            {
                var orderDetail = await _orderDetailRepository.GetByIdOrderDetails(id);
                if (orderDetail == null)
                {
                    throw new NotFoundException($"OrderDetail with id {id} does not exist");
                }
                return OrderDetailDto.CreateDto(orderDetail);
            }

        public async Task<OrderDetailDto> CreateOrderDetail(CreateOrderDetailRequest request, int orderId)
        {
            
            if (!await _orderRepository.Exists(orderId))
            {
                throw new NotFoundException($"Order with id {orderId} does not exist.");
            }
            var order = await _orderRepository.GetOrderById(orderId);
            var product = await _productRepository.GetById(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Product with id {request.ProductId} does not exist.");
            }
            
            if (product.Stock < request.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product with id {request.ProductId}. Requested: {request.Quantity}, Available: {product.Stock}");
            }

           
            product.Stock -= request.Quantity;

            var orderDetail = CreateOrderDetailRequest.ToEntity(request, product, order);     
            order.Details.Add(orderDetail);
            order.TotalPrice += orderDetail.Total;
            // Guardar cambios en una transacción
            // Ejecutar las operaciones en una transacción
            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _orderDetailRepository.Create(orderDetail);
                await _orderRepository.Update(order);
                await _productRepository.Update(product);
            });


            return OrderDetailDto.CreateDto(orderDetail);
        }



        //public async Task<OrderDetailDto> UpdateOrderDetail(int id, UpdateOrderDetailRequest request)
        //{
        //    var orderDetail = await _orderDetailRepository.GetById(id);
        //    if (orderDetail == null)
        //    {
        //        throw new NotFoundException($"OrderDetail with id {id} does not exist");
        //    }

        //    var product = await _productRepository.GetById(request.ProductId);
        //    if (product == null)
        //    {
        //        throw new NotFoundException($"Product with id {request.ProductId} does not exist");
        //    }

        //    orderDetail.Product = product;
        //    orderDetail.Quantity = request.Quantity;
        //    orderDetail.Total = product.Price * request.Quantity;

        //    await _orderDetailRepository.Update(orderDetail);

        //    return OrderDetailDto.CreateDto(orderDetail);
        //}

        public async Task<bool> DeleteOrderDetail(int id)
            {
                var orderDetail = await _orderDetailRepository.GetById(id);
                if (orderDetail == null)
                {
                    throw new NotFoundException($"OrderDetail with id {id} does not exist");
                }

                await _orderDetailRepository.Delete(orderDetail);
                return true;
            }
        }
    }

