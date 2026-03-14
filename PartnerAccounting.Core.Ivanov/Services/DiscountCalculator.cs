using System;
using System.Linq;
using PartnerAccounting.Core.Ivanov.Data;
using PartnerAccounting.Core.Ivanov.Models;

namespace PartnerAccounting.Core.Ivanov.Services
{

    public class DiscountCalculator
    {
        private readonly AppDbContext _context;

        public DiscountCalculator(AppDbContext context)
        {
            _context = context;
        }

        public decimal CalculateTotalSales(int partnerId)
        {
            try
            {
                return _context.SalesHistory
                    .Where(sh => sh.PartnerId == partnerId)
                    .Sum(sh => sh.TotalAmount);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при расчете суммы продаж: {ex.Message}", ex);
            }
        }

        public int GetPartnerDiscount(Partner partner)
        {
            if (partner == null)
                throw new ArgumentNullException(nameof(partner));

            decimal totalSales = CalculateTotalSales(partner.Id);
            return partner.CalculateDiscount(totalSales);
        }

        public int GetPartnerDiscount(int partnerId)
        {
            var partner = _context.Partners.Find(partnerId);
            if (partner == null)
                throw new ArgumentException($"Партнер с ID {partnerId} не найден");

            return GetPartnerDiscount(partner);
        }
    }
}