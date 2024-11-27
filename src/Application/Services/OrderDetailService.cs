using Application.Interfaces;
using Application.Models.Request;
using Application.Models;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services
{
    public class OrderDetailService : IOrderDetailService
    {
            private readonly IOrderDetailRepository _orderDetailRepository;
            private readonly IProductRepository _productRepository;
            private readonly IOrderRepository _orderRepository;

            public OrderDetailService(IOrderDetailRepository orderDetailRepository, IProductRepository productRepository, IOrderRepository orderRepository)
            {
                _orderDetailRepository = orderDetailRepository;
                _productRepository = productRepository;
                _orderRepository = orderRepository;
            }

            public async Task<IEnumerable<OrderDetailDto>> GetAllOrderDetails()
            {
                var orderDetails = await _orderDetailRepository.GetAll();
                return OrderDetailDto.CreateListDto(orderDetails);
            }

            public async Task<OrderDetailDto> GetOrderDetailById(int id)
            {
                var orderDetail = await _orderDetailRepository.GetById(id);
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

            var orderDetail = CreateOrderDetailRequest.ToEntity(request, product, order);

            

            await _orderDetailRepository.Create(orderDetail);
            order.Details.Add(orderDetail);
            order.TotalPrice += orderDetail.Total;
            await _orderRepository.Update(order);

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

