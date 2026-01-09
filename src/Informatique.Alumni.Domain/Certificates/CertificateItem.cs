using System;
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
        QualificationId = qualificationId;
        CertificateTypeId = certificateTypeId;
        Language = language;
        Quantity = quantity;
    }
}
