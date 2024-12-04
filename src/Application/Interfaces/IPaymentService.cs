using Application.Models;
using Application.Models.PaymentModels;
using Domain.Entities;
using MercadoPago.Resource.Preference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        Task<Preference> CreatePaymentAsync(int idOrder);
        Task<IEnumerable<Payments>> GetAllPayments();
        Task<Payments> GetPaymentById(int id);
    }
}
