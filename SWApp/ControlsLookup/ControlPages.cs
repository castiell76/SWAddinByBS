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
            foreach (
                Type? type in SWAppAssembly
                    .Asssembly.GetTypes()
                    .Where(t => t.IsDefined(typeof(SWAppPageAttribute)))
            )
            {
                SWAppPageAttribute? galleryPageAttribute = type.GetCustomAttributes<SWAppPageAttribute>()
                    .FirstOrDefault();

                if (galleryPageAttribute is not null)
                {
                    yield return new SWAppPage(
                        type.Name[..type.Name.LastIndexOf(PageSuffix)],
                        galleryPageAttribute.Description,
                        galleryPageAttribute.Icon,
                        type
                    );
                }
            }
        }

        public static IEnumerable<SWAppPage> FromNamespace(string namespaceName)
        {
            return All().Where(t => t.PageType?.Namespace?.StartsWith(namespaceName) ?? false);
        }
    }
}
