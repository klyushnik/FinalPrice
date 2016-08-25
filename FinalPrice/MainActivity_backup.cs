using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Xml;
using Android.Views.InputMethods;
using ZXing.Mobile;

namespace FinalPrice
{
	[Activity (Label = "FinalPrice", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity_backup : Activity
	{
		EditText StickerPriceEditText;
		EditText DiscountEditText;

		Button CalculatePriceButton;
		Button TaxRateButton;
		Button ScanButton;

		RadioButton DollarRadioButton;
		RadioButton PercentRadioButton;
		LinearLayout MainLayout;
		ISharedPreferences preferences = Application.Context.GetSharedPreferences("TaxInfo", FileCreationMode.Private);

		private string taxRate;
		private string taxDescription;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main2);

			//set up controls - edittext
			StickerPriceEditText = FindViewById<EditText> (Resource.Id.stickerPriceText);
			DiscountEditText = FindViewById<EditText> (Resource.Id.discountText);

			//buttons
			CalculatePriceButton = FindViewById<Button> (Resource.Id.calculatePriceButton);
			TaxRateButton = FindViewById<Button> (Resource.Id.btnTaxRate);
			ScanButton = FindViewById<Button> (Resource.Id.btnScan);

			//radiobuttons
			DollarRadioButton = FindViewById<RadioButton> (Resource.Id.dollarRadioButton);
			PercentRadioButton = FindViewById<RadioButton> (Resource.Id.percentRadioButton);
			MainLayout = FindViewById<LinearLayout> (Resource.Id.linearLayout1);

			//bind functions to buttons
			CalculatePriceButton.Click += (object sender, EventArgs e) => Calculate();
			MainLayout.Click += MainLayout_Click;
			ScanButton.Click += ScanButton_Click;

			TaxRateButton.Click += (object sender, EventArgs e) => {
				//show tax rate dialog
				FragmentTransaction transaction = FragmentManager.BeginTransaction ();
				DialogTaxRate taxRateDialog = new DialogTaxRate ();
				taxRateDialog.Show (transaction, "tax rate");
				taxRateDialog.mOnTaxRateChanged += TaxRateDialog_mOnTaxRateChanged;
			};
				
			//init tax rate for the first-time run
			ResetTaxRateDesc();
			if (taxRate == String.Empty) {
				ISharedPreferencesEditor prefEditor = preferences.Edit ();
				prefEditor.PutString ("Rate", "8.00");
				prefEditor.PutString ("Description", "Sacramento, CA"); //cuz fuk u
				prefEditor.Apply ();
				taxRate = preferences.GetString ("Rate", "ERROR");
			}

			TaxRateButton.Text = "Tax Rate: " + taxRate + "% (Click to Change)";
		}

		async void ScanButton_Click (object sender, EventArgs e)
		{
			//let's see if this works first
			MobileBarcodeScanner.Initialize (Application);
			var scanner = new ZXing.Mobile.MobileBarcodeScanner ();
			var scanResult = await scanner.Scan ();
			if (scanResult != null) {
				string walmartComQuery = "http://api.walmartlabs.com/v1/items?apiKey=6rukyx4ykumu8xncm5b4f2xc&format=xml&upc=" + scanResult.Text;
				XmlTextReader reader = new XmlTextReader (walmartComQuery);
				try
				{ 
					reader.ReadToFollowing ("salePrice");
					string walmartComResult = reader.ReadElementContentAsString ();
					Toast.MakeText (this, "Walmart.Com price: $" + walmartComResult, ToastLength.Long).Show ();
					StickerPriceEditText.Text = walmartComResult;
				}
				catch {
					Toast.MakeText (this, "UPC not found!", ToastLength.Long).Show ();
				}
				finally {
					reader.Close ();
				}
			}
		}

		void TaxRateDialog_mOnTaxRateChanged (object sender, OnTaxRateChanged e)
		{
			ISharedPreferencesEditor edit = preferences.Edit ();
			edit.PutString ("Rate", e.TaxRate);
			edit.PutString ("Description", e.Description);
			edit.Apply ();
			ResetTaxRateDesc ();
			TaxRateButton.Text = "Tax Rate: " + taxRate + "% (Click to Change)";
		}

		private void ResetTaxRateDesc() {
			taxRate = preferences.GetString ("Rate", String.Empty);
			taxDescription = preferences.GetString ("Description", String.Empty);
		}
		
		private void Calculate()
		{
			double stickerPrice;
			double discount;
			double finalPrice;
			double subtotal;
			double tax = Convert.ToDouble(taxRate) / 100;
			if (StickerPriceEditText.Text == String.Empty) {
				Android.Widget.Toast.MakeText (this, "Sticker Price field cannot be empty!", ToastLength.Long).Show();
				return;
			}
			try {
				stickerPrice = (Convert.ToDouble (StickerPriceEditText.Text.ToString()));
			}
			catch {
				stickerPrice = 0;
			}
			try {
				discount = Convert.ToDouble (DiscountEditText.Text.ToString ());
			}
			catch {
				discount = 0;
			}
			//dollar discount or percent discount
			if (PercentRadioButton.Checked) {
				subtotal = stickerPrice * (1 - (discount / 100));
			} else {
				subtotal = stickerPrice - discount;
			}
			finalPrice = subtotal + (subtotal * tax);
			FragmentTransaction transaction = FragmentManager.BeginTransaction ();
			Summary summaryDialog = new Summary ();
			summaryDialog.PassSummaryValues (subtotal.ToString ("C"), (subtotal * tax).ToString ("C"), finalPrice.ToString ("C"));
			summaryDialog.Show (transaction, "overview");

		}

		//hide keyboard when clicked outside of text field
		void MainLayout_Click(object sender, EventArgs e) {
			InputMethodManager inputManager = (InputMethodManager)this.GetSystemService (Activity.InputMethodService);
			inputManager.HideSoftInputFromWindow (this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
		}

	}
}


