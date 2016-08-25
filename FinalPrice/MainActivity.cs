using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using System.Collections.Generic;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Graphics;
using Android.Content.PM;

namespace FinalPrice
{
	[Activity (Label = "Final Price Pro", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/FinalPriceStyle", WindowSoftInputMode = SoftInput.AdjustNothing, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : AppCompatActivity
	{
        private Android.Support.V4.Widget.DrawerLayout drawerLayout;
        private LinearLayout drawerContainer;
        private Button ClearButton;
        private TextView AboutTextView;
        private TextView HelpTextView;
        private TextView SettingsTextView;
        private Button CalculateButton;
		private ImageButton OpenDrawerButton;

		private TextView AddTextView;
		private EditText ItemPriceEditText;
		private EditText DiscountEditText;
		private TextView TaxOptionTextView;
		private RadioButton PercentRadioButton;

        ISharedPreferences preferences;

		private List<TaxItem> taxItems;
		private ListView mListView;

		RowAdapter adapter;

		private int selectedTaxOption = 0;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main2);

            preferences = Application.Context.GetSharedPreferences("TaxInfo", FileCreationMode.Private);

            //drawer layout and textviews
            drawerLayout = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.DL1);
            drawerContainer = FindViewById<LinearLayout>(Resource.Id.myDrawerContainer);

            AboutTextView = FindViewById<TextView>(Resource.Id.aboutTextView);
            AboutTextView.Click += AboutTextView_Click;

            HelpTextView = FindViewById<TextView>(Resource.Id.helpTextView);
            HelpTextView.Click += HelpTextView_Click;

            SettingsTextView = FindViewById<TextView>(Resource.Id.configureTextView);
            SettingsTextView.Click += SettingsTextView_Click;

            //Main list view
            mListView = FindViewById<ListView> (Resource.Id.taxItemsListView);
			taxItems = new List<TaxItem>();
			adapter = new RowAdapter (this, taxItems);
			mListView.Adapter = adapter;

			//buttons

			AddTextView = FindViewById<TextView> (Resource.Id.addTextView);
			AddTextView.Click += AddTextView_Click;

			ItemPriceEditText = FindViewById<EditText> (Resource.Id.itemPriceEditText);
			DiscountEditText = FindViewById<EditText> (Resource.Id.discountEditText);
			TaxOptionTextView = FindViewById<TextView> (Resource.Id.taxOptionTextView);
			TaxOptionTextView.Click += TaxOptionTextView_Click;
			PercentRadioButton = FindViewById<RadioButton> (Resource.Id.percentRadioButton);

            CalculateButton = FindViewById<Button>(Resource.Id.calculateButton);
            CalculateButton.Click += CalculateButton_Click;

            ClearButton = FindViewById<Button>(Resource.Id.clearButton);
            ClearButton.Click += (object sender, EventArgs e) =>
            {
                taxItems.Clear();
                adapter.NotifyDataSetChanged();
            };




            OpenDrawerButton = FindViewById<ImageButton>(Resource.Id.toolbarMenuButton);
            OpenDrawerButton.Click += OpenDrawerButton_Click;

            //init tax rate for the first-time run
            bool firstRunOver = preferences.GetBoolean("isFirstRunComplete", false);

            if (firstRunOver == false)
            {
                Intent intent = new Intent(this, typeof(Welcome));
                StartActivity(intent);
            }
        }

		//main logic

		private void AboutTextView_Click(object sender, EventArgs e)
		{
			var intent = new Intent(this, typeof(About));
			StartActivity(intent);
			drawerLayout.CloseDrawer(drawerContainer);
		}

		void AddTextView_Click (object sender, EventArgs e)
		{
			double stickerPrice;
			double discount;
			InputMethodManager inputManager = (InputMethodManager)this.GetSystemService (Activity.InputMethodService);
			inputManager.HideSoftInputFromWindow (this.CurrentFocus.WindowToken, HideSoftInputFlags.None);

			TaxItem mTaxItem = new TaxItem();
			double.TryParse(ItemPriceEditText.Text, out stickerPrice);
			double.TryParse(DiscountEditText.Text, out discount);

			mTaxItem.StickerPrice = stickerPrice;
			mTaxItem.Discount = discount;
			mTaxItem.isPercent = PercentRadioButton.Checked;

			mTaxItem.TaxOption = selectedTaxOption;

			taxItems.Add (mTaxItem);
			adapter.NotifyDataSetChanged ();
			mListView.SmoothScrollToPosition (taxItems.Count - 1);

			//clean up
			ItemPriceEditText.Text = "";
			DiscountEditText.Text = "";
			PercentRadioButton.Checked = true;
			selectedTaxOption = 0;
			TaxOptionTextView.Text = "Standard Tax";
		}

