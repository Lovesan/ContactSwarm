using System;
using System.Collections.Generic;
using System.Linq;
using ContactSwarmService.Data;

namespace ContactSwarmService.Provider
{
    public class ContactsProvider : IDisposable
    {
        private static readonly Object Lock = new object();

        private static ContactsProvider _singleton;

        public static ContactsProvider Instance
        {
            get
            {
                lock (Lock)
                {
                    return _singleton ?? (_singleton = new ContactsProvider());
                }
            }
        }

        private readonly ContactSwarmEntities _container;

        private ContactsProvider()
        {
            _container = new ContactSwarmEntities();
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public ICollection<PersonData> GetAllPersons()
        {
            return PersonData.Convert(_container.Persons);
        }

        public PersonData GetPerson(int id)
        {
            return PersonData.Convert(_container.Persons.FirstOrDefault(p => p.Id == id));
        }

        public PersonData UpdatePerson(int id, string name)
        {
            var p = _container.Persons.FirstOrDefault(x => x.Id == id);
            if (p == null)
            {
                p = _container.Persons.Create();
                _container.Persons.Add(p);
            }
            p.Name = name;
            _container.SaveChanges();
            return PersonData.Convert(p);
        }

        public void DeletePerson(int id)
        {
            var p = _container.Persons.FirstOrDefault(x => x.Id == id);
            if (p == null) return;
            _container.Persons.Remove(p);
            _container.SaveChanges();
        }

        public ICollection<string> GetContactTypes()
        {
            return _container.ContactTypes.Select(x => x.Name).ToList();
        }

        public void DeleteContactType(string type)
        {
            var ct =
                _container.ContactTypes.FirstOrDefault(
                    x => x.Name.Equals(type, StringComparison.InvariantCultureIgnoreCase));
            if (ct != null)
            {
                _container.ContactTypes.Remove(ct);
                _container.SaveChanges();
            }
        }

        public ICollection<ContactData> GetAllContacts(int personId)
        {
            return ContactData.Convert(_container.Contacts.Where(c => c.PersonId == personId).ToArray());
        }

        public ContactData GetContact(int personId, string type)
        {
            var ct =
                _container.ContactTypes.FirstOrDefault(
                    x => x.Name.Equals(type, StringComparison.InvariantCultureIgnoreCase));
            return ct == null
                       ? null
                       : ContactData.Convert(ct.Contacts.FirstOrDefault(c => c.PersonId == personId));
        }

        public ContactData UpdateContact(int personId, string type, string value)
        {
            if (type == null)
                return null;
            var p =
                _container.Persons.FirstOrDefault(x => x.Id == personId);
            if (p == null)
                return null;
            var ct =
                _container.ContactTypes.FirstOrDefault(
                    x => x.Name.Equals(type, StringComparison.InvariantCultureIgnoreCase));
            if (ct == null)
            {
                ct = _container.ContactTypes.Create();
                ct.Name = type;
                _container.ContactTypes.Add(ct);
                _container.SaveChanges();
            }
            var c =
                p.Contacts.FirstOrDefault(x => x.TypeId == ct.Id);
            if (c == null)
            {
                c = _container.Contacts.Create();
                c.Person = p;
                c.PersonId = p.Id;
                c.Type = ct;
                c.TypeId = ct.Id;
                _container.Contacts.Add(c);
            }
            c.Value = value;
            _container.SaveChanges();
            return ContactData.Convert(c);
        }

        public void DeleteContact(int personId, string type)
        {
            if (type == null)
                return;
            var p =
                _container.Persons.FirstOrDefault(x => x.Id == personId);
            if (p == null)
                return;
            var c =
                p.Contacts.FirstOrDefault(
                    x => x.Type.Name.Equals(type, StringComparison.InvariantCultureIgnoreCase));
            if (c == null) return;
            _container.Contacts.Remove(c);
            _container.SaveChanges();
        }
    }
}
