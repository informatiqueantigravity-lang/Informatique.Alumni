using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Informatique.Alumni.Graduates;

public class Graduate : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    
    public string MembershipCardNumber { get; set; } = string.Empty;
    
    public DateTime MembershipExpiryDate { get; set; }
    
    public decimal OpeningBalance { get; private set; }
    
    protected Graduate()
    {
    }
    
    public Graduate(Guid id, string name, string membershipCardNumber, DateTime membershipExpiryDate, decimal openingBalance)
        : base(id)
    {
        if (openingBalance < 0)
        {
            throw new ArgumentException("Opening balance cannot be negative", nameof(openingBalance));
        }
        
        Name = name;
        MembershipCardNumber = membershipCardNumber;
        MembershipExpiryDate = membershipExpiryDate;
        OpeningBalance = openingBalance;
    }
    
    public bool IsMembershipActive(DateTime currentDate)
    {
        return currentDate <= MembershipExpiryDate;
    }
    
    public void DeductFromBalance(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount to deduct cannot be negative", nameof(amount));
        }
        
        if (amount > OpeningBalance)
        {
            throw new InvalidOperationException("Cannot deduct more than the current opening balance");
        }
        
        OpeningBalance -= amount;
    }
}
