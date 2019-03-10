using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Web.Infrastructure.Handlers;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Web.Admin.Handlers.Cards
{
    public class ExportCardsHandler : AbstractCommandHandler<byte[]>
    {
        protected override byte[] Handle()
        {
            using (var ms = new MemoryStream())
            {
                using (var csvWriter = new CsvHelper.CsvWriter(new StreamWriter(ms, Encoding.UTF8), new CsvConfiguration() { Delimiter = ";", SkipEmptyRecords = false }))
                {
                    var headers = new[] {
                        "Mobile",
                        "Email",
                        "Card",
                        "LastName",
                        "FirstName",
                        "SecondName",
                        "DateOfBirth",
                        "Bonus",
                        "Grade" };

                    foreach (var item in headers)
                    {
                        csvWriter.WriteField(item);
                    }
                    csvWriter.NextRecord();

                    foreach (var card in CardService.Gets())
                    {
                        if (card.Customer != null)
                        {
                            csvWriter.WriteField(card.Customer.Phone);
                            csvWriter.WriteField(card.Customer.EMail);
                            csvWriter.WriteField(card.Customer.BonusCardNumber);
                            csvWriter.WriteField(card.Customer.LastName);
                            csvWriter.WriteField(card.Customer.FirstName);
                            csvWriter.WriteField(card.Customer.Patronymic);
                            csvWriter.WriteField(card.Customer.BirthDay != null ? card.Customer.BirthDay.ToString() : string.Empty);
                        }

                        csvWriter.WriteField(card.BonusAmount);
                        if (card.Grade != null)
                        {
                            csvWriter.WriteField(card.Grade.Name);
                        }

                        csvWriter.NextRecord();
                    }
                }
                return ms.ToArray();
            }
        }
    }
}
