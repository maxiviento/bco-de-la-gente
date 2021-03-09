using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportador.Test
{
    public class StubData
    {
        private static string[] Urls = new[]
        {
            "symantec.com",
            "hao123.com",
            "dyndns.org",
            "archive.org",
            "artisteer.com",
            "ted.com",
            "ebay.co.uk",
            "geocities.jp",
            "stanford.edu",
            "si.edu",
            "narod.ru",
            "photobucket.com",
            "chron.com",
            "gravatar.com",
            "wisc.edu",
            "vk.com",
            "google.com",
            "ucsd.edu",
            "latimes.com",
            "nydailynews.com",
            "toplist.cz",
            "scientificamerican.com",
            "time.com",
            "ustream.tv",
            "1und1.de",
            "qq.com",
            "hhs.gov",
            "quantcast.com",
            "smh.com.au",
            "is.gd"
        };

        public static IList<Ejemplo> GetStubData()
        {
            var ejemplo = new List<Ejemplo>();
            Random random = new Random();


            for (int i = 0; i < 20; i++)
            {
                ejemplo.Add(new Ejemplo()
                {
                    String = Urls[random.Next(0, 30)],
                    Doble = random.NextDouble(),
                    Entero = random.Next(0, 30),
                    Fecha = DateTime.Now.AddDays(random.Next(0, 30))
                });
            }

            return ejemplo;
        }
    }
}