using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using PartnerAccounting.Core.Ivanov.Data;
using PartnerAccounting.Core.Ivanov.Models;
using PartnerAccounting.Core.Ivanov.Interfaces;

namespace PartnerAccounting.Core.Ivanov.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly AppDbContext _context;
        private readonly DiscountCalculator _discountCalculator;

        public PartnerService(AppDbContext context)
        {
            _context = context;
            _discountCalculator = new DiscountCalculator(context);
        }

        public List<Partner> GetAllPartners()
        {
            return _context.Partners
                .Include(p => p.PartnerType)
                .Include(p => p.SalesHistory)
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToList();
        }

        public Partner GetPartnerById(int id)
        {
            return _context.Partners
                .Include(p => p.PartnerType)
                .Include(p => p.SalesHistory)
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id) ?? new Partner();
        }

        public void AddPartner(Partner partner)
        {
            partner.CreatedAt = DateTime.UtcNow;
            partner.UpdatedAt = DateTime.UtcNow;
            _context.Partners.Add(partner);
            _context.SaveChanges();
        }

        public void UpdatePartner(Partner partner)
        {
            var existing = _context.Partners.Find(partner.Id);
            if (existing == null) return;

            existing.TypeId = partner.TypeId;
            existing.Name = partner.Name;
            existing.LegalAddress = partner.LegalAddress;
            existing.Inn = partner.Inn;
            existing.DirectorName = partner.DirectorName;
            existing.Phone = partner.Phone;
            existing.Email = partner.Email;
            existing.Rating = partner.Rating;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public void DeletePartner(int id)
        {
            var partner = _context.Partners.Find(id);
            if (partner != null)
            {
                _context.Partners.Remove(partner);
                _context.SaveChanges();
            }
        }

        public List<PartnerType> GetAllPartnerTypes()
        {
            return _context.PartnerTypes
                .AsNoTracking()
                .OrderBy(pt => pt.Name)
                .ToList();
        }

        public List<SaleHistory> GetPartnerSalesHistory(int partnerId)
        {
            return _context.SalesHistory
                .Include(sh => sh.Product)
                .Where(sh => sh.PartnerId == partnerId)
                .OrderByDescending(sh => sh.SaleDate)
                .AsNoTracking()
                .ToList();
        }

        public int GetPartnerDiscount(int partnerId)
        {
            return _discountCalculator.GetPartnerDiscount(partnerId);
        }
    }
}