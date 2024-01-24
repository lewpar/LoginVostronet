namespace LoginVostronet
{
    internal class Program
    {
        static HttpClient? client;

        static async Task Main(string[] args)
        {
            if(args.Length < 2)
            {
                Console.WriteLine("You need to supply a username and password as arguments:");
                Console.WriteLine("LoginVostronet <username> <password>");
                return;
            }

            string username = args[0];
            string password = args[1];

            client = new HttpClient();

            Console.WriteLine("Testing internet connection..");

            var getResponse = await client.GetStringAsync("http://google.com/");

            if(!getResponse.Contains("https://api.vostronet.com/api/auth/hotspot?rlogin=true"))
            {
                Console.WriteLine("VostroNet redirect not detected, no need to login.");
                return;
            }

            Console.WriteLine("VostoNet redirect detected.");

            Console.WriteLine("Logging into hotspot captive portal..");

            var hotspotContent = new StringContent($"radius11=hotspot&dst=https%3A%2F%2Fportal.vostronet.com%2F&username={username}&password={password}", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            var hotspotResponse = await client.PostAsync("https://wifi.authentication.technology/login", hotspotContent);

            if (!hotspotResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed login to hotspot with status code: {hotspotResponse.StatusCode}");
                Console.WriteLine(await hotspotResponse.Content.ReadAsStringAsync());
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Logged into hotspot captive portal.");

            Console.ReadLine();
        }
    }
}