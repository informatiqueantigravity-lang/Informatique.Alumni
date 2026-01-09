using Volo.Abp;

namespace Informatique.Alumni.Certificates;

public class InactiveMembershipException : BusinessException
{
    public InactiveMembershipException(string membershipCardNumber)
        : base(code: "Alumni:InactiveMembership")
    {
        WithData("MembershipCardNumber", membershipCardNumber);
    }
}
