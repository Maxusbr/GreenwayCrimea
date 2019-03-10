//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Shipping.Usps
{
    public struct UspsTemplate
    {
        public const string UserId = "UserId";
        public const string PostalCode = "PostalCode";
        public const string Password = "Password";
        public const string Extracharge = "Extracharge";
        public const string Rate = "Rate";


        //domestic
        public const string FirstClass = "First-Class";
        public const string ExpressMailSundayHolidayGuarantee = "Express Mail Sunday/Holiday Guarantee";
        public const string ExpressMailFlatRateEnvelopeSundayHolidayGuarantee = "Express Mail Flat-Rate Envelope Sunday/Holiday Guarantee";
        public const string ExpressMailHoldForPickup = "Express Mail Hold For Pickup";
        public const string ExpressMailFlatRateEnvelopeHoldForPickup = "Express Mail Flat Rate Envelope Hold For Pickup";
        public const string ExpresMail = "Express Mail";
        public const string ExpressMailFlatRateEnvelope = "Express Mail Flat Rate Envelope";
        public const string PriorityMail = "Priority Mail";
        public const string PriorityMailFlatRateEnvelope = "Priority Mail Flat Rate Envelope";
        public const string PriorityMailSmallFlatRateBox = "Priority Mail Small Flat Rate Box";
        public const string PriorityMailMediumFlatRateBox = "Priority Mail Medium Flat Rate Box";
        public const string PriorityMailLargeFlatRateBox = "Priority Mail Large Flat Rate Box";
        public const string ParcelPost = "Parcel Post";
        public const string BoundPrintedMatter = "Bound Printed Matter";
        public const string MediaMail = "Media Mail";
        public const string LibraryMail = "Library Mail";

        //international
        public const string GlobalExpressGuaranteed = "Global Express Guaranteed (GXG)";
        public const string GlobalExpressGuaranteedNonDocumentRectangular = "Global Express Guaranteed Non-Document Rectangular";
        public const string GlobalExpressGuaranteedNonDocumentNonRectangular = "Global Express Guaranteed Non-Document Non-Rectangular";
        public const string UspsGxgEnvelopes = "USPS GXG Envelopes";
        public const string ExpressMailInternationalFlatRateEnvelope = "Express Mail International Flat Rate Envelope";
        public const string PriorityMailInternational = "Priority Mail International";
        public const string PriorityMailInternationalLargeFlatRateBox = "Priority Mail International Large Flat Rate Box";
        public const string PriorityMailInternationalMediumFlatRateBox = "Priority Mail International Medium Flat Rate Box";
        public const string PriorityMailInternationalSmallFlatRateBox = "Priority Mail International Small Flat Rate Box";
        public const string FirstClassMailInternationalLargeEnvelope = "First-Class Mail International Large Envelope";
        public const string ExpressMailInternational = "Express Mail International";
        public const string PriorityMailInternationalFlatRateEnvelope = "Priority Mail International Flat Rate Envelope";
        public const string FirstClassMailInternationalPackage = "First-Class Mail International Package";
    }
}