using Application.Models.PaymentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentNotificationService
    {
        bool ValidateSignature(string xSignature, string xRequestId, string dataId);
        Task ProcessNotification(InfoPaymentNotification notification);
    }
}
