using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Middleware.Model.API;
using Middleware.Model.Local;
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

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK
                ? (response.Data ?? new List<APISesion>())
                : null;
        }

        public static bool PostSesion(VFPSesion sesion, string dbFolder)
        {
            var request = new RestRequest("sesion", Method.POST);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(sesion);

            var response = Client.Execute(request);
            Debug.WriteLine($"POST Sesion: [{sesion.numero}][{sesion.linea}] {response.Content}");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }

        public static bool PutSesion(int id, VFPSesion sesion, string dbFolder)
        {
            var request = new RestRequest("sesion/{id}", Method.PUT);
            request.AddHeader("authorization", AuthKey);
            request.AddUrlSegment("id", id.ToString());
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(sesion);

            var response = Client.Execute(request);
            Debug.WriteLine($"PUT Sesion: [{id}][{sesion.numero}][{sesion.linea}] {response.Content}");

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

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK
                ? (response.Data ?? new List<APIMaeCue>())
                : null;
        }

        public static bool PostMaeCue(VFPMaeCue maeCue, string dbFolder)
        {
            var request = new RestRequest("maecue", Method.POST);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(maeCue);

            var response = Client.Execute(request);
            Debug.WriteLine($"POST MaeCue: [{maeCue.codigo}] {response.Content}");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }

        public static bool PutMaeCue(int id, VFPMaeCue maeCue, string dbFolder)
        {
            var request = new RestRequest("maecue/{id}", Method.PUT);
            request.AddHeader("authorization", AuthKey);
            request.AddUrlSegment("id", id.ToString());
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(maeCue);

            var response = Client.Execute(request);
            Debug.WriteLine($"PUT MaeCue: [{id}][{maeCue.codigo}] {response.Content}");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }
        #endregion
        #region Tabanco
        public static List<APITabanco> GetTabanco(string dbFolder)
        {
            var request = new RestRequest("tabanco", Method.GET);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);

            var response = Client.Execute<List<APITabanco>>(request);
            Debug.WriteLine("GET: Tabanco table fetched from API");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK
                ? (response.Data ?? new List<APITabanco>())
                : null;
        }

        public static bool PostTabanco(VFPTabanco tabanco, string dbFolder)
        {
            var request = new RestRequest("tabanco", Method.POST);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(tabanco);

            var response = Client.Execute(request);
            Debug.WriteLine($"POST Tabanco: [{tabanco.codbanco}] {response.Content}");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }

        public static bool PutTabanco(int id, VFPTabanco tabanco, string dbFolder)
        {
            var request = new RestRequest("tabanco/{id}", Method.PUT);
            request.AddHeader("authorization", AuthKey);
            request.AddUrlSegment("id", id.ToString());
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(tabanco);

            var response = Client.Execute(request);
            Debug.WriteLine($"PUT Tabanco: [{id}][{tabanco.codbanco}] {response.Content}");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }
        #endregion
        #region Tabaux10
        public static List<APITabaux10> GetTabaux10(string dbFolder)
        {
            var request = new RestRequest("tabaux10", Method.GET);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);

            var response = Client.Execute<List<APITabaux10>>(request);
            Debug.WriteLine("GET: Tabaux10 table fetched from API");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK
                ? (response.Data ?? new List<APITabaux10>())
                : null;
        }

        public static bool PostTabaux10(VFPTabaux10 tabaux10, string dbFolder)
        {
            var request = new RestRequest("tabaux10", Method.POST);
            request.AddHeader("authorization", AuthKey);
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(tabaux10);

            var response = Client.Execute(request);
            Debug.WriteLine($"POST Tabaux10: [{tabaux10.kod}] {response.Content}");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }
        public static bool PutTabaux10(int id, VFPTabaux10 tabaux10, string dbFolder)
        {
            var request = new RestRequest("tabaux10/{id}", Method.PUT);
            request.AddHeader("authorization", AuthKey);
            request.AddUrlSegment("id", id.ToString());
            request.AddParameter("vfptable", dbFolder);
            request.AddObject(tabaux10);

            var response = Client.Execute(request);
            Debug.WriteLine($"PUT Tabaux10: [{id}][{tabaux10.kod}] {response.Content}");

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }
        #endregion
    }
}
