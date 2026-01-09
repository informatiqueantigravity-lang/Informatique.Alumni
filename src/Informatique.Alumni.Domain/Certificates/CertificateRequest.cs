using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Informatique.Alumni.Certificates;

public class CertificateRequest : AuditedAggregateRoot<Guid>
{
    public Guid GraduateId { get; set; }
    
    public decimal TotalFees { get; set; }
    
    public decimal AmountDeductedFromBalance { get; set; }
    
    public decimal RemainingAmountToPay { get; set; }
    
    public CertificateRequestStatus Status { get; set; }
    
    public DateTime? SentToOfficeDate { get; set; }
    
    protected CertificateRequest()
    {
    }
    
    public CertificateRequest(Guid id, Guid graduateId, decimal totalFees)
        : base(id)
    {
        GraduateId = graduateId;
        TotalFees = totalFees;
        Status = CertificateRequestStatus.Draft;
    }
    
    public void SetFinancialDetails(decimal amountDeductedFromBalance, decimal remainingAmountToPay)
    {
        AmountDeductedFromBalance = amountDeductedFromBalance;
        RemainingAmountToPay = remainingAmountToPay;
    }
    
    public void MarkAsSentToOffice()
    {
        Status = CertificateRequestStatus.SentToOffice;
        SentToOfficeDate = DateTime.UtcNow;
    }
}

public enum CertificateRequestStatus
{
    Draft = 0,
    SentToOffice = 1,
    Approved = 2,
    Rejected = 3
}
