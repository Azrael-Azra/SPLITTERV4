

namespace SPLITTER
{
    public partial class MainPage : ContentPage
    {
        //Initialization
        private double billAmount = 0;
        private double tipPercentage = 0;
        private int numberOfPeople = 1;

        public MainPage()
        {
            InitializeComponent();
        }


        // Show or hide custom tip section
        private void OnCustomTipClicked(object sender, EventArgs e)
        {
            // Toggle visibility of the custom tip section
            CustomTipSection.IsVisible = !CustomTipSection.IsVisible;

            // If custom tip is visible, hide the slider section
            TipSliderSection.IsVisible = !CustomTipSection.IsVisible;

            // Clear the custom tip entry when switching
            CustomTipEntry.Text = string.Empty;

            // Reset slider and labels
            TipSlider.Value = 0;
        }

        // When a tip button is clicked (e.g., 5%, 10%, etc.)
        private void OnTipButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                // Parse the percentage from the button text
                var percentage = int.Parse(btn.Text.Replace("%", ""));

                // Update the tip percentage
                tipPercentage = percentage;
                // Update the slider value to match the percentage from the button
                TipSlider.Value = percentage; // Set the slider value

                // Reset custom tip section if a preset percentage is selected
                CustomTipSection.IsVisible = false;
                TipSliderSection.IsVisible = true;

                // Update the slider label with the selected percentage
                TipSliderLabel.Text = $"Tip Percentage: {tipPercentage}%";


                // Recalculate amounts based on the new tip percentage
                CalculateAmounts();
            }
        }

        // When the tip slider value changes (1-50%)
        private void OnTipSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            tipPercentage = Math.Round(e.NewValue); // Round to integer


            // Recalculate amounts based on the new slider value
            CalculateAmounts();
        }

        // When a custom tip amount is entered
        private void OnCustomTipEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            // Remove non-numeric characters (except decimal points)
            string enteredText = new string(e.NewTextValue.Where(c => char.IsDigit(c)).ToArray());

            if (enteredText.Length > 0)
            {
                // Parse the text as a number and divide by 100 to simulate monetary value
                double parsedCustomTip = double.Parse(enteredText) / 100;

                // Update the custom tip entry with formatted currency
                CustomTipEntry.Text = parsedCustomTip.ToString("0.00");
                CustomTipEntry.CursorPosition = CustomTipEntry.Text.Length;

                // Update the tip percentage based on the custom tip amount
                tipPercentage = billAmount > 0 ? (parsedCustomTip / billAmount) * 100 : 0;


                // Recalculate amounts after setting the custom tip
                CalculateAmounts();
            }
            else
            {
                // Reset custom tip if input is invalid
                tipPercentage = 0;
                CalculateAmounts();
            }
        }

        // When the bill amount entry is changed
        private void OnBillEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            // Remove non-numeric characters (except decimal points)
            string enteredText = new string(e.NewTextValue.Where(c => char.IsDigit(c)).ToArray());

            if (enteredText.Length > 0)
            {
                // Parse the text as a number and divide by 100 to place it in cents
                double parsedValue = double.Parse(enteredText) / 100;

                // Update the billAmount and the BillEntry with formatted currency
                billAmount = parsedValue;
                BillEntry.Text = parsedValue.ToString("0.00");

                // Move the cursor to the end of the text
                BillEntry.CursorPosition = BillEntry.Text.Length;

                CalculateAmounts();
            }
            else
            {
                // If input is invalid, set the bill amount to 0 or show an error
                billAmount = 0;
                TipAmountLabel.Text = "Php 0.00";
                TotalAmountLabel.Text = "Php 0.00";
            }
        }

        // Method to calculate tip amount and total bill per person
        private void CalculateAmounts()
        {
            // Calculate the total tip amount (before dividing by the number of people)
            double totalTipAmount = billAmount * (tipPercentage / 100);

            // Divide the tip amount and the total amount by the number of people
            double tipAmountPerPerson = totalTipAmount / numberOfPeople;
            double totalAmountPerPerson = (billAmount + totalTipAmount) / numberOfPeople;

            // Display the per-person values
            TipAmountLabel.Text = $"Php {tipAmountPerPerson:F2}";
            TotalAmountLabel.Text = $"Php {totalAmountPerPerson:F2}";
        }

        // When the number of people entry is changed
        private void OnPeopleEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            // Try to parse the number of people entered, ensure it's at least 1
            if (!int.TryParse(PeopleEntry?.Text, out numberOfPeople) || numberOfPeople <= 0)
            {
                numberOfPeople = 1; // Default to 1 if input is invalid or zero
            }

            // Recalculate amounts after setting the new number of people
            CalculateAmounts();
        }

    }

}
