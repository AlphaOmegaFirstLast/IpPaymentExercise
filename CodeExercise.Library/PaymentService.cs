using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeExercise.Library
{
    public class PaymentService : IPaymentService
    {

        public string WhatsYourId()
        {
            return "2a8dec5a-5f70-45a2-9e0b-b14064850de0";
        }

        public bool IsCardNumberValid(string cardNumber)
        {
            bool validInput = false;

            if (!string.IsNullOrEmpty(cardNumber))
            {
                Regex regex = new Regex(@"^\d{16}$");
                Match isNumeric16Digit = regex.Match(cardNumber);

                if (isNumeric16Digit.Success)
                {
                    var validAlogarithm = applyLuhnAlgoritm(cardNumber);
                    if (validAlogarithm)
                    { return true; }
                    else
                    {
                        var faultCode = new FaultCode("Invalid CardNumber");
                        var faultReason = string.Format("CardNumber fails checksum. Actual CardNumber is {0}", cardNumber);

                        throw new FaultException(faultReason, faultCode);
                    }
                }
            }
            //-------------- if invalid cardNumber --------------
            if (!validInput)
            {
                // keep it simple
                var faultCode = new FaultCode("Invalid CardNumber");
                var faultReason = string.Format("CardNumber should be a string of 16 digits. Actual CardNumber is {0}", cardNumber);

                throw new FaultException(faultReason, faultCode);
            }
            return false;
        }

        private bool applyLuhnAlgoritm(string cardNumber)
        {
            try
            {
                var arChars = cardNumber.ToCharArray();
                var arNumbers = arChars.Select(x => int.Parse(x.ToString())).ToArray<int>(); //convert chars to ints

                var arSumDigits = new List<int>();
                var doDouble = true;
                for (var i = arNumbers.Length - 2; i >= 0; i--)  // (i = arNumbers.Length - 2) so we dont copy the last digit (the checkSum) and start applying the alogrithm from the second right most digit.
                {
                    if (doDouble)   //every second digit should be doubled
                    {
                        var arDoubledNumber = (arNumbers[i] * 2).ToString().ToCharArray();  // get the doubled number into array of digits
                        foreach (var digit in arDoubledNumber)
                        {
                            arSumDigits.Add(int.Parse(digit.ToString()));     //if the doubled number has 2 digits then both digits will be summed
                        }
                    }
                    else
                    {
                        arSumDigits.Add(arNumbers[i]);
                    }
                    doDouble = !doDouble;
                }
                var sum = arSumDigits.Sum();
                var checkSumDigit = ((sum * 9) % 10);
                return checkSumDigit == arNumbers[arNumbers.Length - 1];
            }
            catch (Exception ex)
            {
                //Log(ex);  using a logging tool like log4net

                //something wrong in code that needs Fix!
                var faultCode = new FaultCode("Internal Server Error");
                var faultReason = "Internal Server Error";

                throw new FaultException(faultReason, faultCode);
            }

        }

        public bool IsValidPaymentAmount(long amount)
        {
            var validAmount = ((amount >= 99) && (amount <= 99999999));
            if (!validAmount)
            {
                // keep it simple
                var faultCode = new FaultCode("Argument Out of Range");
                var faultReason = string.Format("Amount should be between 99 and 99999999 cents. Actual amount is {0}", amount);

                throw new FaultException(faultReason, faultCode);
            }
            return validAmount;
        }


        public bool CanMakePaymentWithCard(string cardNumber, int expiryMonth, int expiryYear)
        {
            var currentDate = DateTime.Today.ToUniversalTime();
            var validExpiryMonth = ((expiryMonth >= 1) && (expiryMonth <= 12));
            var validExpiryYear = ((expiryYear >= currentDate.Year) && (expiryYear <= (currentDate.Year + 10))); // business should provide the rule. assuming card expiry year within 10 years
            var isDateInFuture = true;

            if (validExpiryMonth && validExpiryYear) // if not valid then no need to perform a lengthy process and waste cpu time!
            {
                var expiryDate = new DateTime(year: expiryYear, month: expiryMonth, day: 1); // business should provide the rule, whether expiry date at the begening or end of the expiry month
                isDateInFuture = expiryDate >= currentDate;
                if (isDateInFuture)
                {
                    return IsCardNumberValid(cardNumber);
                }
            }
            else
            {
                if (!validExpiryMonth)
                {
                    var faultCode = new FaultCode("Argument Out of Range");
                    var faultReason = string.Format("ExpiryMonth should be between 1 and 12 cents. Actual ExpiryMonth is {0}", expiryMonth);

                    throw new FaultException(faultReason, faultCode);
                }
                if (!validExpiryYear)
                {
                    var faultCode = new FaultCode("Invalid Expiry Date");
                    var faultReason = string.Format("ExpiryYear should be greater than or equals current year + 10. Actual ExpiryYear is {0}", expiryYear);

                    throw new FaultException(faultReason, faultCode);
                }
            }
            if (!isDateInFuture)
            {
                var faultCode = new FaultCode("Invalid Expiry Date");
                var faultReason = string.Format("A valid expiry month and year should be in future. Actual Expiry Date is {0}/{0}", expiryMonth, expiryYear);

                throw new FaultException(faultReason, faultCode);
            }

            return false;
        }

    }
}
