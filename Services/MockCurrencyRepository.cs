using System;
using DomainLayer;
using DomainLayer.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository;
using Microsoft.EntityFrameworkCore;

namespace Services.CurrencyServices
{
    public class MockCurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyContext _context;

        public MockCurrencyRepository(CurrencyContext context)
        {
            _context = context;
        }



        public List<GettingDetails> GetCurrency(string name)
        {
            var currencies = _context.Currency.Where
                           (pp => pp.Name.Contains(name)).ToList();

            return ListCurrencies(currencies);

        }

        public List<GettingDetails> GetAllCurrencies()
        {
            var currencies = _context.Currency.ToList();

            return ListCurrencies(currencies);
        }

        public List<GettingDetails> ListCurrencies(List<Currency> currencies)
        {
            List<GettingDetails> list = new List<GettingDetails>();

            for (int i = 0; i < currencies.Count; i++)
            {
                GettingDetails currencydetails = new GettingDetails();

                currencydetails.Name = currencies[i].Name;
                currencydetails.Sign = currencies[i].Sign;
                currencydetails.Rate = _context.ExchangeHistory.Where
                                    (pp => pp.CurrencyId == currencies[i].CurrencyId)
                                    .OrderByDescending(pp => pp.ExchangeDate)
                                    .Select(pp => pp.CurruencyRate).FirstOrDefault();

                currencydetails.Date = (DateTime)_context.ExchangeHistory.Where
                                   (pp => pp.CurrencyId == currencies[i].CurrencyId)
                                   .OrderByDescending(pp => pp.ExchangeDate)
                                   .Select(pp => pp.ExchangeDate).FirstOrDefault();

                list.Add(currencydetails);
            }

            return list;


        }

        public List<GettingDetails> GetHighestCurrencies(int count)
        {
            int sortType = 0;

            return orderCurrencies(sortType, count);

        }

        public List<GettingDetails> GetLowestCurrencies(int count)
        {
            int sortType = 1;

            return orderCurrencies(sortType, count);

        }
        public List<GettingDetails> orderCurrencies(int sortType, int count)
        {

            var currencies = _context.Currency.ToList();
            List<GettingDetails> list = new List<GettingDetails>();

            if (sortType == 1)
                list = ListCurrencies(currencies).OrderBy(pp => pp.Rate).Take(count).ToList();

            else
                list = ListCurrencies(currencies).OrderByDescending(pp => pp.Rate).Take(count).ToList();


            return list;
        }



        public List<string> GetMostImprovedCurrencies(int count, DateTime Start, DateTime End)
        {
            int improvedType = 1;
            return improvedCurrencies(improvedType, count, Start, End);

        }


        public List<string> GetLeastImprovedCurrencies(int count, DateTime Start, DateTime End)
        {
            int improvedType = 0;
            return improvedCurrencies(improvedType, count, Start, End);


        }



        public List<string> improvedCurrencies(int improvedType, int count, DateTime Start, DateTime End)
        {
            var firstCurrencies = _context.ExchangeHistory.Where
                                 (pp => pp.ExchangeDate == Start)
                                 .OrderBy(pp => pp.CurrencyId).ToList();

            List<ExchangeHistory> secondCurrencies = new List<ExchangeHistory>();

            for (int i = 0; i < firstCurrencies.Count; i++)
            {
                var find = _context.ExchangeHistory.Where
                    (pp => pp.CurrencyId == firstCurrencies[i].CurrencyId && pp.ExchangeDate == End)
                           .FirstOrDefault();

                if (find != null)
                    secondCurrencies.Add(find);
            }


            double rate = 0;
            List<string> mostImproved = new List<string>();
            List<string> leastImproved = new List<string>();

            for (int i = 0; i < secondCurrencies.Count; i++)
            {

                rate = firstCurrencies[i].CurruencyRate - secondCurrencies[i].CurruencyRate;

                string name = _context.Currency.Where
                            (pp => pp.CurrencyId == firstCurrencies[i].CurrencyId)
                             .Select(pp => pp.Name).FirstOrDefault();

                if (rate >= 0)
                    mostImproved.Add(name);
                else
                    leastImproved.Add(name);

            }

            if (improvedType == 1)

                return mostImproved.Take(count).ToList();

            else
                return leastImproved.Take(count).ToList();


        }


        public double ConvertAmount(double count, string fromSign, string toSign)
        {
            double rate, operation, value;

            rate = getRate(fromSign);
            operation = count / rate;

            if (toSign == "USD")
            {
                value = (double)System.Math.Round(operation, 2);
                return value;
            }

            rate = getRate(toSign);
            operation *= rate;
            value = (double)System.Math.Round(operation, 2);

            return value;

        }
        public double getRate(string sign)
        {
            int signId = _context.Currency.Where(pp => pp.Sign.Equals(sign))
                          .FirstOrDefault().CurrencyId;

            double rate = _context.ExchangeHistory.Where(pp => pp.CurrencyId == signId)
                          .OrderByDescending(pp => pp.ExchangeDate)
                          .FirstOrDefault().CurruencyRate;
            return rate;
        }

        public AddingDetails Add(AddingDetails currency)
        {
            Currency newCurrency = new Currency();
            int type = 1;
            AddToCurrency(type, newCurrency, currency);

            int id = _context.Currency.Where
                    (pp => pp.Name == currency.Name)
                    .Select(pp => pp.CurrencyId).FirstOrDefault();

            AddToExchangeHistory(id, currency.Rate, currency.Date);

            return currency;

        }

        public AddingDetails Update(int id, AddingDetails currency)
        {

            Currency oldCurrency = _context.Currency.Find(id);
            int type = 0;
            AddToCurrency(type, oldCurrency, currency);


            double oldRate = _context.ExchangeHistory.Where
                           (pp => pp.CurrencyId == id)
                           .OrderByDescending(pp => pp.ExchangeDate)
                           .FirstOrDefault().CurruencyRate;

            if (currency.Rate != oldRate)

                AddToExchangeHistory(id, currency.Rate, currency.Date);

            return currency;
        }

        public void AddToCurrency(int type, Currency currency, AddingDetails newData)
        {

            currency.Name = newData.Name;
            currency.Sign = newData.Sign;
            currency.IsActive = newData.IsActive;

            if (type == 1)
                _context.Currency.Add(currency);
            else
                _context.Entry(currency).State = EntityState.Modified;

            _context.SaveChanges();
        }
        public void AddToExchangeHistory(int id, double currencyRate, DateTime currencyDate)
        {
            ExchangeHistory exchangeHistory = new ExchangeHistory();

            exchangeHistory.CurrencyId = id;
            exchangeHistory.ExchangeDate = currencyDate;
            exchangeHistory.CurruencyRate = currencyRate;

            _context.ExchangeHistory.Add(exchangeHistory);
            _context.SaveChanges();

        }


        public Currency Delete(int Id)
        {
            Currency currency = _context.Currency.Find(Id);
            if (currency != null)
            {

                currency.IsActive = false;
                _context.Entry(currency).Property("IsActive").IsModified = true;
                _context.SaveChanges();

            }
            return currency;
        }
    }
}
