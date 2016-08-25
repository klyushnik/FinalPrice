using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

namespace FinalPrice
{
	[Activity(Label = "Settings", ScreenOrientation = ScreenOrientation.Portrait)]

    public class Settings : Activity
    {
        ISharedPreferences preferences;
        private TextView saveSettingsTextView;

        private EditText mainTaxRateEditText;

        private EditText tax1DescEditText;
        private EditText tax1RateEditText;
        private RadioButton tax1PercentRadioButton;
        private RadioButton tax1DollarsRadioButton;
        private CheckBox tax1CountedCheckbox;

        private EditText tax2DescEditText;
        private EditText tax2RateEditText;
        private RadioButton tax2PercentRadioButton;
        private RadioButton tax2DollarsRadioButton;
        private CheckBox tax2CountedCheckbox;

        private EditText tax3DescEditText;
        private EditText tax3RateEditText;
        private RadioButton tax3PercentRadioButton;
        private RadioButton tax3DollarsRadioButton;
        private CheckBox tax3CountedCheckbox;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.settings);
            preferences = Application.Context.GetSharedPreferences("TaxInfo", FileCreationMode.Private);

            saveSettingsTextView = FindViewById<TextView>(Resource.Id.saveSettingsTextView);
            saveSettingsTextView.Click += SaveSettingsTextView_Click;

            mainTaxRateEditText = FindViewById<EditText>(Resource.Id.mainTaxRateEditText);

            tax1DescEditText = FindViewById<EditText>(Resource.Id.tax1DescriptionEditText);
            tax1RateEditText = FindViewById<EditText>(Resource.Id.tax1ValueEditText);
            tax1PercentRadioButton = FindViewById<RadioButton>(Resource.Id.tax1PercentRadioButton);
            tax1DollarsRadioButton = FindViewById<RadioButton>(Resource.Id.tax1DollarsRadioButton);
            tax1CountedCheckbox = FindViewById<CheckBox>(Resource.Id.tax1OnlyTaxCheckbox);

            tax2DescEditText = FindViewById<EditText>(Resource.Id.tax2DescriptionEditText);
            tax2RateEditText = FindViewById<EditText>(Resource.Id.tax2ValueEditText);
            tax2PercentRadioButton = FindViewById<RadioButton>(Resource.Id.tax2PercentRadioButton);
            tax2DollarsRadioButton = FindViewById<RadioButton>(Resource.Id.tax2DollarsRadioButton);
            tax2CountedCheckbox = FindViewById<CheckBox>(Resource.Id.tax2OnlyTaxCheckbox);

            tax3DescEditText = FindViewById<EditText>(Resource.Id.tax3DescriptionEditText);
            tax3RateEditText = FindViewById<EditText>(Resource.Id.tax3ValueEditText);
            tax3PercentRadioButton = FindViewById<RadioButton>(Resource.Id.tax3PercentRadioButton);
            tax3DollarsRadioButton = FindViewById<RadioButton>(Resource.Id.tax3DollarsRadioButton);
            tax3CountedCheckbox = FindViewById<CheckBox>(Resource.Id.tax3OnlyTaxCheckbox);

            mainTaxRateEditText.Text = preferences.GetFloat("mainRate", 8.00f).ToString();

            tax1DescEditText.Text = preferences.GetString("tax1Desc","Recycle");
            tax1RateEditText.Text = preferences.GetFloat("tax1Rate", 5.00f).ToString();
            tax1PercentRadioButton.Checked = preferences.GetBoolean("tax1IsPercent", false);
            tax1DollarsRadioButton.Checked = !preferences.GetBoolean("tax1IsPercent", false);
            tax1CountedCheckbox.Checked = preferences.GetBoolean("tax1IsOnlyTax", false);

            tax2DescEditText.Text = preferences.GetString("tax2Desc", "CRV");
            tax2RateEditText.Text = preferences.GetFloat("tax2Rate", 0.05f).ToString();
            tax2PercentRadioButton.Checked = preferences.GetBoolean("tax2IsPercent", false);
            tax2DollarsRadioButton.Checked = !preferences.GetBoolean("tax2IsPercent", false);
            tax2CountedCheckbox.Checked = preferences.GetBoolean("tax2IsOnlyTax", true);

            tax3DescEditText.Text = preferences.GetString("tax3Desc", "Tobacco");
            tax3RateEditText.Text = preferences.GetFloat("tax3Rate", 0.87f).ToString();
            tax3PercentRadioButton.Checked = preferences.GetBoolean("tax3IsPercent", false);
            tax3DollarsRadioButton.Checked = !preferences.GetBoolean("tax3IsPercent", false);
            tax3CountedCheckbox.Checked = preferences.GetBoolean("tax3IsOnlyTax", false);
        }

        private void SaveSettingsTextView_Click(object sender, EventArgs e)
        {
            float w, x, y, z;
            float.TryParse(mainTaxRateEditText.Text, out w);
            float.TryParse(tax1RateEditText.Text, out x);
            float.TryParse(tax2RateEditText.Text, out y);
            float.TryParse(tax3RateEditText.Text, out z);
			ISharedPreferencesEditor prefEditor = preferences.Edit ();

            prefEditor.PutBoolean("isFirstRunComplete", true);

			prefEditor.PutFloat ("mainRate", w);

			prefEditor.PutString ("tax1Desc", tax1DescEditText.Text);
            prefEditor.PutFloat("tax1Rate", x);
            prefEditor.PutBoolean("tax1IsPercent", tax1PercentRadioButton.Checked);
            prefEditor.PutBoolean("tax1IsOnlyTax", tax1CountedCheckbox.Checked);

            prefEditor.PutString("tax2Desc", tax2DescEditText.Text);
            prefEditor.PutFloat("tax2Rate", y);
            prefEditor.PutBoolean("tax2IsPercent", tax2PercentRadioButton.Checked);
            prefEditor.PutBoolean("tax2IsOnlyTax", tax2CountedCheckbox.Checked);

            prefEditor.PutString("tax3Desc", tax3DescEditText.Text);
            prefEditor.PutFloat("tax3Rate", z);
            prefEditor.PutBoolean("tax3IsPercent", tax3PercentRadioButton.Checked);
            prefEditor.PutBoolean("tax3IsOnlyTax", tax3CountedCheckbox.Checked);

            prefEditor.Apply ();

            Toast.MakeText (this, "Settings saved", ToastLength.Long).Show ();

            Finish();
			}
        }
    }