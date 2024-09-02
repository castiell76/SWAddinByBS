using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SWApp.ControlsLookup
{
    internal static class ControlPages
    {
        private const string PageSuffix = "Page";

        public static IEnumerable<SWAppPage> All()
        {
            var pages = new List<SWAppPage>();
            Type[] types;

            // Próbujemy pobrać wszystkie typy z assembly
            try
            {
                types = SWAppAssembly.Assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Jeśli wystąpi błąd, używamy tylko załadowanych typów
                types = ex.Types.Where(t => t != null).ToArray();

                // Logujemy każdy wyjątek, jeśli potrzebne
                foreach (var loaderEx in ex.LoaderExceptions)
                {
                    Console.WriteLine($"Error loading type: {loaderEx.Message}");
                }
            }

            // Przechodzimy przez załadowane typy i przetwarzamy te, które mają wymagany atrybut
            foreach (Type? type in types.Where(t => t.IsDefined(typeof(SWAppPageAttribute))))
            {
                try
                {
                    SWAppPageAttribute? galleryPageAttribute = type.GetCustomAttributes<SWAppPageAttribute>()
                        .FirstOrDefault();

                    if (galleryPageAttribute is not null)
                    {
                        Console.WriteLine($"Found type: {type.FullName}");
                        pages.Add(new SWAppPage(
                            type.Name[..type.Name.LastIndexOf(PageSuffix)],
                            galleryPageAttribute.Description,
                            galleryPageAttribute.Icon,
                            type
                        ));
                    }
                }
                catch (Exception ex)
                {
                    // Opcjonalnie: Logowanie błędu podczas przetwarzania konkretnego typu
                    Console.WriteLine($"Error processing type {type?.FullName}: {ex.Message}");
                }
            }

            // Zwrócenie wyników za pomocą yield return
            foreach (var page in pages)
            {
                yield return page;
            }
        }



        public static IEnumerable<SWAppPage> FromNamespace(string namespaceName)
        {
            return All().Where(t => t.PageType?.Namespace?.StartsWith(namespaceName) ?? false);
        }
    }
}
