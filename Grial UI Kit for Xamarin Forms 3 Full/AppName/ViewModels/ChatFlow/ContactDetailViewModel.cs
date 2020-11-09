using System.Collections.Generic;
using System.Linq;
using AppName.Core;

namespace AppName
{
    public class ContactDetailViewModel : ObservableObject
    {
        private readonly string _contactId;
        private FlowContactData _contact;

        public ContactDetailViewModel(string contactId)
        {
            _contactId = contactId;

            LoadData();
        }

        public FlowContactData Contact
        {
            get { return _contact; }
            set 
            {
                if (SetProperty(ref _contact, value))
                {
                    NotifyPropertyChanged(nameof(Values));
                }
            }
        }

        public IEnumerable<ValueData> Values
        {
            get
            {
                if (Contact == null)
                {
                    yield break;
                }

                yield return new ValueData("Name", Contact.Name);
                yield return new ValueData("Address 1", Contact.Address1);
                yield return new ValueData("Address 2", Contact.Address2);
                yield return new ValueData("Phone", Contact.Phone);
                yield return new ValueData("Email", Contact.Email);
                yield return new ValueData("Organization", Contact.Organization);
                yield return new ValueData("Country", Contact.Country);
                yield return new ValueData("City", Contact.City);
                yield return new ValueData("Zip", Contact.Zip);
                yield return new ValueData("Notes", Contact.Notes);
            }
        }

        private void LoadData()
        {
            Contact = null;

            JsonHelper.Instance.LoadViewModel(this, source: "ChatFlow.json");

            if (_contactId != null)
            {
                Contact = new ChatMainViewModel()
                    .Contacts.FirstOrDefault(c => c.Id == _contactId);
            }
        }

        public class ValueData
        {
            public ValueData(string label, string value)
            {
                Label = label;
                Value = value;
            }

            public string Label { get; }
            public string Value { get; }
        }
    }
}
