using AdvantShop.Diagnostics;
using AdvantShop.Statistic;
using AdvantShop.Web.Infrastructure.Handlers;
using AdvantShop.Core.Common.Extensions;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Customers;
using AdvantShop.Core.Services.Bonuses.Model.Enums;

namespace AdvantShop.Web.Admin.Handlers.Cards
{
    public class ImportCardsHandler : AbstractCommandHandler<bool>
    {
        private readonly byte[] _model;

        public ImportCardsHandler(byte[] model)
        {
            _model = model;
        }

        protected override bool Handle()
        {
            if (CommonStatistic.IsRun)
            {
                return true;
            }
            CommonStatistic.Init();
            CommonStatistic.CurrentProcess = "cards/importcards";
            CommonStatistic.CurrentProcessName = "импорт карты";
            CommonStatistic.IsRun = true;
            CommonStatistic.StartNew(() =>
            {
                //add bonus
                try
                {
                    var items = ProcessFile(_model, reader =>
                                                                {
                                                                    if (!(reader.CurrentRecord.Length == 9 || reader.CurrentRecord.Length ==10 )) return null;
                                                                    var t = new ImportCardDto
                                                                    {
                                                                        Mobile = reader[0].TryParseLong(),
                                                                        Email = reader[1],
                                                                        CardNumber = reader[2].TryParseLong(),
                                                                        LastName = reader[3],
                                                                        FirstName = reader[4],
                                                                        SecondName = reader[5],
                                                                        DateOfBirth = reader[6].TryParseDateTime(true),
                                                                        BonusAmount = reader[7].TryParseDecimal(),
                                                                        Grade = reader[8],
                                                                    };
                                                                    if (reader.CurrentRecord.Length == 10)
                                                                    {
                                                                        t.PurchaseSum = reader[9].TryParseDecimal();
                                                                    }
                                                                    return t;
                                                                }, encoding: Encoding.GetEncoding("windows-1251"));

                    CommonStatistic.TotalRow = items.Count;
                    foreach (var item in items)
                    {
                        if (string.IsNullOrWhiteSpace(item.Email)) continue;

                        AddCard(item);
                        CommonStatistic.RowPosition++;
                        CommonStatistic.TotalUpdateRow++;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    CommonStatistic.WriteLog(ex.Message);
                }
                finally
                {
                    CommonStatistic.IsRun = false;
                }
            });
            return true;
        }

        private void AddCard(ImportCardDto item)
        {
            var card = item.CardNumber != 0 ? CardService.Get(item.CardNumber) : null;
            if (card != null) {

                if (item.PurchaseSum.HasValue && item.PurchaseSum.Value != 0)
                {
                    var p = PurchaseService.HasImport(card.CardId, "импорт");
                    if (p == null)
                    {
                        p = new Purchase
                        {
                            PurchaseAmount = item.PurchaseSum.Value,
                            CardId = card.CardId,
                            CreateOn = DateTime.Now,
                            CreateOnCut = DateTime.Now,
                            CashAmount = 0,
                            MainBonusAmount = 0,
                            AdditionBonusAmount = 0,
                            NewBonusAmount = 0,
                            Comment = "импорт",
                            Status = EPuchaseState.Complete
                        };
                        PurchaseService.Add(p);
                    }
                }
                return;
            }

            var grade = GradeService.Get(item.Grade);
            var gradeid = grade != null ? grade.Id : BonusSystem.DefaultGrade;

            var customer = CustomerService.GetCustomerByEmail(item.Email);

            if (customer == null)
            {
                customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                {
                    LastName = item.LastName,
                    FirstName = item.FirstName,
                    Patronymic = item.SecondName,
                    BirthDay = item.DateOfBirth,
                    EMail = item.Email,
                    Phone = item.Mobile.ToString(),
                    StandardPhone = item.Mobile
                };
                customer.Id = CustomerService.InsertNewCustomer(customer);
            }
            else
            {
                var cardHave = CardService.Get(customer.Id);
                if (cardHave != null) return;
            }

            if (customer.Id == Guid.Empty) return;

            if (item.CardNumber == 0)
            {
                item.CardNumber = BonusSystemService.GenerateCardNumber(0);
            }

            card = new Card
            {
                CardNumber = item.CardNumber,
                CardId = customer.Id,
                BonusAmount = item.BonusAmount,
                CreateOn = DateTime.Now,
                GradeId = gradeid
            };

            CardService.Add(card);
            if (item.PurchaseSum.HasValue && item.PurchaseSum.Value != 0)
            {
                var p = PurchaseService.HasImport(customer.Id, "импорт");
                if (p == null)
                {
                    p = new Purchase
                    {
                        PurchaseAmount = item.PurchaseSum.Value,
                        CardId = customer.Id,
                        CreateOn = DateTime.Now,
                        CreateOnCut = DateTime.Now,
                        CashAmount = 0,
                        MainBonusAmount = 0,
                        AdditionBonusAmount = 0,
                        NewBonusAmount = 0,
                        Comment = "импорт",
                        Status = EPuchaseState.Complete
                    };
                    PurchaseService.Add(p);
                }
            }
        }

        private List<TResult> ProcessFile<TResult>(byte[] data, Func<CsvReader, TResult> function, TResult defaultValue = default(TResult), Encoding encoding = null, string delimetr = ";", bool hasHeaderRecord = true)
        {
            var result = new List<TResult>();
            using (var csv = new CsvReader(new StreamReader(new MemoryStream(data), encoding ?? Encoding.UTF8)))
            {
                csv.Configuration.Delimiter = delimetr;
                csv.Configuration.HasHeaderRecord = hasHeaderRecord;
                while (csv.Read())
                {
                    var item = function != null ? function(csv) : defaultValue;
                    if (item != null)
                        result.Add(item);
                }
            }
            return result;
        }

        private class ImportCardDto
        {
            public string Email { get; set; }
            public long CardNumber { get; set; }
            public long Mobile { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public decimal BonusAmount { get; set; }
            public string Grade { get; set; }
            public decimal? PurchaseSum { get; set; }
        }
    }
}
