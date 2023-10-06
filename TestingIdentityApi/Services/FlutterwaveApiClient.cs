using Flutterwave.Net;

namespace TestingIdentityApi.Services
{
    public static  class FlutterwaveApiClient
    {
        private static string PbKey = "FLWSECK_TEST-SANDBOXDEMOKEY-X";
        private static string ScKey = "pass your secret key here";
        //public static FlutterwaveApiClient() { }

        public static async Task<object> BankPayment()
        {
            var raveConfig = new RaveConfig(PbKey, ScKey, false);
     
            var client = new FlutterwaveApi(PbKey);
            var banks = client.Banks.GetBanks(Country.Nigeria);
            return banks;
        }
    }
}
