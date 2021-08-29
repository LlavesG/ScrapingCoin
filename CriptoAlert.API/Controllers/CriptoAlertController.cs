using CriptoAlert.API.Logic;
using CriptoAlert.API.Model;
using CriptoScrapingLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CriptoAlert.API.Controllers
{
    [ApiController]
    [Route("CriptoAlert")]
    public class CriptoAlertController : ControllerBase
    {
        AlertCriptLogic _CriptoAlertLogic = new AlertCriptLogic();

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<CriptoAlertController> _logger;

        public CriptoAlertController(ILogger<CriptoAlertController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetValuesAllCoins")]
        public IActionResult  GetValuesAllCoins()
        {
            try { 
            ValueCoins result = new ValueCoins();
            //CriptoScraping criptoScrapingLibrary = new CriptoScraping("https://es.investing.com/crypto/");
            CriptoScraping criptoScrapingLibrary = new CriptoScraping("https://es.investing.com/crypto/currencies");
            result.resultCoins = criptoScrapingLibrary.GetCriptosValues();
            _CriptoAlertLogic.SendEmails(result.resultCoins);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Unauthorized();
            }

        }
        [HttpGet("GetDropListCripto")]
        public IActionResult GetDropListCripto()
        {
            try
            {
                ValueDropCoins result = new ValueDropCoins();
                CriptoScraping criptoScrapingLibrary = new CriptoScraping("https://coinmarketcap.com/es/all/views/all/");
                result.resulDroptCoins = criptoScrapingLibrary.GetCriptosDropList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

        }
        [HttpPost("SaveUserCoin")]
        public IActionResult SaveUserCoin([FromBody] UserCoin _userCoin)
        {
            bool result;
            try
            {
                result=_CriptoAlertLogic.SaveUserCoin(_userCoin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

        }
    }
}
