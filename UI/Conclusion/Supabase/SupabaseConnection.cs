using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintToPACSDemo.UI.Conclusion
{
    public sealed class SupabaseConnection
    {
        private static Supabase.Client _client;

        public static Supabase.Client Client
        {
            get
            {
                if (_client == null)
                {
                    var url = ConfigurationManager.AppSettings["SUPABASE_URL"];
                    var key = ConfigurationManager.AppSettings["SUPABASE_KEY"];

                    var options = new Supabase.SupabaseOptions
                    {
                        AutoConnectRealtime = true
                    };

                    _client = new Supabase.Client(url, key, options);
                    Task.Factory.StartNew(async () =>
                    {
                        await _client.InitializeAsync();
                    });
                }
                return _client;
            }
        }
    }
}
