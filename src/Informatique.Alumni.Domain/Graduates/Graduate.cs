using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Informatique.Alumni.Graduates;

public class Graduate : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    
    public string MembershipCardNumber { get; set; } = string.Empty;
    
    public DateTime MembershipExpiryDate { get; set; }
    
    public bool IsMembershipActive => DateTime.UtcNow <= MembershipExpiryDate;
    
    public decimal OpeningBalance { get; set; }
    
    protected Graduate()
    {
    }
    
    public Graduate(Guid id, string name, string membershipCardNumber, DateTime membershipExpiryDate, decimal openingBalance)
        : base(id)
    {
        Name = name;
        MembershipCardNumber = membershipCardNumber;
        MembershipExpiryDate = membershipExpiryDate;
        OpeningBalance = openingBalance;
    }
}
