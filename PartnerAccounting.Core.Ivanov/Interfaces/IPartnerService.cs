using System.Collections.Generic;
using PartnerAccounting.Core.Ivanov.Models;

namespace PartnerAccounting.Core.Ivanov.Interfaces
{

    public interface IPartnerService
    {

        List<Partner> GetAllPartners();

        Partner GetPartnerById(int id);

        void AddPartner(Partner partner);

        void UpdatePartner(Partner partner);

        void DeletePartner(int id);

        List<PartnerType> GetAllPartnerTypes();

        List<SaleHistory> GetPartnerSalesHistory(int partnerId);

        int GetPartnerDiscount(int partnerId);
    }
}