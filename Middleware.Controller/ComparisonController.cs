using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Middleware.Models.API;
using Middleware.Models.Local;

namespace Middleware.Controller
{
    public static class ComparisonController
    {
        #region Sesion

        public static List<VFPSesion> GetNewObjects(List<VFPSesion> vfpList, List<APISesion> apiList)
        {
            throw new NotImplementedException();
        }
        public static List<Tuple<int, VFPSesion>> GetUpdatedObjects(List<VFPSesion> vfpList, List<APISesion> apiList)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Tabanco
        public static List<VFPTabanco> GetNewObjects(List<VFPTabanco> vfpList, List<APITabanco> apiList)
        {
            return vfpList.Where(vfp => !apiList.Exists(api => api.codbanco.Equals(vfp.codbanco))).ToList();
        }
        public static List<Tuple<int, VFPTabanco>> GetUpdatedObjects(List<VFPTabanco> vfpList, List<APITabanco> apiList)
        {
            return (from vfp in vfpList from api in apiList.Where(api => api.codbanco.Equals(vfp.codbanco)) where !vfp.codempre.Equals(api.codempre) || !vfp.codbanco.Equals(api.codbanco) || !vfp.nombanco.Equals(api.nombanco) || !vfp.codctacc.Equals(api.codctacc) || !vfp.ctacc.Equals(api.ctacc) || !vfp.ctacontab.Equals(api.ctacontab) || vfp.chequeact != api.chequeact || vfp.chequefin != api.chequefin || vfp.ingreact != api.ingreact || vfp.ingrefin != api.ingrefin || vfp.egreact != api.egreact || vfp.egrefin != api.egrefin || vfp.trasact != api.trasact || vfp.trasfin != api.trasfin || vfp.compact != api.compact || vfp.compfin != api.compfin || vfp.ventact != api.ventact || vfp.ventfin != api.ventfin || vfp.uniact != api.uniact || vfp.estado != api.estado || vfp.ano != api.ano || vfp.flg_ing != api.flg_ing select new Tuple<int, VFPTabanco>(api.id, vfp)).ToList();
        }
        #endregion
        #region MaeCue
        public static List<VFPMaeCue> GetNewObjects(List<VFPMaeCue> vfpList, List<APIMaeCue> apiList)
        {
            throw new NotImplementedException();
        }
        public static List<Tuple<int, VFPMaeCue>> GetUpdatedObjects(List<VFPMaeCue> vfpList, List<APIMaeCue> apiList)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Tabaux10
        public static List<VFPTabaux10> GetNewObjects(List<VFPTabaux10> vfpList, List<APITabaux10> apiList)
        {
            throw new NotImplementedException();
        }
        public static List<Tuple<int, VFPTabaux10>> GetUpdatedObjects(List<VFPTabaux10> vfpList, List<APITabaux10> apiList)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
