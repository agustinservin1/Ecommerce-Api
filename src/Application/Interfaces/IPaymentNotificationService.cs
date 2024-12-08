using Application.Models.PaymentModels;

namespace Application.Interfaces
{
    public interface IPaymentNotificationService
    {
        bool ValidateSignature(string xSignature, string xRequestId, string dataId);
        Task<bool> ProcessNotification(InfoPaymentNotification notification);

    }
}
