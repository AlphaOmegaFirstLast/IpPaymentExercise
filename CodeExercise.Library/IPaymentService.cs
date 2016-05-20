using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CodeExercise.Library
{
    /// <summary>
    /// The contract required to be implemented by a payment service
    /// </summary>
    [ServiceContract]
    public interface IPaymentService
    {

        /// <summary>
        /// Returns the unique ID allocated to a candidate
        /// </summary>
        [OperationContract]
        string WhatsYourId();

        /// <summary>
        /// Performs a Mod-10/LUHN check on the passed number and returns true if the check passed
        /// </summary>
        /// <param name="cardNumber">A 16 digit card number</param>
        /// <returns>true if the card number is valid, otherwise false</returns>
        /// <remarks>
        /// Refer here for MOD10 algorithm: https://en.wikipedia.org/wiki/Luhn_algorithm
        /// </remarks>
        [OperationContract]
        bool IsCardNumberValid(string cardNumber);

        /// <summary>
        /// Checks if the amount represents a valid payment amount 
        /// </summary>
        /// <param name="amount">An amount value in cents (1 Dollar = 100 cents)</param>
        /// <remarks>
        /// Validation:
        /// The amount must be between 99 cents and 99999999 cents
        /// </remarks>
        [OperationContract]
        bool IsValidPaymentAmount(long amount);

        /// <summary>
        /// Validates the card number, expiry motnh and year to ensure the details can be used to make a payment
        /// </summary>
        /// <param name="cardNumber">A 16 digit card number</param>
        /// <param name="expiryMonth">Month part of the expiry date</param>
        /// <param name="expiryYear">Year part of the expiry date</param>
        /// <returns>true if the details represent a valid card, otherwise false</returns>
        /// <remarks>
        /// Validations:
        /// cardNumber: Ensure the passed string is 16 in length and passes the MOD10/LUHN check
        /// expiryMonth: should represent a month number between 1 and 12
        /// expiryYear: Should represent a year value, 4 characters in lenght and either the current or a future year
        /// The expiry month + year should represent a date in the future
        /// </remarks>
        [OperationContract]
        bool CanMakePaymentWithCard(string cardNumber, int expiryMonth, int expiryYear);
    }
}
