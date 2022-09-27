using AutoMapper;
using Retailer.Common.Utility;
using Retailer.Data.Models;
using Retailer.DataAccess.Repository.IRepository;
using Retailer.Interface.Dtos;
using Retailer.Interface.Interfaces.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Business.Managers
{
    public class OrderManager : IOrderManager
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public OrderManager(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        public async Task<OrderDto> GetOrder(int orderId)
        {
            var orderDetails = await _repositoryFactory.OrderDetailRepository.FindOne(x => x.OrderHeaderId == orderId);
            var orderHeader = await _repositoryFactory.OrderHeaderRepository.FindOne(x => x.Id == orderId);

            var orderDetailsDto = _mapper.Map<List<OrderDetailDto>>(orderDetails);
            var orderHeaderDto = _mapper.Map<OrderHeaderDto>(orderHeader);

            return new OrderDto
            {
                OrderDetails = orderDetailsDto,
                OrderHeader = orderHeaderDto
            };
        }

        public async Task<List<OrderDto>> GetAllOrders(string? userId = null, string? status = null)
        {
            var orderDetails = await _repositoryFactory.OrderDetailRepository.FindMany(x => true, includePaths: new string[] { "Product", "Product.ProductPriceList" });
            var orderHeaderList = await _repositoryFactory.OrderHeaderRepository.FindMany(x => true);

            var orderDetailsDto = _mapper.Map<List<OrderDetailDto>>(orderDetails);
            var orderHeaderDtoList = _mapper.Map<List<OrderHeaderDto>>(orderHeaderList);

            var orderDtoList = new List<OrderDto>();

            foreach (var orderheader in orderHeaderDtoList)
            {
                orderDtoList.Add(new OrderDto
                {
                    OrderHeader = orderheader,
                    OrderDetails = orderDetailsDto.Where(x => x.OrderHeaderId == orderheader.Id).ToList()
                });
            }

            return orderDtoList;
        }

        public async Task<OrderDto> CreateOrder(OrderDto order)
        {

            var orderHeader = _mapper.Map<OrderHeader>(order.OrderHeader);
            await _repositoryFactory.OrderHeaderRepository.Add(orderHeader);
            await _repositoryFactory.OrderHeaderRepository.SaveChanges();

            var orderDetailList = new List<OrderDetailDto>();

            if (orderDetailList != null)
            {
                foreach (var orderDetailDto in order.OrderDetails)
                {
                    orderDetailDto.OrderHeaderId = orderHeader.Id;

                    var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);

                    await _repositoryFactory.OrderDetailRepository.Add(orderDetail);

                    orderDetailList.Add(orderDetailDto);
                }

                await _repositoryFactory.OrderHeaderRepository.SaveChanges();
            }

            return new OrderDto
            {
                OrderDetails = orderDetailList,
                OrderHeader = _mapper.Map<OrderHeaderDto>(orderHeader)
            };
        }

        public async Task DeleteOrder(int id)
        {
            var orderDetails = await _repositoryFactory.OrderDetailRepository.FindMany(x => x.OrderHeaderId == id);

            await _repositoryFactory.OrderDetailRepository.DeleteRange(_mapper.Map<List<OrderDetail>>(orderDetails));

            await _repositoryFactory.OrderHeaderRepository.Delete(_mapper.Map<OrderHeader>(await _repositoryFactory.OrderHeaderRepository.FindOne(x => x.Id == id)));

            await _repositoryFactory.OrderHeaderRepository.SaveChanges();
        }

        public async Task<OrderHeaderDto> UpdateHeader(OrderHeaderDto orderHeaderDto)
        {
            var orderHeader = await _repositoryFactory.OrderHeaderRepository.FindOne(x => x.Id == orderHeaderDto.Id);

            //Why cant we use Update method????
            if (orderHeaderDto != null)
            {
                orderHeader.OrderDate = orderHeaderDto.OrderDate;
                orderHeader.ShippingDate = orderHeaderDto.ShippingDate;
                orderHeader.State = orderHeaderDto.State;
                orderHeader.SessionId = orderHeaderDto.SessionId;
                orderHeader.Status = orderHeaderDto.Status;
                orderHeader.StreetAddress = orderHeaderDto.StreetAddress;
                orderHeader.UserId = orderHeaderDto.UserId;
                orderHeader.PostalCode = orderHeaderDto.PostalCode;
                orderHeader.OrderTotal = orderHeaderDto.OrderTotal;
                orderHeader.City = orderHeaderDto.City;
                orderHeader.Name = orderHeaderDto.Name;
                orderHeader.PaymentIntentId = orderHeaderDto.PaymentIntentId;
                orderHeader.PhoneNumber = orderHeaderDto.PhoneNumber;
            }

            await _repositoryFactory.OrderHeaderRepository.SaveChanges();

            return orderHeaderDto;
        }

        public async Task<OrderHeaderDto> MarkPaymentSuccessfull(int id)
        {
            var orderHeader = await _repositoryFactory.OrderHeaderRepository.FindOne(x => x.Id == id);

            if (orderHeader != null)
            {
                if (orderHeader.Status == SD.Status_Pending)
                {
                    orderHeader.Status = SD.Status_Confirmed;
                    await _repositoryFactory.OrderHeaderRepository.SaveChanges();
                    return _mapper.Map<OrderHeaderDto>(orderHeader);
                }
            }

            return new OrderHeaderDto();
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var orderHeader = await _repositoryFactory.OrderHeaderRepository.FindOne(x => x.Id == orderId);

            if (orderHeader == null)
            {
                return false;
            }

            orderHeader.Status = status;

            if (orderHeader.Status == SD.Status_Shipped)
            {
                orderHeader.ShippingDate = DateTime.Now;
            }

            await _repositoryFactory.OrderHeaderRepository.SaveChanges();
            return true;
        }

    }


}

