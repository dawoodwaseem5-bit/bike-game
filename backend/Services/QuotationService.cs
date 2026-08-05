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

        public async Task<(bool Success, string Message, QuotationResponseDto? Quotation)> CreateQuotationAsync(QuotationCreateDto dto, string username)
        {
            if (!await _quotationRepository.CustomerExistsAsync(dto.CustomerId))
                return (false, "Customer not found.", null);

            var quotation = new Quotation
            {
                CustomerId = dto.CustomerId,
                TaxRate = dto.TaxRate,
                ValidUntil = dto.ValidUntil,
                Status = "Pending",
                QuotationNumber = $"QT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4)}",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = username
            };

            decimal subTotal = 0;
            decimal totalDiscount = 0;

            foreach (var item in dto.Items)
            {
                var product = await _quotationRepository.GetProductByIdAsync(item.ProductId);
                if (product == null) continue;

                decimal itemSubtotal = product.UnitPrice * item.Quantity;
                decimal itemDiscountAmount = itemSubtotal * (item.DiscountPercent / 100m);

                subTotal += itemSubtotal;
                totalDiscount += itemDiscountAmount;

                quotation.QuotationItems.Add(new QuotationItem
                {
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice,
                    DiscountPercent = item.DiscountPercent,
                    DiscountAmount = itemDiscountAmount,
                    TaxRate = dto.TaxRate,
                    LineTotal = itemSubtotal - itemDiscountAmount,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = username
                });
            }

            quotation.SubTotal = subTotal;
            quotation.DiscountAmount = totalDiscount;
            decimal amountAfterDiscount = subTotal - totalDiscount;
            quotation.TaxAmount = amountAfterDiscount * (dto.TaxRate / 100m);
            quotation.TotalAmount = amountAfterDiscount + quotation.TaxAmount;

            await _quotationRepository.AddAsync(quotation);
            await _quotationRepository.SaveChangesAsync();

            var responseDto = new QuotationResponseDto
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

            return (true, "Quotation created successfully.", responseDto);
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

            var responseDto = new QuotationResponseDto
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

            return (true, "Quotation status updated successfully.", responseDto);
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
    }
}
