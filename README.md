# API is intended to Convert Currencies


The purpose of this project is to meet the challenge of the company JAYA

This WEB API was developed in .NET 5 using the IDE Microsoft Visual Studio Community 2019, with the in-memory database and EntityFramework to save all transactions performed, the unit and integration tests, xUnit was used with FluentAssertions and Mock, as it was a challenge for the company and for me, I decided to use frameworks I wasn't familiar with, like .NET 5, Swagger, xUnit, FluentAssertions and Serilog

## The features are :
**CreateTransaction :** Creates a new currency conversion transaction according to source currency to destination currency

**GetAllTransactionsByUserId :** Fetch all transactions for a given user

## How to Run?
You will need .NET 5 and Visual Studio Community 2019

### Inside the IDE

Select the CurrencyConverterAPI project and run it as CurrencyConverterAPI, it will automatically open the browser according to the port marked in Properties/launchSettings.json

![Alt text](https://raw.githubusercontent.com/DanielFilippo/CurrencyConverterAPI/master/CurrencyConverterAPI/docs/image_01.png)

![Alt text](https://raw.githubusercontent.com/DanielFilippo/CurrencyConverterAPI/master/CurrencyConverterAPI/docs/image_02.png)

https://localhost:5001/swagger/index.html

It can also be run by IIS, it will also automatically open the browser according to the port marked in Properties/launchSettings.json

![Alt text](https://raw.githubusercontent.com/DanielFilippo/CurrencyConverterAPI/master/CurrencyConverterAPI/docs/image_03.png)

![Alt text](https://raw.githubusercontent.com/DanielFilippo/CurrencyConverterAPI/master/CurrencyConverterAPI/docs/image_04.png)

https://localhost:44339/swagger/index.html

### External API

As this project needs an External API to fetch currency quotes, where you can easily create a free account and get the access token at https://exchangeratesapi.io/

![Alt text](https://raw.githubusercontent.com/DanielFilippo/CurrencyConverterAPI/master/CurrencyConverterAPI/docs/image_05.png)

Once the account is created on the External API website, change the configuration in appsettings.json/appsettings.Development.json

![Alt text](https://raw.githubusercontent.com/DanielFilippo/CurrencyConverterAPI/master/CurrencyConverterAPI/docs/image_06.png)

Change the value of the "ACCESS_KEY" property to your new access token as shown in the image above

### Currency Converter API

CurrencyConverterAPI requires currencies to be passed with ISO 4217 standard code at [ISO_2417](https://docs.1010data.com/1010dataReferenceManual/DataTypesAndFormats/currencyUnitCodes.html) and may return some fault codes such as:

* 1001 - User does not exist
* 1002 - Exchangeratesapi API error (used to fetch factors)
* 1003 - Field {field} must be greater than 0
* 1004 - Need to inform a currency of origin/destination
* 1005 - Invalid currency(s) for conversion {currency}

![Alt text](https://raw.githubusercontent.com/DanielFilippo/CurrencyConverterAPI/master/CurrencyConverterAPI/docs/image_07.png)
