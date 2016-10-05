using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        private const string AuthKey = "@0w*mNg*bYHdXH6-";

        #region Sesion
        public static List<APISesion> GetSesion(string dbFolder)
        {
            var request = new RestRequest("sesion", Method.GET);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);

            var response = Client.Execute<List<APISesion>>(request);
            Debug.WriteLine("GET: Sesion table fetched from API");

            return response.Data ?? new List<APISesion>();
        }

        public static bool PostSesion(VFPSesion sesion, string dbFolder)
        {
            var request = new RestRequest("sesion", Method.POST);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(sesion);

            var response = Client.Execute(request);
            Debug.WriteLine($"POST: [{sesion.numero}] {response.Content}");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }
        #endregion
        #region MaeCue
        public static List<APIMaeCue> GetMaeCue(string dbFolder)
        {
            var request = new RestRequest("maecue", Method.GET);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);

            var response = Client.Execute<List<APIMaeCue>>(request);
            Debug.WriteLine("GET: MaeCue table fetched from API");

            return response.Data ?? new List<APIMaeCue>();
        }

        public static bool PostMaeCue(VFPMaeCue maeCue, string dbFolder)
        {
            var request = new RestRequest("maecue", Method.POST);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(maeCue);

            var response = Client.Execute(request);
            Debug.WriteLine($"POST: [{maeCue.codigo}] {response.Content}");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }
        #endregion
    }
}
