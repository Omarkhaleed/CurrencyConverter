using DomainLayer;
using DomainLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi

{
    [Route("Currency")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {

        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }


        [HttpGet]
        [Route("GetCurrency")]
        public IActionResult GetCurrency(string name)
        {
            try
            {

                List<GettingDetails> list = new List<GettingDetails>();
                list = _currencyRepository.GetCurrency(name);
                return Ok(list);

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }

        }


        [HttpGet]
        [Route("GetAllCurruncies")]
        public IActionResult GetAllCurrencies()
        {
            try
            {
                var currencies = _currencyRepository.GetAllCurrencies();
                if (currencies.Count() != 0)
                    return Ok(currencies);
                else
                    return NotFound(" Sorry, there is no currencies");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");

            }
        }


        [HttpGet]
        [Route("GetHighestCurrencies")]
        public IActionResult GetHighestCurrencies(int count)
        {
            try
            {
                var currency = _currencyRepository.GetHighestCurrencies(count);
                return Ok(currency);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }

        }


        [HttpGet]
        [Route("GetLowestCurrencies")]
        public IActionResult GetLowestCurrencies(int count)
        {
            try
            {

                var currency = _currencyRepository.GetLowestCurrencies(count);
                return Ok(currency);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet]
        [Route("GetMostImprovedCurrencies")]
        public IActionResult GetMostImprovedCurrencies(int count, DateTime Start, DateTime End)
        {
            try
            {
                List<string> ll2 = new List<string>();
                ll2 = _currencyRepository.GetMostImprovedCurrencies(count, Start, End).ToList();

                return Ok(ll2);
            }

            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }

        }


        [HttpGet]
        [Route("GetLeastImprovedCurrencies")]
        public IActionResult GetLeastImprovedCurrencies(int count, DateTime Start, DateTime End)
        {
            try
            {
                List<string> ll2 = new List<string>();
                ll2 = _currencyRepository.GetLeastImprovedCurrencies(count, Start, End).ToList();
                return Ok(ll2);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        [Route("ConvertAmount")]
        public IActionResult ConvertAmount(int count, string fromSign, string toSign)
        {

            try
            {
                double rate = _currencyRepository.ConvertAmount(count, fromSign, toSign);
                return Ok($"{count} {fromSign} = {rate} {toSign} ");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }


        }


        [HttpPost]
        [Route("AddCurrency")]
        public IActionResult Add([FromBody] AddingDetails currency)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AddingDetails AddedCurrency = _currencyRepository.Add(currency);
                    return Ok(AddedCurrency);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");

                }

            }
            else
                return UnprocessableEntity(ModelState);

        }


        [HttpPut]
        [Route("UpdateCurrency")]
        public IActionResult Update(int id, [FromBody] AddingDetails currency)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AddingDetails updatedCurrency = _currencyRepository.Update(id, currency);
                    return Ok(updatedCurrency);
                }
                catch (Exception ex)
                {
                    return UnprocessableEntity(ModelState);

                }

            }

            return UnprocessableEntity(ModelState);

        }



        [HttpDelete]
        [Route("DeleteCurrency")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var currency = _currencyRepository.Delete(Id);
                if (currency is null)
                    return NotFound();
                return Ok(currency.Name + " is deleted");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

    }
}

