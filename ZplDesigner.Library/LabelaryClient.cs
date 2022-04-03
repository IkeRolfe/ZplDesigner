using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZplDesigner.Library
{
    public class LabelaryClient
    {
        public static async Task<Bitmap> GetImage(string zpl)
        {
            using var client = new HttpClient();
            var dimensions = "4x8";
            var response = client.PostAsync($@"http://api.labelary.com/v1/printers/8dpmm/labels/{dimensions}/0/",
                new ByteArrayContent(Encoding.UTF8.GetBytes(zpl)));
            
            var image = new Bitmap(await response.Result.Content.ReadAsStreamAsync());
            return image;
        }
    }
}
