using System;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;

namespace FinalPrice
{
    [Activity(Label = "Welcome")]
    public class Welcome : Activity
    {
        TextView GetStartedTextView;
        ISharedPreferences preferences;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            preferences = Application.Context.GetSharedPreferences("TaxInfo", FileCreationMode.Private);
            //Set up default settings just to be safe in case some asshole presses back and kills activity
            ISharedPreferencesEditor prefEditor = preferences.Edit();

            prefEditor.PutBoolean("isFirstRunComplete", true);

            prefEditor.PutFloat("mainRate", 8.00f);

            prefEditor.PutString("tax1Desc", "Recycle");
            prefEditor.PutFloat("tax1Rate", 5.00f);
            prefEditor.PutBoolean("tax1IsPercent", false);
            prefEditor.PutBoolean("tax1IsOnlyTax", false);

            prefEditor.PutString("tax2Desc", "Tobacco");
            prefEditor.PutFloat("tax2Rate", 0.87f);
            prefEditor.PutBoolean("tax2IsPercent", false);
            prefEditor.PutBoolean("tax2IsOnlyTax", true);

            prefEditor.PutString("tax3Desc", "Bay Area");
            prefEditor.PutFloat("tax3Rate", 10.0f);
            prefEditor.PutBoolean("tax3IsPercent", true);
            prefEditor.PutBoolean("tax3IsOnlyTax", true);

            prefEditor.Apply();

            SetContentView(Resource.Layout.welcome);
            GetStartedTextView = FindViewById<TextView>(Resource.Id.getStartedTextView);
            GetStartedTextView.Click += (object sender, EventArgs e) =>
            {
                Intent intent = new Intent(this, typeof(Settings));
                StartActivity(intent);
                Finish();
            };
        }
    }
}