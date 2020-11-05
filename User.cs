using System.Collections.Generic;

namespace AzureGraphApiTry
{
    public class User
    {
        public List<object> BusinessPhones { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string JobTitle { get; set; }
        public object Mail { get; set; }
        public object MobilePhone { get; set; }
        public object OfficeLocation { get; set; }
        public object PreferredLanguage { get; set; }
        public string Surname { get; set; }
        public string UserPrincipalName { get; set; }
        public string Id { get; set; }
    }
}
