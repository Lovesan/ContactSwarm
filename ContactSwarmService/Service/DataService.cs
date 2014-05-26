using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using ContactSwarmService.Provider;
using NLog;

namespace ContactSwarmService.Service
{
    [ServiceContract]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
                     InstanceContextMode = InstanceContextMode.Single)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ContactsProvider _provider;

        public DataService()
        {
            _provider = ContactsProvider.Instance;
            Logger.Trace("WCF Service created");
        }

        [OperationContract]
        [WebGet(UriTemplate = "Persons/{personId}",
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json)]
        public PersonData GetPerson(string personId)
        {
            Logger.Trace("GET /Persons/{0}", personId);
            try
            {
                return _provider.GetPerson(int.Parse(personId));
            }
            catch (Exception e)
            {
                Logger.ErrorException("GET /Persons/" + personId, e);
                throw;
            }
        }

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json,
                UriTemplate = "Persons",
                BodyStyle = WebMessageBodyStyle.Bare)]
        public IEnumerable<PersonData> GetAllPersons()
        {
            Logger.Trace("GET /Persons");
            try
            {
                return _provider.GetAllPersons();
            }
            catch (Exception e)
            {
                Logger.ErrorException("GET /Persons", e);
                throw;
            }
        }

        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare,
                   UriTemplate = "Persons")]
        public PersonData UpdatePerson(PersonData data)
        {
            Logger.Trace("POST /Persons");
            try
            {
                if (data.Name == null)
                {
                    Logger.Error("POST /Persons - person name must be present");
                    return null;
                }
                if(data.Id == 0)
                    Logger.Trace("Adding person {0}", data.Name);
                else
                    Logger.Trace("Modifying person with id {0}. New name: {1}", data.Id, data.Name);
                return _provider.UpdatePerson(data.Id, data.Name);
            }
            catch (Exception e)
            {
                Logger.ErrorException("POST /Persons", e);
                throw;
            }
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE",
                   BodyStyle = WebMessageBodyStyle.Bare,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "Persons/{personId}")]
        public void DeletePerson(string personId)
        {
            Logger.Trace("DELETE /Persons/{0}", personId);
            try
            {
                _provider.DeletePerson(int.Parse(personId));
            }
            catch (Exception e)
            {
                Logger.ErrorException("DELETE /Persons/" + personId, e);
                throw;
            }
        }

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json,
                UriTemplate = "ContactTypes")]
        public IEnumerable<string> GetContactTypes()
        {
            Logger.Trace("GET /ContactTypes");
            try
            {
                return _provider.GetContactTypes();
            }
            catch (Exception e)
            {
                Logger.ErrorException("GET /ContactTypes", e);
                throw;
            }
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE",
                   BodyStyle = WebMessageBodyStyle.Bare,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "ContactTypes/{type}")]
        public void DeleteContactType(string type)
        {
            Logger.Trace("DELETE /ContactTypes/{0}", type);
            try
            {
                _provider.DeleteContactType(type);
            }
            catch (Exception e)
            {
                Logger.ErrorException("DELETE /ContactTypes/" + type, e);
                throw;
            }
        }

        [OperationContract]
        [WebGet(UriTemplate = "Contacts/{personId}",
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<ContactData> GetAllContacts(string personId)
        {
            Logger.Trace("GET /Contacts/{0}", personId);
            try
            {
                return _provider.GetAllContacts(int.Parse(personId));
            }
            catch (Exception e)
            {
                Logger.ErrorException("GET /Contacts/" + personId, e);
                throw;
            }
        }

        [OperationContract]
        [WebGet(UriTemplate = "Contacts/{personId}/{type}",
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json)]
        public ContactData GetContact(string personId, string type)
        {
            Logger.Trace("GET /Contacts/{0}/{1}", personId, type);
            try
            {
                return _provider.GetContact(int.Parse(personId), type);
            }
            catch (Exception e)
            {
                Logger.ErrorException("GET /Contacts/" + personId + "/" + type, e);
                throw;
            }
        }

        [OperationContract]
        [WebInvoke(Method = "POST",
                   BodyStyle = WebMessageBodyStyle.Bare,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "Contacts/{personId}/{type}")]
        public ContactData UpdateContact(string personId, string type, ContactData data)
        {
            Logger.Trace("POST /Contacts/{0}/{1}", personId, type);
            try
            {
                if (data.Value == null)
                {
                    Logger.Error("POST /Contacts/{0}/{1} - 'value' must be present", personId, type);
                    return null;
                }
                return _provider.UpdateContact(int.Parse(personId), type, data.Value);
            }
            catch (Exception e)
            {
                Logger.ErrorException("POST /Contacts/" + personId + "/" + type, e);
                throw;
            }
        }

        [OperationContract]
        [WebInvoke(Method = "DELETE",
                   BodyStyle = WebMessageBodyStyle.Bare,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/Contacts/{personId}/{type}")]
        public void DeleteContact(string personId, string type)
        {
            Logger.Trace("DELETE /Contacts/{0}/{1}", personId, type);
            try
            {
                _provider.DeleteContact(int.Parse(personId), type);
            }
            catch (Exception e)
            {
                Logger.ErrorException("DELETE /Contacts/" + personId + "/" + type, e);
                throw;
            }
        }
    }
}
