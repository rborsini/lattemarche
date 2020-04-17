using LatteMarche.Application.Auth.Dtos;
using LatteMarche.WebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace LatteMarche.WebApi.Helpers
{
    public static class ReflectionHelper
    {

        public static List<AzioneDto> GetAzioni()
        {
            List<AzioneDto> azioni = new List<AzioneDto>();

            foreach (Type controllerType in GetControllers())
            {
                bool isMvcController = HasCustomAuthorizeAttribute<MvcCustomAuthorize>(controllerType);
                bool isApiController = HasCustomAuthorizeAttribute<ApiCustomAuthorize>(controllerType);

                if (isMvcController || isApiController)
                {
                    string type = isMvcController ? "MVC" : "API";
                    string controllerName = controllerType.Name.Replace("Controller", String.Empty);
                    foreach (MethodInfo methodInfo in GetAnnotatedMethods<ViewItem>(controllerType))
                    {
                        string actionName = methodInfo.Name;
                        foreach (ViewItem item in GetViewItems(methodInfo))
                        {
                            azioni.Add(new AzioneDto()
                            {
                                Action = actionName,
                                Controller = controllerName,
                                Id = String.Format("{0}-{1}-{2}-{3}", type, controllerName, actionName, item.Id),
                                Nome = item.DisplayName,
                                Pagina = item.Page,
                                Type = type,
                                ViewItem = item.Id
                            });
                        }
                    }
                }
            }

            return azioni;
        }

        private static bool HasCustomAuthorizeAttribute<T>(Type type)
            where T : System.Attribute
        {
            return type.GetCustomAttribute<T>() != null;
        }

        private static List<MethodInfo> GetAnnotatedMethods<T>(Type type)
            where T : System.Attribute
        {
            return type.GetMembers()
                .Where(m => m is MethodInfo)
                .Where(p => p.GetCustomAttributes<T>(false) != null)
                .Select(m => m as MethodInfo)
                .ToList();
        }

        private static List<ViewItem> GetViewItems(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes<ViewItem>()
                .Where(vi => !String.IsNullOrEmpty(vi.Page))    // in questa maniera filtro tutti gli item delle classi base EntityApiController
                .ToList();
        }

        private static List<Type> GetControllers()
        {
            List<Type> controllerTypes = new List<Type>();
            GetSubClasses<Controller>().ForEach(type => controllerTypes.Add(type));
            GetSubClasses<ApiController>().ForEach(type => controllerTypes.Add(type));
            return controllerTypes;
        }

        /// <summary>
        /// Returns the list of classes that inherit from the type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static List<Type> GetSubClasses<T>()
        {

            List<Type> result = new List<Type>();
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in asms)
            {
                try
                {
                    result.AddRange(assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(T))).ToList());
                }
                catch
                {
                    // GetTypes not implemented
                }
            }

            return result;
        }

    }
}