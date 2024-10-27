
namespace ProjectPfa.View
{
    public partial class ConfirmationPage : ContentPage
    {
        private string _verificationCode;
        private string _mobileNumber;

        public ConfirmationPage(string verificationCode, string mobileNumber)
        {
            InitializeComponent();
            _verificationCode = verificationCode;
            _mobileNumber = mobileNumber;

            // Mettre à jour l'étiquette avec le numéro de téléphone
            VerificationMessage.Text = $"Please type the verification code sent to {FormatPhoneNumber(mobileNumber)}";
        }

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            if (VerificationCodeEntry.Text == _verificationCode)
            {
                DisplayAlert("Success", "Verification successful!", "OK");
                // Navigate to the next page or main page
            }
            else
            {
                DisplayAlert("Error", "Invalid verification code. Please try again.", "OK");
            }
        }

        private string FormatPhoneNumber(string phoneNumber)
        {
            return $"+{phoneNumber.Substring(0, 3)} {phoneNumber.Substring(3, 4)}****{phoneNumber.Substring(phoneNumber.Length - 2)}";
        }
    }
}
