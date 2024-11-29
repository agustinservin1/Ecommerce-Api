using Application.Models;
using Application.Models.PaymentModels;
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
    }
}
