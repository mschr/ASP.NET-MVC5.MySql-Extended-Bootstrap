using System.ComponentModel.DataAnnotations;

namespace my.ns.entities.dto.administration
{
    using RTYP = Resources.Views.Admin.Resources;

    public class SettingsModel
    {
        //[Key]
        //public int id { get; set; }
        [Display(Name = "PropertyKey", ResourceType = typeof(RTYP))]
        public string key { get; set; }
        [Display(Name = "PropertyValue", ResourceType = typeof(RTYP))]
        public string value { get; set; }
    }
}