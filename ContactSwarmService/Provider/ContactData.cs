using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ContactSwarmService.Data;

namespace ContactSwarmService.Provider
{
    [DataContract]
    public class ContactData
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        public static ContactData Convert(Contact c)
        {
            return c == null
                       ? null
                       : new ContactData
                           {
                               Type = c.Type.Name,
                               Value = c.Value
                           };
        }

        public static ICollection<ContactData> Convert(IEnumerable<Contact> cs)
        {
            return cs.Select(Convert).ToList();
        }
    }
}
