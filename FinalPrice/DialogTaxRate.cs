
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace FinalPrice
{
    public class OnTaxRateChanged : EventArgs {
		private string mTaxRate;
		private string mDescription;
		public string TaxRate {
			get { return mTaxRate; }
			set { mTaxRate = value; }
		}
		public string Description {
			get { return mDescription; }
			set { mDescription = value; }
		}
		public OnTaxRateChanged (string taxRate, string description) : base()
		{
			TaxRate = taxRate;
			Description = description;
		}
	}
	public class DialogTaxRate : DialogFragment
	{
		EditText TaxRateEditText;
		EditText DescriptionEditText;
		Button ApplyTaxRateButton;

		public event EventHandler<OnTaxRateChanged> mOnTaxRateChanged;

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle); //get rid of blue bar on top
			base.OnActivityCreated (savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogAnim;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);

			var view = inflater.Inflate (Resource.Layout.TaxRate, container, false);

			TaxRateEditText = view.FindViewById<EditText> (Resource.Id.txtTaxRate);
			DescriptionEditText = view.FindViewById<EditText> (Resource.Id.txtDescription);
			ApplyTaxRateButton = view.FindViewById<Button> (Resource.Id.btnApplyTaxRate);

			ISharedPreferences preferences = Application.Context.GetSharedPreferences("TaxInfo", FileCreationMode.Private);
			TaxRateEditText.Text = preferences.GetString ("Rate", String.Empty);
			DescriptionEditText.Text = preferences.GetString ("Description", String.Empty);

			ApplyTaxRateButton.Click += ApplyTaxRateButton_Click;

			return view;
		}

		void ApplyTaxRateButton_Click (object sender, EventArgs e)
		{
			if (TaxRateEditText.Text == string.Empty) {
				Android.Widget.Toast.MakeText (this.Activity, "Enter Tax Rate!", ToastLength.Long).Show();
				return;
			}
			double rate = Convert.ToDouble (TaxRateEditText.Text);
			mOnTaxRateChanged.Invoke (this, new OnTaxRateChanged(rate.ToString("N2"), DescriptionEditText.Text));
            Dismiss();
		}
			
	}
}

