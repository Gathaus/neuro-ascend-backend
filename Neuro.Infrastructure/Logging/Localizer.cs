using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Neuro.Infrastructure.Logging
{
    public class Localizer
    {
        private static Localizer instance = null!;

        public static Localizer Instance
        {
            get
            {
                if(instance == null)
                    instance = new Localizer();

                return instance;
            }
        }

        private readonly ResourceManager _resourceManager;

        public Localizer()
        {
            _resourceManager = new ResourceManager("Neuro.Infrastructure.Logging.Resources.Resource", Assembly.GetAssembly(this.GetType())!);
        }

        public string? Translate(string key)
        {
            var message = _resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true)!.GetString(key);

            return message;
        }

        public string GetLocalizedEnumTexts(Enum value)
        {
            var stringBuilder = new StringBuilder();

            foreach (Enum item in Enum.GetValues(value.GetType()))
            {
                if (value.HasFlag(item) && !item.ToString().Equals("None"))
                    stringBuilder.Append(Translate(item.ToString()) + ",");
            }

            return stringBuilder.ToString().TrimEnd(',');
        }
    }
}
