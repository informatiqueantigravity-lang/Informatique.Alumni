using Volo.Abp;

namespace Informatique.Alumni.Certificates;

public class InsufficientPaymentException : BusinessException
{
    public InsufficientPaymentException(decimal totalFees, decimal paidAmount)
        : base(code: "Alumni:InsufficientPayment")
    {
        WithData("TotalFees", totalFees);
        WithData("PaidAmount", paidAmount);
        WithData("RemainingAmount", totalFees - paidAmount);
    }
}
