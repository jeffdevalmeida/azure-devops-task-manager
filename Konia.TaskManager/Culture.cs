using System.Globalization;
using System.Threading;

namespace Konia.TaskManager
{
    internal class Culture
    {
        internal static void SetCulture()
        {
            CultureInfo ci = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
