using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Angular2Application3.Controllers
{ 
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
        // GET: /<controller>/
        [HttpGet("[action]/{city}")]
        //public IActionResult City(string city)
        //{
        //    return Ok(new { Temp = "12", Summary = "Barmy", City = "London" });
        //}
        public async Task<IActionResult> City(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid=f6c810b72d69b9224b8ddde39e19f6f9&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);

                    return Ok(new
                    {
                        Temp = rawWeather.Main.Temp,
                        Summary = string.Join(",", rawWeather.Weather.Select(x => x.Main)),
                        City = rawWeather.Name
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
    }

    public class OpenWeatherResponse
    {
        public string Name { get; set; }
        public IEnumerable<WeatherDescription> Weather { get; set; }
        public Main Main { get; set; }
    }

    public class WeatherDescription
    {
        public string Main { get; set; }
        public string Descriptin { get; set; }
    }

    public class Main
    {
        public string Temp { get; set; }
    }
}
