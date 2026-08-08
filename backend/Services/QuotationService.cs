using backend.DTOs;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IQuotationRepository _quotationRepository;

        public QuotationService(IQuotationRepository quotationRepository)
        {
            _quotationRepository = quotationRepository;
        }

        public async Task<object> GetQuotationsPaginatedAsync(int page, int pageSize, string search, string email = "", string role = "")
        {
            var (totalItems, items) = await _quotationRepository.GetPaginatedAsync(page, pageSize, search, email, role);

            return new
            {
                totalItems,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                data = items
            };
        }

        public async Task<(bool Success, string Message, QuotationResponseDto? Quotation)> CreateQuotationAsync(QuotationCreateDto dto, string email, string role)
        {
            int customerId = dto.CustomerId;
            decimal taxRate = dto.TaxRate;
            string status = "Pending";

            if (role == "Customer")
            {
                var customer = await _quotationRepository.GetCustomerByEmailAsync(email);
                if (customer == null)
                    return (false, "Customer profile not found.", null);

                customerId = customer.CustomerId;
                taxRate = 0;
                status = "Draft";
            }
            else
            {
                if (!await _quotationRepository.CustomerExistsAsync(customerId))
                    return (false, "Customer not found.", null);

                if (taxRate < 0)
                    return (false, "Tax rate cannot be negative.", null);
            }

            if (dto.Items == null || dto.Items.Count == 0)
                return (false, "At least one line item is required.", null);

            foreach (var item in dto.Items)
            {
                if (item.Quantity < 1)
                    return (false, "Quantity must be at least 1.", null);

                if (role != "Customer" && (item.DiscountPercent < 0 || item.DiscountPercent > 100))
                    return (false, "Discount percent must be between 0 and 100.", null);
            }

            var quotation = new Quotation
            {
                CustomerId = customerId,
                TaxRate = taxRate,
                ValidUntil = dto.ValidUntil,
                Status = status,
                QuotationNumber = $"QT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4)}",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = email
            };

            decimal subTotal = 0;
            decimal totalDiscount = 0;

            foreach (var item in dto.Items)
            {
                var product = await _quotationRepository.GetProductByIdAsync(item.ProductId);
                if (product == null) continue;

                var discountPercent = role == "Customer" ? 0 : item.DiscountPercent;
                decimal itemSubtotal = product.UnitPrice * item.Quantity;
                decimal itemDiscountAmount = itemSubtotal * (discountPercent / 100m);

                subTotal += itemSubtotal;
                totalDiscount += itemDiscountAmount;

                quotation.QuotationItems.Add(new QuotationItem
                {
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice,
                    DiscountPercent = discountPercent,
                    DiscountAmount = itemDiscountAmount,
                    TaxRate = taxRate,
                    LineTotal = itemSubtotal - itemDiscountAmount,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = email
                });
            }

            if (quotation.QuotationItems.Count == 0)
                return (false, "At least one valid product item is required.", null);

            quotation.SubTotal = subTotal;
            quotation.DiscountAmount = totalDiscount;
            decimal amountAfterDiscount = subTotal - totalDiscount;
            quotation.TaxAmount = amountAfterDiscount * (taxRate / 100m);
            quotation.TotalAmount = amountAfterDiscount + quotation.TaxAmount;

            await _quotationRepository.AddAsync(quotation);
            await _quotationRepository.SaveChangesAsync();

            return (true, "Quotation created successfully.", ToResponseDto(quotation));
        }

        public async Task<(bool Success, string Message, QuotationResponseDto? UpdatedQuotation)> FinalizeQuotationAsync(int id, QuotationFinalizeDto dto, string email)
        {
            var quotation = await _quotationRepository.GetByIdAsync(id);
            if (quotation == null)
                return (false, "Quotation not found.", null);

            if (quotation.Status != "Draft")
                return (false, "Only draft quotations can be finalized.", null);

            if (dto.TaxRate < 0)
                return (false, "Tax rate cannot be negative.", null);

            if (dto.DiscountPercent < 0 || dto.DiscountPercent > 100)
                return (false, "Discount percent must be between 0 and 100.", null);

            decimal subTotal = 0;
            decimal totalDiscount = 0;

            foreach (var item in quotation.QuotationItems)
            {
                decimal itemSubtotal = item.UnitPrice * item.Quantity;
                decimal itemDiscountAmount = itemSubtotal * (dto.DiscountPercent / 100m);

                item.DiscountPercent = dto.DiscountPercent;
                item.DiscountAmount = itemDiscountAmount;
                item.TaxRate = dto.TaxRate;
                item.LineTotal = itemSubtotal - itemDiscountAmount;
                item.UpdatedAt = DateTime.UtcNow;
                item.UpdatedBy = email;

                subTotal += itemSubtotal;
                totalDiscount += itemDiscountAmount;
            }

            quotation.TaxRate = dto.TaxRate;
            quotation.SubTotal = subTotal;
            quotation.DiscountAmount = totalDiscount;
            decimal amountAfterDiscount = subTotal - totalDiscount;
            quotation.TaxAmount = amountAfterDiscount * (dto.TaxRate / 100m);
            quotation.TotalAmount = amountAfterDiscount + quotation.TaxAmount;
            quotation.Status = "Pending";
            quotation.UpdatedAt = DateTime.UtcNow;
            quotation.UpdatedBy = email;

            await _quotationRepository.SaveChangesAsync();

            return (true, "Quotation finalized successfully.", ToResponseDto(quotation));
        }

        public async Task<(bool Success, string Message, QuotationResponseDto? UpdatedQuotation)> UpdateQuotationStatusAsync(int id, string status, string username)
        {
            var quotation = await _quotationRepository.GetByIdAsync(id);
            if (quotation == null)
                return (false, "Quotation not found.", null);

            quotation.Status = status;
            quotation.UpdatedAt = DateTime.UtcNow;
            quotation.UpdatedBy = username;

            await _quotationRepository.SaveChangesAsync();

            return (true, "Quotation status updated successfully.", ToResponseDto(quotation));
        }

        public async Task<(bool Success, string Message)> DeleteQuotationAsync(int id, string username)
        {
            var quotation = await _quotationRepository.GetByIdAsync(id);
            if (quotation == null)
                return (false, "Quotation not found.");

            quotation.IsDeleted = true;
            quotation.UpdatedAt = DateTime.UtcNow;
            quotation.UpdatedBy = username;

            await _quotationRepository.SaveChangesAsync();

            return (true, "Quotation deleted successfully.");
        }

        private static QuotationResponseDto ToResponseDto(Quotation quotation) => new()
        {
            QuotationId = quotation.QuotationId,
            QuotationNumber = quotation.QuotationNumber,
            CustomerId = quotation.CustomerId,
            Status = quotation.Status,
            SubTotal = quotation.SubTotal,
            DiscountAmount = quotation.DiscountAmount,
            TaxRate = quotation.TaxRate,
            TaxAmount = quotation.TaxAmount,
            TotalAmount = quotation.TotalAmount,
            ValidUntil = quotation.ValidUntil,
            CreatedAt = quotation.CreatedAt
        };
    }
}
