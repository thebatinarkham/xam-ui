using AppName.Core;
namespace AppName
{
    public class AddContactViewModel : ObservableObject
    {
        private string _phoneCode, _phone;

        public AddContactViewModel(FlowContactData contact)
        {
            IsEdit = contact != null;
            Contact = contact ?? new FlowContactData();

            if (IsEdit)
            {
                UpdatePhone();
            }
        }

        public bool IsEdit { get; }
 
        public string Title => IsEdit ? Resx.AppResources.StringEditContact : Resx.AppResources.StringNewContact;

        public string PhoneCode
        {
            get { return _phoneCode; }
            set { SetProperty(ref _phoneCode, value); }
        }

        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        public FlowContactData Contact { get; }

        private void UpdatePhone()
        {
            if (string.IsNullOrEmpty(Contact.Phone))
            {
                return;
            }

            var s = Contact.Phone.IndexOf('(');
            var e = Contact.Phone.IndexOf(')');
            if (s > 0 && e > s)
            {
                PhoneCode = Contact.Phone.Substring(s + 1, e - s).Trim();
                Phone = Contact.Phone.Substring(s + 1).Trim();
            }
        }
    }
}
