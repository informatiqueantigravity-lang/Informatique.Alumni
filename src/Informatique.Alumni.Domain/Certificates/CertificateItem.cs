using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Informatique.Alumni.Certificates;

public class CertificateItem : Entity<Guid>
{
    public Guid QualificationId { get; private set; }
    
    public Guid CertificateTypeId { get; private set; }
    
    public int Quantity { get; private set; }
    
    public CertificateLanguage Language { get; private set; }

    private CertificateItem()
    {
        // Private constructor for ORM
    }

    internal CertificateItem(
        Guid id,
        Guid qualificationId,
        Guid certificateTypeId,
        CertificateLanguage language,
        int quantity = 1) : base(id)
    {
        if (qualificationId == Guid.Empty)
            throw new ArgumentException("Value cannot be empty.", nameof(qualificationId));
        if (certificateTypeId == Guid.Empty)
            throw new ArgumentException("Value cannot be empty.", nameof(certificateTypeId));
        Check.Positive(quantity, nameof(quantity));

        QualificationId = qualificationId;
        CertificateTypeId = certificateTypeId;
        Language = language;
        Quantity = quantity;
    }
}
