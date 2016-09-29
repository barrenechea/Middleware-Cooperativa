using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Middleware.Models.API;
using Middleware.Models.Local;
using RestSharp;

namespace Middleware.Controller
{
    public static class RestController
    {
        private static readonly RestClient Client = new RestClient("http://localhost:8000/api");
        private static readonly string AuthKey = "@0w*mNg*bYHdXH6-";


        public static List<APISesion> GetSesion(string dbFolder)
        {
            var request = new RestRequest("sesion", Method.GET);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);

            var response = Client.Execute<List<APISesion>>(request);
            Debug.WriteLine("Sesion table fetched from API");

            return response.Data ?? new List<APISesion>();
        }

        public static bool PostSesion(VFPSesion sesion, string dbFolder)
        {
            var request = new RestRequest("sesion", Method.POST);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(sesion);

            var response = Client.Execute(request);
            Debug.WriteLine($"[{sesion.numero}] {response.Content}");

            return true;
        }
    }
}
