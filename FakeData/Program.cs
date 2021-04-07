using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Bogus;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace FakeData
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!int.TryParse(args[0], out int numberOfRecords))
                return;
            Faker faker;
            string country;
            switch (args[1])
            {
                case "en_US":
                    faker = new Faker("en_US");
                    country = "USA";
                    break;

                case "ru_RU":
                    faker = new Faker("ru");
                    country = "Российская Федерация";
                    break;

                case "uk_UA":
                    faker = new Faker("uk");
                    country = "Україна";
                    break;

                default:
                    return;
            }
            Console.OutputEncoding = Encoding.UTF8;
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";" };   
            using (var streamWriter = new StreamWriter(Console.OpenStandardOutput()))
                using (CsvWriter csvWriter = new CsvWriter(streamWriter, config))
                {
                    for (int i = 0; i < numberOfRecords; i++)
                    {
                        csvWriter.WriteField(faker.Name.FullName());

                        using (StringWriter stringWriter = new StringWriter())
                            using (var csvAddressWriter = new CsvWriter(stringWriter, CultureInfo.InvariantCulture))
                            {
                                csvAddressWriter.WriteField(country);
                                csvAddressWriter.WriteField(faker.Address.State());
                                csvAddressWriter.WriteField(faker.Address.City());
                                csvAddressWriter.WriteField(faker.Address.StreetName());
                                csvAddressWriter.WriteField(faker.Address.BuildingNumber());
                                csvAddressWriter.WriteField(faker.Address.SecondaryAddress());
                                csvAddressWriter.WriteField(faker.Address.ZipCode());
                                csvAddressWriter.NextRecord();
                                csvWriter.WriteField(stringWriter.ToString().Trim());
                            }
                        csvWriter.WriteField(faker.Phone.PhoneNumber());

                        csvWriter.NextRecord();
                    }
                }
        }
    }
}