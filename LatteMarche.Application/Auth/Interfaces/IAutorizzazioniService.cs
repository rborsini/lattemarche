using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace LatteMarche.Application.Auth.Interfaces
{
    public interface IAutorizzazioniService
    {
        bool Authorize(HttpSessionState session, string username, string type, string controllerName, string actionName);

        bool Authorize(HttpSessionStateBase session, string username, string type, string controllerName, string actionName);

        bool Authorize(HttpSessionState session, string username, string type, string controllerName, string actionName, string viewItem);

        Dictionary<string, bool> GetViewBagTokens(HttpSessionState session, string username, string controllerName, string actionName);

        List<string> GetPermissions(string username);
    }
}
