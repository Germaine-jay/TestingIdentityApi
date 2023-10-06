using Flutterwave.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PayStack.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TestingIdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlutterController : ControllerBase
    {
        private static string PbKey = "FLWSECK_TEST-SANDBOXDEMOKEY-X";
        private static string paystackPbKey = "sk_test_9ff048e6b5abd10c034dd39d1b092cbd9e67716c";
        private static string ScKey = "pass your secret key here";

        public string reference = Guid.NewGuid().ToString();

        //public IHttpClientFactory _httpClientFactory;

        private readonly HttpClient _httpClient;
        private readonly string _healthCheckEndpoint;

        public FlutterController()
        {
            _httpClient = new HttpClient();
        }

        [HttpPost]
        public async Task<IActionResult> Banks()
        {
            var client = new FlutterwaveApi(PbKey);
            var banks = client.Banks.GetBanks(Country.Nigeria);
            return Ok(banks);
        }


        [HttpPost("paystackpayment", Name = "paystackpayment")]
        public async Task<IActionResult> PaystackPayment()
        {
            var charge = new TransactionInitializeRequest
            {
                Reference = Guid.NewGuid().ToString(),
                AmountInKobo = 10000,
                Currency = "NGN",
                Email = "jsonosii097@gmail.com",
                Bearer = "johnson osii",
                CallbackUrl = "https://Localhost:7085/api/Flutter/VerifyPayment",
                TransactionCharge = 15
            };

            //var client = new FlutterwaveApi(PbKey);
            //var banks = client.Payments.InitiatePayment(charge.Reference, charge.Amount, charge.CallbackUrl, charge.CustomerName, charge.CustomerEmail, "0913334567", "bills", "food", "u", Currency.NigerianNaira, splitPaymentRequests:null);

            //_refrence = charge.Reference;
            PayStackApi payStack = new PayStackApi(paystackPbKey);
            var result = payStack.Transactions.Initialize(charge);
            return Ok(result);
        }


        [HttpGet("VerifyPayment", Name = "VerifyPayment")]
        public async Task<IActionResult> VerifyPayment([FromQuery] string reference)
        {
            PayStackApi payStack = new PayStackApi(paystackPbKey);

            var result = payStack.Transactions.Verify(reference);
            var say = new FlutterwaveApi(paystackPbKey);

            return Ok(result);
        }


        [HttpPost("cardpayment", Name = "cardpayment")]
        public async Task<IActionResult> CardPayment()
        {
            PayStackApi payStack = new PayStackApi(paystackPbKey);

            var card = payStack.Charge.ChargeCard("jsonosii097@gmail.com", "1500000", "507850785078507812", "081", "10", "24", "1111");
            var response = JsonConvert.SerializeObject(card.RawJson);
            var result = JsonConvert.DeserializeObject<dynamic>(response);

            return Ok(result);
        }


        [HttpPost("flutter", Name = "flutter")]
        public async Task<IActionResult> FlutterPayment()
        {
            var say = new FlutterwaveApi("FLWSECK_TEST-689f4305ab8e742e8c2008a985a5c647-X");

            var result = say.Payments.InitiatePayment(reference, 1000, "https://Localhost:7085/api/Flutter/FlutterVerify", "johnson", "jermaine.jay00@gmail.com", "0913444567", "cloth", "buy cloths", Currency.NigerianNaira.ToString());
            var response = $"{result}/n {reference}";
            Console.WriteLine(response);
            return Ok(result);
        }


        [HttpGet("FlutterVerify", Name = "FlutterVerify")]
        public async Task<IActionResult> VerifyFlutterPayment([FromQuery] string transaction_id)
        {
            var say = new FlutterwaveApi("FLWSECK_TEST-689f4305ab8e742e8c2008a985a5c647-X");
            var verify = say.Transactions.VerifyTransaction(int.Parse(transaction_id));
            return Ok(verify);
        }


        [HttpGet("Fluttercardpayment", Name = "Fluttercardpayment")]
        public async Task<IActionResult> Standard()
        {
            var Api = "FLWSECK_TEST-689f4305ab8e742e8c2008a985a5c647-X";

            var details = new
            {
                card_number = "5531886652142950",
                cvv = "564",
                expiry_month = "09",
                expiry_year = "32",
                currency = "NGN",
                amount = "1000",
                fullname = "Yolande Aglaé Colbert",
                email = "user@example.com",
                tx_ref = "MC-3243e",
                redirect_url = "https://www,flutterwave.ng",
                client = "Of8p6iJUVUezgvjUkjjJsP8aPd6CjHR3f9ptHiH5Q0+2h/FzHA/X1zPlDmRmH5v+GoLWWB4TqEojrKhZI38MSjbGm3DC8UPf385zBYEHZdgvQDsacDYZtFEruJqEWXmbvw9sUz+YwUHegTSogQdnXp7OGdUxPngiv6592YoL0YXa4eHcH1fRGjAimdqucGJPurFVu4sE5gJIEmBCXdESVqNPG72PwdRPfAINT9x1bXemI1M3bBdydtWvAx58ZE4fcOtWkD/IDi+o8K7qpmzgUR8YUbgZ71yi0pg5UmrT4YpcY2eq5i46Gg3L+fxFl4tauG9H4WBChF0agXtP4kjfhfYVD48N9Hrt"
            };


            using var fetch = new HttpClient();
            fetch.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Api);
            var json = JsonConvert.SerializeObject(details);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //fetch.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await fetch.PostAsync("https://api.flutterwave.com/v3/charges?type=card", data);
            //response.EnsureSuccessStatusCode();
            var a = response.IsSuccessStatusCode;
            var result = await response.Content.ReadAsStringAsync();

            var ret = JsonConvert.DeserializeObject<dynamic>(result);

            return Ok(ret);
        }



        [HttpGet("isworking", Name = "isworking")]
        public async Task<bool> IsServiceUpAsync()
        {
            //_httpClient.BaseAddress = new Uri("https://api.paystack.co/");
            //_httpClient.BaseAddress = new Uri("");

            var Api = "FLWSECK_TEST-689f4305ab8e742e8c2008a985a5c647-X";
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Api}");

            var response = await _httpClient.GetAsync("https://api.flutterwave.com/v3//ping");
            return response.IsSuccessStatusCode;
        }

    }
}
