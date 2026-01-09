using System;
using System.Threading.Tasks;
using Informatique.Alumni.Graduates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Informatique.Alumni.Certificates;

public class CertificateManager : DomainService
{
    private readonly IRepository<Graduate, Guid> _graduateRepository;
    private readonly IRepository<CertificateRequest, Guid> _certificateRequestRepository;

    public CertificateManager(
        IRepository<Graduate, Guid> graduateRepository,
        IRepository<CertificateRequest, Guid> certificateRequestRepository)
    {
        _graduateRepository = graduateRepository;
        _certificateRequestRepository = certificateRequestRepository;
    }

    /// <summary>
    /// Validates that the graduate's membership card is currently active
    /// </summary>
    public async Task ValidateMembershipAsync(Guid graduateId)
    {
        var graduate = await _graduateRepository.GetAsync(graduateId);
        
        if (!graduate.IsMembershipActive(DateTime.UtcNow))
        {
            throw new InactiveMembershipException(graduate.MembershipCardNumber);
        }
    }

    /// <summary>
    /// Calculates the total fees and determines how much will be deducted from the graduate's
    /// opening balance and how much remains to be paid externally
    /// </summary>
    public async Task<FinancialCalculationResult> CalculateFinancialDetailsAsync(Guid graduateId, decimal totalFees)
    {
        var graduate = await _graduateRepository.GetAsync(graduateId);
        
        decimal amountDeductedFromBalance;
        decimal remainingAmountToPay;
        
        if (graduate.OpeningBalance >= totalFees)
        {
            // Balance is sufficient to cover the entire fee
            amountDeductedFromBalance = totalFees;
            remainingAmountToPay = 0;
        }
        else
        {
            // Balance is insufficient, use what's available and calculate remaining
            amountDeductedFromBalance = graduate.OpeningBalance;
            remainingAmountToPay = totalFees - graduate.OpeningBalance;
        }
        
        return new FinancialCalculationResult
        {
            TotalFees = totalFees,
            AmountDeductedFromBalance = amountDeductedFromBalance,
            RemainingAmountToPay = remainingAmountToPay
        };
    }

    /// <summary>
    /// Creates a certificate request and calculates financial details
    /// </summary>
    public async Task<CertificateRequest> CreateCertificateRequestAsync(Guid graduateId, decimal totalFees)
    {
        // 1. Validate membership
        await ValidateMembershipAsync(graduateId);
        
        // 2. Calculate financial details
        var financialDetails = await CalculateFinancialDetailsAsync(graduateId, totalFees);
        
        // 3. Create the certificate request
        var certificateRequest = new CertificateRequest(
            GuidGenerator.Create(),
            graduateId,
            totalFees
        );
        
        certificateRequest.SetFinancialDetails(
            financialDetails.AmountDeductedFromBalance,
            financialDetails.RemainingAmountToPay
        );
        
        return await _certificateRequestRepository.InsertAsync(certificateRequest);
    }

    /// <summary>
    /// Validates that the total fee is fully covered and sends the request to the office
    /// </summary>
    public async Task SendRequestToOfficeAsync(Guid certificateRequestId, decimal? externalPaymentAmount = null)
    {
        var certificateRequest = await _certificateRequestRepository.GetAsync(certificateRequestId);
        var graduate = await _graduateRepository.GetAsync(certificateRequest.GraduateId);
        
        // Calculate total paid amount (balance deduction + external payment)
        var totalPaidAmount = certificateRequest.AmountDeductedFromBalance + (externalPaymentAmount ?? 0);
        
        // Validate that the total fee is fully covered
        if (totalPaidAmount < certificateRequest.TotalFees)
        {
            throw new InsufficientPaymentException(certificateRequest.TotalFees, totalPaidAmount);
        }
        
        // Verify that the graduate still has sufficient balance
        // (balance may have changed since request creation)
        if (graduate.OpeningBalance < certificateRequest.AmountDeductedFromBalance)
        {
            throw new InvalidOperationException(
                $"Graduate's current balance ({graduate.OpeningBalance}) is insufficient to deduct the planned amount ({certificateRequest.AmountDeductedFromBalance}). " +
                "The balance may have changed since the request was created.");
        }
        
        // Deduct from graduate's opening balance using the domain method
        graduate.DeductFromBalance(certificateRequest.AmountDeductedFromBalance);
        await _graduateRepository.UpdateAsync(graduate);
        
        // Mark as sent to office
        certificateRequest.MarkAsSentToOffice();
        await _certificateRequestRepository.UpdateAsync(certificateRequest);
    }
}

public class FinancialCalculationResult
{
    public decimal TotalFees { get; set; }
    public decimal AmountDeductedFromBalance { get; set; }
    public decimal RemainingAmountToPay { get; set; }
}
