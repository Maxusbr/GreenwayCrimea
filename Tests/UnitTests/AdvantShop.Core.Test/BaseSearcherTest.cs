using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Core.Services.FullSearch.Core;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace AdvantShop.Core.Test
{
    [TestFixture]
    public class BaseSearcherTest
    {
        public class TestDocument : BaseDocument
        {
            private string _name;
            [SearchField]
            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    AddParameterToDocumentNoStoreAnalyzed(_name);
                }
            }
        }

        string path = "/index";
        string fullPath = "";

        [SetUp]
        public void Init()
        {
            var data = new List<TestDocument>
            {
                new TestDocument { Id=1, Name="Платье \"City Nights\"" },
                new TestDocument { Id=2, Name="Изящное платье" },
                new TestDocument { Id=3, Name="Элегантное вечернее платье" },
                new TestDocument { Id=4, Name="Элегантное вечернее платье ddd44-543d" },
                new TestDocument { Id=5, Name="\"royal canin для кошек\"" },
                new TestDocument { Id=6, Name="Royal Canin Indoor 27 / Роял Канин сухой корм для кошек" },
                new TestDocument { Id=7, Name="Royal Canin Instinctive / Роял Канин влажный корм для кошек (в желе)" },
                new TestDocument { Id=8, Name="Sony KDL-52Z4500 Серо-черный Full HD 3D (HI-FI)" },
                new TestDocument { Id=9, Name= "Пила дисковая 5704R Makita 1200/4,6/190*30мм/подкл.пылесоса/диск (1/2)" },
                new TestDocument { Id=10, Name="Бензопила CHAMPION 137-16\"-3/8-1.3-56 (1.55кВт легкий старт 4.5 кг). CHAMPION. Китай. S3716" },
                new TestDocument { Id=11, Name="Tom.m Сандалии A-T49-79-B (Том М)-0" },
                new TestDocument { Id=12, Name="samsung mlt-d307u" },
                new TestDocument { Id=13, Name="Свечи восковые для домашней молитвы" },
                new TestDocument { Id=14, Name="икона в рамке" },
                new TestDocument { Id=15, Name="побелить стену" }
            };

            var dir = Directory.GetCurrentDirectory();
            fullPath = dir + path;           
            using (var writer = new BaseWriter<TestDocument>(fullPath))
            {
                writer.AddUpdateItemsToIndex(data);
            }
        }

        [TearDown]
        public void Cleanup()
        {
            Directory.Delete(fullPath, true);
        }

        #region strong
        [Test]
        public void SearchStrongQuotes()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.StrongPhase, fullPath))
            {
                var res = reader.SearchItems("Платье \"City Nights\"");
                Assert.AreEqual(1, res.SearchResultItems.Count);
                Assert.AreEqual(1, res.SearchResultItems[0].Id);

            }
        }

        [Test]
        public void SearchStrongSimpleSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.StrongPhase, fullPath))
            {
                var res = reader.SearchItems("Элегантное вечернее платье");
                Assert.AreEqual(2, res.SearchResultItems.Count);
                Assert.AreEqual(3, res.SearchResultItems[0].Id);
            }
        }

        [Test]
        public void SearchStrongMiltiSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.StrongPhase, fullPath))
            {
                var res = reader.SearchItems("платье");
                Assert.AreEqual(4, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchStrongNumericFail()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.StrongPhase, fullPath))
            {
                var res = reader.SearchItems("44");
                Assert.AreNotEqual(1, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchStrongNumericSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.StrongPhase, fullPath))
            {
                var res = reader.SearchItems("ddd44-543d");
                Assert.AreEqual(1, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchStrongPartNumericFail()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.StrongPhase, fullPath))
            {
                var res = reader.SearchItems("4-54");
                Assert.AreEqual(0, res.SearchResultItems.Count);
            }
        }
        #endregion

        #region SepareteWords
        [Test]
        public void SearchSepareteWordsQuotes()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.SepareteWords, fullPath))
            {
                var res = reader.SearchItems("Платье \"City Nights\"");
                Assert.AreEqual(4, res.SearchResultItems.Count);
                Assert.AreEqual(1, res.SearchResultItems[0].Id);

            }
        }

        [Test]
        public void SearchSepareteWordsSimpleSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.SepareteWords, fullPath))
            {
                var res = reader.SearchItems("Элегантное вечернее платье");
                Assert.AreEqual(4, res.SearchResultItems.Count);
                Assert.AreEqual(3, res.SearchResultItems[0].Id);
            }
        }

        [Test]
        public void SearchSepareteWordsMiltiSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.SepareteWords, fullPath))
            {
                var res = reader.SearchItems("платье");
                Assert.AreEqual(4, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchSepareteWordsNumericFail()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.SepareteWords, fullPath))
            {
                var res = reader.SearchItems("44");
                Assert.AreNotEqual(1, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchSepareteWordsNumericSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.SepareteWords, fullPath))
            {
                var res = reader.SearchItems("ddd44-543d");
                Assert.AreEqual(1, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchStrongPhasePartNumericFail()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.SepareteWords, fullPath))
            {
                var res = reader.SearchItems("4-54");
                Assert.AreEqual(2, res.SearchResultItems.Count);
            }
        }
        #endregion


        #region WordsStartFrom
        [Test]
        public void SearchWordsStartFromQuotes()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsStartFrom, fullPath))
            {
                var res = reader.SearchItems("Платье \"City Nights\"");
                Assert.AreEqual(4, res.SearchResultItems.Count);
                Assert.AreEqual(1, res.SearchResultItems[0].Id);

            }
        }

        [Test]
        public void SearchWordsStartFromSimpleSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsStartFrom, fullPath))
            {
                var res = reader.SearchItems("Элегантное вечернее платье");
                Assert.AreEqual(4, res.SearchResultItems.Count);
                Assert.AreEqual(3, res.SearchResultItems[0].Id);
            }
        }

        [Test]
        public void SearchWordsStartFromMiltiSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsStartFrom, fullPath))
            {
                var res = reader.SearchItems("платье");
                Assert.AreEqual(4, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchWordsStartFromNumericFail()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsStartFrom, fullPath))
            {
                var res = reader.SearchItems("44");
                Assert.AreNotEqual(1, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchWordsStartFromNumericSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsStartFrom, fullPath))
            {
                var res = reader.SearchItems("ddd44-543d");
                Assert.AreEqual(1, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchWordsStartFromPartNumericFail()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsStartFrom, fullPath))
            {
                var res = reader.SearchItems("4-54");
                Assert.AreEqual(3, res.SearchResultItems.Count);
            }
        }

        #endregion


        #region WordsBetween
        [Test]
        public void SearchWordsBetweenQuotes()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsBetween, fullPath))
            {
                var res = reader.SearchItems("Платье \"City Nights\"");
                Assert.AreEqual(4, res.SearchResultItems.Count);
                Assert.AreEqual(1, res.SearchResultItems[0].Id);

            }
        }

        [Test]
        public void SearchWordsBetweenSimpleSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsStartFrom, fullPath))
            {
                var res = reader.SearchItems("Элегантное вечернее платье");
                Assert.AreEqual(4, res.SearchResultItems.Count);
                Assert.AreEqual(3, res.SearchResultItems[0].Id);
            }
        }

        [Test]
        public void SearchWordsBetweenMiltiSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsBetween, fullPath))
            {
                var res = reader.SearchItems("платье");
                Assert.AreEqual(4, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchWordsBetweenNumericFail()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsBetween, fullPath))
            {
                var res = reader.SearchItems("7");
                Assert.AreNotEqual(1, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchWordsBetweenNumericSucsess()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsBetween, fullPath))
            {
                var res = reader.SearchItems("45");
                Assert.AreEqual(1, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void SearchWordsBetweenPartNumericFail()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsBetween, fullPath))
            {
                var res = reader.SearchItems("4-54");
                Assert.AreEqual(5, res.SearchResultItems.Count);
            }
        }
        #endregion


        public void Test_SUPPORT_1623()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.StrongPhase, fullPath))
            {
                var res = reader.SearchItems("A-T49-79-B (Том М)-0");
                Assert.AreEqual(1, res.SearchResultItems.Count);
            }
        }

        public void Test_SUPPORT_1674()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsBetween, fullPath))
            {
                var res = reader.SearchItems("307");
                Assert.AreEqual(1, res.SearchResultItems.Count);
            }
        }

        [Test]
        public void Test_Blagochectie1()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsBetween, fullPath))
            {
                var res = reader.SearchItems("Свечи для домашней молитвы");
                Assert.AreEqual(1, res.SearchResultItems.Count); // не находит "Свечи восковые для домашней молитвы"
            }
        }

        [Test]
        public void Test_Blagochectie2()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsBetween, fullPath))
            {
                var res = reader.SearchItems("икона в рамке");
                Assert.AreEqual(1, res.SearchResultItems.Count); //находит все слова на в*, как-то не учтывать предлоги
            }
        }


        [Test]
        public void Test_RusMorfology()
        {
            using (var reader = new BaseSearcher<TestDocument>(100, ESearchDeep.WordsBetween, fullPath))
            {
                var res = reader.SearchItems("побеливший слона");
                Assert.AreEqual(1, res.SearchResultItems.Count); 
            }
        }


    }
}