		private void CalculateButton_Click(object sender, EventArgs e)
		{
			double subtotal = 0;
			double finalPrice = 0;
			double tax = 0;

			foreach (TaxItem x in taxItems) {

				double taxRate = Convert.ToDouble (preferences.GetFloat ("mainRate", 8.00f)) / 100;
				double additionalTax = 0;
				bool additionalTaxIsPercent = false;
				double itemSubtotal = 0;

				switch (x.TaxOption) {
				case 0:
					break;
				case 1:
					taxRate = 0;
					break;
				case 2:
					additionalTax = Convert.ToDouble (preferences.GetFloat ("tax1Rate", 5.00f));
					additionalTaxIsPercent = preferences.GetBoolean ("tax1IsPercent", false);
					if (preferences.GetBoolean ("tax1IsOnlyTax", false)) {
						taxRate = additionalTax / 100;
						additionalTax = 0;
					}
					break;
				case 3:
					additionalTax = Convert.ToDouble (preferences.GetFloat ("tax2Rate", 5.00f));
					additionalTaxIsPercent = preferences.GetBoolean("tax2IsPercent", false);
					if (preferences.GetBoolean ("tax2IsOnlyTax", false)) {
						taxRate = additionalTax / 100;
						additionalTax = 0;
					}
					break;
				case 4:
					additionalTax = Convert.ToDouble (preferences.GetFloat ("tax3Rate", 5.00f));
					additionalTaxIsPercent = preferences.GetBoolean("tax3IsPercent", false);
					if (preferences.GetBoolean ("tax3IsOnlyTax", false)) {
						taxRate = additionalTax / 100;
						additionalTax = 0;
					}
					break;
				}

				if (additionalTax == 0 & x.TaxOption != 0) {
					//discount first
					if (x.isPercent) {
						itemSubtotal = x.StickerPrice * (1 - (x.Discount / 100));
					} else {
						itemSubtotal = x.StickerPrice - x.Discount;
					}
					//increment function-level vars
					subtotal += itemSubtotal;
					tax += (additionalTaxIsPercent) ? (itemSubtotal * taxRate) : (taxRate * 100);
				} else {
					//discount
					if (x.isPercent) {
						itemSubtotal = x.StickerPrice * (1 - (x.Discount / 100));
					} else {
						itemSubtotal = x.StickerPrice - x.Discount;
					}
					//add additional tax
					if (additionalTaxIsPercent) {
						itemSubtotal += itemSubtotal * additionalTax / 100;
					} else {
						itemSubtotal += additionalTax;
					}
					//function-level vars
					subtotal += itemSubtotal;
					tax += itemSubtotal * taxRate;
				}
			}
			finalPrice = subtotal + tax;
			FragmentTransaction transaction = FragmentManager.BeginTransaction ();
			Summary summaryDialog = new Summary ();
			summaryDialog.PassSummaryValues (subtotal.ToString ("C"), tax.ToString ("C"), finalPrice.ToString ("C"));
			summaryDialog.Show (transaction, "overview");
		}

		private void HelpTextView_Click(object sender, EventArgs e)
		{
			var intent = new Intent(this, typeof(Help));
			StartActivity(intent);
			drawerLayout.CloseDrawer(drawerContainer);
		}

		private void OpenDrawerButton_Click(object sender, EventArgs e)
		{
			//start welcome screen in dev version
			//var intent = new Intent(this, typeof(Welcome));
			//StartActivity(intent);
			drawerLayout.OpenDrawer(drawerContainer);
		}

		void SelectTaxFragment_mOnSelectTax (object sender, OnSelectTax e)
		{
			selectedTaxOption = e.TaxOption;
			switch (selectedTaxOption) {
			case 0:
				TaxOptionTextView.Text = "Standard Tax";
				break;
			case 1:
				TaxOptionTextView.Text = "Tax Exempt";
				break;
			case 2:
				TaxOptionTextView.Text = preferences.GetString ("tax1Desc", "ERROR");
				break;
			case 3:
				TaxOptionTextView.Text = preferences.GetString ("tax2Desc", "ERROR");
				break;
			case 4:
				TaxOptionTextView.Text = preferences.GetString ("tax3Desc", "ERROR");
				break;
			}

		}

        private void SettingsTextView_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Settings));
            StartActivity(intent);
            drawerLayout.CloseDrawer(drawerContainer);
        }

		void TaxOptionTextView_Click (object sender, EventArgs e)
		{
			FragmentTransaction transaction = FragmentManager.BeginTransaction ();
			SelectTaxFragment taxRateDialog = new SelectTaxFragment ();
			taxRateDialog.Show (transaction, "tax rate");
			SelectTaxFragment.mOnSelectTax += SelectTaxFragment_mOnSelectTax;
		}

	}
}


