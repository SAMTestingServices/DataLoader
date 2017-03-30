using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lloyds.LAF.Agent.Restricted.Extensions
{
    public static class StringExtensions
    {
        public static string GetSanitisedApplicationUrl(this string applicationUrl)
        {
            if (string.IsNullOrEmpty(applicationUrl))
            {
                return string.Empty;
            }

            applicationUrl = applicationUrl.TrimEnd('/');
            return applicationUrl;
        }
    }
}
