using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ContactSwarmService.Data;

namespace ContactSwarmService.Provider
{
    [DataContract]
    public class PersonData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        public static PersonData Convert(Person p)
        {
            return p == null
                       ? null
                       : new PersonData
                           {
                               Id = (int)p.Id,
                               Name = p.Name
                           };
        }

        public static ICollection<PersonData> Convert(IEnumerable<Person> ps)
        {
            return ps.Select(Convert).ToList();
        }
    }
}
