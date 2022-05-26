using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainLayer;
using DomainLayer.DTOs;
namespace Repository
{
    public  interface ICurrencyRepository
    {
        List<GettingDetails> GetCurrency(string name);
        List<GettingDetails> GetAllCurrencies();
        List<GettingDetails> GetHighestCurrencies(int count);
        List<GettingDetails> GetLowestCurrencies(int count);
        List<string> GetMostImprovedCurrencies(int count, DateTime Start, DateTime End);
        List<string> GetLeastImprovedCurrencies(int count, DateTime Start, DateTime End);
        double ConvertAmount(double count, string signFrom, string signTo);
        AddingDetails Add(AddingDetails currency);
        AddingDetails Update(int id, AddingDetails currency);
        Currency Delete(int Id);

    }
}
