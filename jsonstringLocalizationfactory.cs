using Microsoft.Extensions.Localization;
using System;

namespace Json_Based_Localization.Web
{
    public class jsonstringLocalizationfactory : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer();
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer();
        }
    }
}
