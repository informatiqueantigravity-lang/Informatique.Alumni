using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Informatique.Alumni.Certificates;

public class CertificateRequest : FullAuditedAggregateRoot<Guid>
{
    public Guid GraduateId { get; private set; }
    
    public decimal TotalAmount { get; private set; }
    
    public PaymentMethod PaymentMethod { get; private set; }
    
    public DeliveryMethod DeliveryMethod { get; private set; }
    
    public CertificateRequestStatus Status { get; private set; }
    
    public Guid BranchId { get; private set; }

    private readonly List<CertificateItem> _items;
    public IReadOnlyCollection<CertificateItem> Items => new ReadOnlyCollection<CertificateItem>(_items);

    private CertificateRequest()
    {
        // Private constructor for ORM
        _items = new List<CertificateItem>();
    }

    internal CertificateRequest(
        Guid id,
        Guid graduateId,
        Guid branchId,
        PaymentMethod paymentMethod,
        DeliveryMethod deliveryMethod,
        decimal totalAmount = 0) : base(id)
    {
        if (graduateId == Guid.Empty)
            throw new ArgumentException("Value cannot be empty.", nameof(graduateId));
        if (branchId == Guid.Empty)
            throw new ArgumentException("Value cannot be empty.", nameof(branchId));
        Check.Range(totalAmount, nameof(totalAmount), 0, decimal.MaxValue);

        GraduateId = graduateId;
        BranchId = branchId;
        PaymentMethod = paymentMethod;
        DeliveryMethod = deliveryMethod;
        TotalAmount = totalAmount;
        Status = CertificateRequestStatus.Pending;
        _items = new List<CertificateItem>();
    }

    public void AddItem(
        Guid qualificationId,
        Guid certificateTypeId,
        CertificateLanguage language,
        int quantity = 1)
    {
        Check.Positive(quantity, nameof(quantity));

        var item = new CertificateItem(
            Guid.NewGuid(),
            qualificationId,
            certificateTypeId,
            language,
            quantity);

        _items.Add(item);
    }

    public void UpdateTotalAmount(decimal totalAmount)
    {
        Check.Range(totalAmount, nameof(totalAmount), 0, decimal.MaxValue);
        TotalAmount = totalAmount;
    }

    public void UpdateStatus(CertificateRequestStatus status)
    {
        Status = status;
    }
}
