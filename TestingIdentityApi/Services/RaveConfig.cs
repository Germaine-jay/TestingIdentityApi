namespace TestingIdentityApi.Services
{
    public class RaveConfig
    {
        private string pbKey;
        private object sCKey;
        private bool v;

        public RaveConfig(string pbKey, object sCKey, bool v)
        {
            this.pbKey = pbKey;
            this.sCKey = sCKey;
            this.v = v;
        }
    }
}