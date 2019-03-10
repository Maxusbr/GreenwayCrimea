
namespace AdvantShop.Module.RussianPostPrintBlank.Models
{
    public class FormItem
    {
        public string Name { get; set; }

        public string Type { get; set; }
    }

    public class Template
    {
        public int TemplateID { get; set; }

        public string Name { get; set; }

        public FormType Type { get; set; }

        public string Content { get; set; }

        public string TypeName
        {
            get
            {
                switch(Type)
                {
                    case FormType.F7: return "Адресный ярлык Ф.7";
                    case FormType.F107: return "Опись вложения Ф.107";
                    case FormType.F112: return "Наложенный платеж Ф.112";
                    case FormType.None: return string.Empty;
                    default: return string.Empty;
                }
            }

            set
            {
                TypeName = value;
            }
        }
    }
}